# Havit.Blazor.ApplicationInsights

Blazor wrapper for the [Application Insights JavaScript SDK](https://github.com/microsoft/ApplicationInsights-JS).
Targets browser-side telemetry. Server-side rendering is also supported but telemetry is
typically handled by the [Application Insights](https://www.nuget.org/packages/Microsoft.ApplicationInsights)
or [Azure Monitor OpenTelemetry Exporter](https://www.nuget.org/packages/Azure.Monitor.OpenTelemetry.Exporter).

## Setup

### 1. Register services

**Server project** (`Program.cs`):
```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    options.JsSdkOptions.ConnectionString = "your-connection-string";
});
```

**WebAssembly (Client) project** (`Program.cs`):
```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    ...
});
```

Call `AddBlazorApplicationInsights` in both the server project and the WebAssembly client project (if any).
Configure **options** in the project where `<HxApplicationInsights>` is rendered:
if the component is prerendered or used in SSR, configure the **server** project;
if it is rendered in WebAssembly without prerendering, configure the **client** project.

### 2. Add the script component

Place `<HxApplicationInsights />` inside the `<head>` element of your root layout or `App.razor`,
**before** any other scripts:

```razor
@using Havit.Blazor.ApplicationInsights.Components

<head>
    ...
    <HxApplicationInsights @rendermode="..." />
    ...
</head>
```

The component emits the SDK snippet — inline as a `<script>` tag in SSR and prerendering,
via JS interop when it first renders interactively without prior prerendering.
The snippet then downloads the SDK itself from the Microsoft CDN.

### 3. Inject and use

```razor
@inject IBlazorApplicationInsights AppInsights

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await AppInsights.TrackEventAsync(new EventTelemetry { Name = "my-event" });
        }
    }
}
```

`IBlazorApplicationInsights` exposes the SDK API (`TrackEventAsync`, `TrackPageViewAsync`, `TrackExceptionAsync`,
`TrackTraceAsync`, `TrackMetricAsync`, `TrackDependencyDataAsync`, `Start/StopTrackPageAsync`,
`Start/StopTrackEventAsync`, `Set/ClearAuthenticatedUserContextAsync`, `AddTelemetryInitializerAsync`, `FlushAsync`, …).

## How the calls reach the SDK

Every call goes through a small wrapper (`window.havitBlazorAppInsights`) which the component installs together with the snippet.
The wrapper **holds the calls until the SDK is fully initialized** and then runs them in the order they were made.
You can therefore start tracking right away — from `OnAfterRenderAsync` of the first interactive component —
without waiting for the SDK download to finish.

Two situations are worth knowing about:

- **Prerendering.** During server-side prerendering there is no browser to talk to.
  Calls made at that point are **silently ignored**; once the interactive circuit is up, calls work normally.
  Issue telemetry from `OnAfterRenderAsync`, which does not run during prerendering. A call made from `OnInitialized[Async]`
  is dropped in the prerender pass — with prerendered interactivity it runs again in the interactive pass and is sent then,
  in static SSR it is simply lost.
- **SDK download failure.** If the SDK script cannot be loaded (all CDN fallbacks exhausted — offline, blocked by a proxy or an ad blocker),
  the wrapper releases the pending calls after 15 seconds and the snippet turns them into no-ops.
  The wait happens once; later calls pass through immediately. No exception is thrown.

From JavaScript, `await window.havitBlazorAppInsights.ready` resolves once the gate opens — with `true` when the SDK initialized,
with `false` when the 15-second fallback opened it. Resolution alone is not proof that the SDK loaded; check the value
(or `window.appInsights.core`) when that matters.

## Configuration

Options are split into two groups.

### `JsSdkOptions` — JavaScript SDK configuration

Settings in `JsSdkOptions` are serialized to JSON and passed directly to the JS SDK.
The class mirrors the SDK's [`IConfig`](https://github.com/microsoft/ApplicationInsights-JS#configuration); a property left `null` is not emitted, so the SDK default applies.

Commonly used properties:

| Property | Description | SDK default |
|---|---|---|
| `ConnectionString` | Application Insights connection string | — |
| `EnableAutoRouteTracking` | Track page views on Blazor navigation (`history.pushState`) | `false` |
| `DisableAjaxTracking` | Disable automatic tracking of XHR requests | `false` |
| `DisableFetchTracking` | Disable automatic tracking of Fetch requests | `false` |
| `DisableExceptionTracking` | Disable automatic tracking of unhandled JS exceptions | `false` |
| `SamplingPercentage` | Percentage of telemetry to send | `100` |

### Blazor wrapper options

These options control the behavior of the C# wrapper and are **not** forwarded to the JS SDK.

| Property | Description | Default |
|---|---|---|
| `EnableInitialPageViewTracking` | Track a page view (`trackPageView({})`) when the SDK starts | `true` |
| `DefaultTelemetryInitializer` | Static tags applied to every telemetry item — registered before the initial page view, so the tags are present even on that one. See [Telemetry initializer](#telemetry-initializer). | `null` |

## Logging

The library includes an ASP.NET Core logging provider that forwards `ILogger` entries
to Application Insights as traces (or exceptions when an exception is attached).

It is designed for **Interactive WebAssembly**. The provider is a singleton living outside any Blazor circuit,
so in SSR and Interactive Server it has no browser to reach and the log entries are **silently discarded**.

### Register the logging provider

**WebAssembly (Client) project** (`Program.cs`):
```csharp
// Required in Blazor WebAssembly — the WASM host does NOT read logging
// configuration from appsettings.json automatically (unlike server-side ASP.NET Core).
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Logging.AddBlazorApplicationInsights();
```

`AddBlazorApplicationInsights` on `ILoggingBuilder` also registers the services, so a separate
`builder.Services.AddBlazorApplicationInsights()` call is needed only when you want to configure the options.

### Configure log levels in `wwwroot/appsettings.json`

The provider alias is `BlazorApplicationInsights`. Use it to set per-provider log levels:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    },
    "BlazorApplicationInsights": {
      "LogLevel": {
        "Default": "Error",
        "MyApp.Pages": "Warning"
      }
    }
  }
}
```

> **Why is `AddConfiguration` needed?**
> Server-side ASP.NET Core (`WebApplicationBuilder`) reads `Logging` from `appsettings.json` automatically.
> Blazor WebAssembly (`WebAssemblyHostBuilder`) does not — you must call
> `builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))` explicitly.
> This applies to **all** logging providers in WASM, not just this one.

Each entry carries `CategoryName`, `EventId`/`EventName` (when set) and the values of active logging scopes as custom properties.
The provider buffers up to 1024 entries; when the buffer is full, the oldest entries are dropped.

## Page view tracking

### Automatic tracking on initial load

By default a page view is tracked when the SDK starts. Disable it via
`EnableInitialPageViewTracking = false` if you need full control.

### Automatic tracking on Blazor navigation

Enable `EnableAutoRouteTracking` to track page views on URL changes:

```csharp
options.JsSdkOptions.EnableAutoRouteTracking = true;
```

### Manual page view tracking

```csharp
await AppInsights.TrackPageViewAsync(new PageViewTelemetry
{
    Name = "Product detail",
    Uri = "/products/42"
});
```

### Timed page views and events

`EnterTrackPageScopeAsync` / `EnterTrackEventScopeAsync` pair `StartTrack…` with `StopTrack…` on dispose,
so the duration gets recorded even when the code in between throws:

```csharp
await using (var scope = await AppInsights.EnterTrackEventScopeAsync("checkout"))
{
    scope.Properties["step"] = "payment";
    scope.Measurements["items"] = cart.Items.Count;
    await ProcessPaymentAsync();
}
```

## Exceptions

### Tracking a .NET exception

`TrackExceptionAsync(Exception)` converts a .NET exception (type name, message and stack trace)
to Application Insights exception telemetry:

```csharp
catch (Exception ex)
{
    await AppInsights.TrackExceptionAsync(ex, SeverityLevel.Error);
}
```

### ErrorBoundary integration

.NET exceptions in Blazor are invisible to the JS SDK and must be tracked manually.
This component is useful only if you are not using the logging provider (`AddBlazorApplicationInsights` on `ILoggingBuilder`)
or if exceptions are not being written to `ILogger` in your application.

Use `<HxApplicationInsightsExceptionTracker>` inside an `ErrorBoundary` to report caught exceptions automatically:

```razor
@using Havit.Blazor.ApplicationInsights.Components

<ErrorBoundary>
    <ChildContent>
        ...
    </ChildContent>
    <ErrorContent Context="ex">
        <HxApplicationInsightsExceptionTracker Exception="ex" />
        <p>An error occurred. Please try again.</p>
    </ErrorContent>
</ErrorBoundary>
```

## Authenticated user context

```csharp
await AppInsights.SetAuthenticatedUserContextAsync(authenticatedUserId: userId);
```

Clear on sign-out:

```csharp
await AppInsights.ClearAuthenticatedUserContextAsync();
```

## Telemetry initializer

Attach tags to every telemetry item, including the telemetry the SDK collects on its own
(page views, requests, unhandled exceptions). Tags are static key-value pairs set at registration time —
the JS SDK cannot call back into .NET code for every item.

`TelemetryInitializer` offers typed properties for the common tags (`CloudRoleName` → `ai.cloud.role`,
`ApplicationVersion` → `ai.application.ver`); any other SDK tag goes into `Tags` directly.

### Default telemetry initializer (via options)

Set once during registration; applies to all telemetry including the initial page view:

```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    options.JsSdkOptions.ConnectionString = "...";
    options.DefaultTelemetryInitializer = new TelemetryInitializer
    {
        CloudRoleName = "MyBlazorApp",
        ApplicationVersion = "1.2.3"
    };
});
```

### Runtime telemetry initializer

To attach tags later (e.g. once the tenant is known), call `AddTelemetryInitializerAsync` at runtime.
The tags apply to the telemetry tracked from that point on:

```csharp
await AppInsights.AddTelemetryInitializerAsync(new TelemetryInitializer
{
    CloudRoleName = "MyBlazorApp",
    ApplicationVersion = "1.2.3",
    Tags = { ["ai.cloud.roleInstance"] = tenantId } // any other SDK tag
});
```

## Content Security Policy

If your policy uses `nonce-*`, pass the nonce to the component so that the inline snippet and the SDK script tag carry it:

```razor
<HxApplicationInsights Nonce="@cspNonce" />
```

Interactive render modes **without prerendering** inject the snippet through `eval()`, which a strict policy blocks
unless `script-src` allows `'unsafe-eval'`. Under a strict CSP, render the component in SSR or with prerendering
enabled so that the snippet is emitted inline.

## Limitations

- `CookieMgr` (the SDK cookie manager API) is not exposed.
- A handful of SDK configuration properties without a JSON representation (callbacks, `RegExp` patterns) are not available in `JsSdkOptions`;
  see the remarks on `ApplicationInsightsConfig` for the list.
