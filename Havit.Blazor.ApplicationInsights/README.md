# Havit.Blazor.ApplicationInsights

Blazor wrapper for the [Application Insights JavaScript SDK](https://github.com/microsoft/ApplicationInsights-JS).
Targets browser-side telemetry. Server-side rendering is also supported but telemetry is
typically handled by the [Application Insights](https://www.nuget.org/packages/Microsoft.ApplicationInsights)
or [Azure Monitor OpenTelemetry Exporter ](https://www.nuget.org/packages/Azure.Monitor.OpenTelemetry.Exporter).

CookieMgr not yet implemented.

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

Call `AddBlazorApplicationInsights` in both server project and WebAssembly client project (if any).
Configure **options** in the project where `<HxApplicationInsights>` is rendered.
If the component is prerendered or used in SSR, configure in the **server** project.
If rendered in WebAssembly without prerendering, configure in the **client** project.
 

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

## Configuration

Options are split into two groups.

### `JsSdkOptions` — JavaScript SDK configuration

Settings in `JsSdkOptions` are serialized to JSON and passed directly to the JS SDK.

Commonly used properties:

| Property | Description |
|---|---|
| `ConnectionString` | Application Insights connection string |
| `EnableAutoRouteTracking` | Automatically track page views on Blazor navigation (`history.pushState`). Default: `false` |
| `DisableAjaxTracking` | Disable automatic tracking of AJAX/Fetch requests. Default: `false` |
| `DisableExceptionTracking` | Disable automatic tracking of unhandled exceptions. Default: `false` |


### Blazor wrapper options

These options control behavior of the C# wrapper and are **not** forwarded to the JS SDK.

| Property | Description | Default |
|---|---|---|
| `EnableInitialPageViewTracking` | Whether the SDK snippet calls `trackPageView({})` on startup | `true` |
| `DefaultTelemetryInitializer` | Static tags applied to every telemetry item — registered before the initial page view, so the tags are present even on the auto-tracked page view on startup. See [Telemetry initializer](#telemetry-initializer). | `null` |

## Logging

The library includes an ASP.NET Core logging provider that forwards `ILogger` entries
to Application Insights as traces (or exceptions).
It is designed primarily for **Interactive WebAssembly**. In other render modes (SSR, Interactive Server),
log entries are silently discarded because no browser JS runtime is available server-side.

### Register the logging provider

**WebAssembly (Client) project** (`Program.cs`):
```csharp
// Required in Blazor WebAssembly — the WASM host does NOT read logging
// configuration from appsettings.json automatically (unlike server-side ASP.NET Core).
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Logging.AddBlazorApplicationInsights();
```

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

## Page view tracking

### Automatic tracking on initial load

By default, the SDK snippet calls `trackPageView({})` on initialization. Disable it via
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

## ErrorBoundary integration

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

Attach tags to every telemetry item. Tags are static (key-value pairs set at registration time)
because the JS SDK does not support dynamic callbacks from .NET code per telemetry item.

### Default telemetry initializer (via options)

Set once during registration and applies to all telemetry including the auto-tracked initial page view:

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

To attach tags dynamically, call `AddTelemetryInitializerAsync` at runtime.

```csharp
await AppInsights.AddTelemetryInitializerAsync(new TelemetryInitializer
{
    CloudRoleName = "MyBlazorApp",
    ApplicationVersion = "1.2.3"
});
```

## CSP nonce

If your Content Security Policy uses `nonce-*`, pass the nonce to the script component:

```razor
<HxApplicationInsights Nonce="@cspNonce" />
```
