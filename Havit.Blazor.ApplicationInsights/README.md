# Havit.Blazor.ApplicationInsights

Blazor integration for the [Application Insights JavaScript SDK](https://github.com/microsoft/ApplicationInsights-JS).
Targets browser-side telemetry. Best suited for Static SSR and Blazor WebAssembly scenarios, including mixed Blazor Web App setups. Blazor Server, server-side are also supported but telemetry is typically handled by the [Azure Application Insights SDK](https://learn.microsoft.com/azure/azure-monitor/app/asp-net-core) or Azure Monitor OpenTelemetry Exporter — this library adds value mainly for tracking browser exceptions.

Supports .NET 9 and .NET 10.

---

## Setup

### 1. Register services

**Server project** (`Program.cs`):
```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    options.JsSdkOptions.ConnectionString = "your-connection-string";
	...
});
```

**WebAssembly (Client) project** (`Program.cs`):
```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    ...
});
```

> In a Blazor Web App with WASM interactivity, register `AddBlazorApplicationInsights` in both projects.
> Each project uses its own configuration — the WASM client project must include all required options
> (e.g. the connection string) directly, as server-side configuration is not propagated to the client.

### 2. Add the script component

Place `<BlazorApplicationInsightsScript />` inside the `<head>` element of your root layout or `App.razor`,
**before** any other scripts:

```razor
@using Havit.Blazor.ApplicationInsights.Components

<head>
    ...
    <BlazorApplicationInsightsScript @rendermode="..." />
    ...
</head>
```

The component injects the Application Insights SDK snippet and handles both SSR (inline `<script>` tag)
and interactive rendering (`eval()` on first render).

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

---

## Configuration

Options are split into two groups.

### `JsSdkOptions` — JavaScript SDK configuration

Settings in `JsSdkOptions` are serialized to JSON and passed directly to the Application Insights JavaScript SDK.
They mirror the JS SDK's `IConfig` interface.

Commonly used properties:

| Property | Description |
|---|---|
| `ConnectionString` | Application Insights connection string (preferred over instrumentation key) |
| `InstrumentationKey` | Legacy instrumentation key |
| `EnableAutoRouteTracking` | Automatically track page views on Blazor navigation (`history.pushState`). Default: `false` |
| `DisableAjaxTracking` | Disable automatic tracking of AJAX/Fetch requests. Default: `false` |
| `DisableExceptionTracking` | Disable automatic tracking of unhandled exceptions. Default: `false` |
| `SamplingPercentage` | Percentage of telemetry to send (0–100). Default: `100` |

For the full list of JS SDK options see `ApplicationInsightsConfig` and `ApplicationInsightsConfiguration`.

### Blazor wrapper options

These options control behavior of the C# Blazor wrapper and are **not** forwarded to the JS SDK.

| Property | Description | Default |
|---|---|---|
| `EnableInitialPageViewTracking` | Whether the SDK snippet calls `trackPageView({})` on startup | `true` |

---

## Page view tracking

### Automatic tracking on initial load

By default, the SDK snippet calls `trackPageView({})` when it initializes. This covers the first page load
in all rendering modes. Disable it via `EnableInitialPageViewTracking = false` if you need full control
(e.g. to attach the authenticated user context first).

### Automatic tracking on Blazor navigation

Blazor navigation does not reload the page, so the snippet does not re-run. Enable `EnableAutoRouteTracking`
in `JsSdkOptions` to have the JS SDK automatically track page views when the URL changes:

```csharp
builder.Services.AddBlazorApplicationInsights(options =>
{
    options.JsSdkOptions.ConnectionString = "...";
    options.JsSdkOptions.EnableAutoRouteTracking = true;
});
```

### Manual page view tracking

```csharp
await AppInsights.TrackPageViewAsync(new PageViewTelemetry
{
    Name = "Product detail",
    Uri = new Uri("/products/42", UriKind.Relative)
});
```

### Timed page views

Use `EnterTrackPageScopeAsync` to measure how long a user spends on a page.
The scope calls `StartTrackPage` immediately and `StopTrackPage` when disposed:

```csharp
await using var scope = await AppInsights.EnterTrackPageScopeAsync("Product detail");
scope.Url = "/products/42";
// scope is stopped and sent when disposed
```

---

## Tracking API

### Custom events

```csharp
await AppInsights.TrackEventAsync(new EventTelemetry { Name = "add-to-cart" },
    customProperties: new Dictionary<string, object> { ["productId"] = 42 });
```

### Timed events

```csharp
await using var scope = await AppInsights.EnterTrackEventScopeAsync("checkout");
scope.Properties["paymentMethod"] = "card";
scope.Measurements["items"] = 3;
// event is stopped and sent when disposed
```

### Exceptions

> **Note:** The Application Insights JavaScript SDK only captures JavaScript errors (`window.onerror`).
> .NET exceptions in Blazor — whether they cause a circuit failure (Blazor Server) or are caught by
> `ErrorBoundary` — are invisible to the JS SDK and must be tracked manually.

Track a raw `ExceptionTelemetry`:
```csharp
await AppInsights.TrackExceptionAsync(new ExceptionTelemetry { ... });
```

Or use the convenience extension that accepts a C# `Exception` directly:
```csharp
await AppInsights.TrackExceptionAsync(exception, SeverityLevel.Error);
```

### ErrorBoundary integration

Use the `<BlazorApplicationInsightsException>` component inside an `ErrorBoundary`'s `ErrorContent`
to report caught exceptions automatically — no code-behind required:

```razor
@using Havit.Blazor.ApplicationInsights.Components

<ErrorBoundary>
    <ChildContent>
        ...
    </ChildContent>
    <ErrorContent Context="ex">
        <BlazorApplicationInsightsException Exception="ex" />
        <p>An error occurred. Please try again.</p>
    </ErrorContent>
</ErrorBoundary>
```

The component renders nothing. It tracks a new telemetry item whenever the `Exception` parameter
changes to a new instance, which covers scenarios where the `ErrorBoundary` is reset and a different
exception is caught next. In SSR and during prerendering the call is silently ignored (JS SDK is not
active yet); in interactive mode the exception is reported immediately.

### Traces

```csharp
await AppInsights.TrackTraceAsync(new TraceTelemetry
{
    Message = "Something happened",
    SeverityLevel = SeverityLevel.Warning
});
```

### Metrics

```csharp
await AppInsights.TrackMetricAsync(new MetricTelemetry
{
    Name = "cart-size",
    Average = 3.5
});
```

---

## Authenticated user context

Set the user identity so it appears on all subsequent telemetry (including auto-collected):

```csharp
await AppInsights.SetAuthenticatedUserContextAsync(authenticatedUserId: userId);
```

Clear it on sign-out:

```csharp
await AppInsights.ClearAuthenticatedUserContextAsync();
```


---

## Telemetry initializer

Use a telemetry initializer to attach static tags to **every** telemetry item, including auto-collected
(page views, exceptions, AJAX requests):

```csharp
await AppInsights.AddTelemetryInitializerAsync(new TelemetryInitializer
{
    CloudRoleName = "MyBlazorApp",
    ApplicationVersion = "1.2.3"
});
```

For tags not covered by the typed properties, use `Tags` directly:
```csharp
var initializer = new TelemetryInitializer();
initializer.Tags["ai.user.accountId"] = tenantId;
await AppInsights.AddTelemetryInitializerAsync(initializer);
```


---

## CSP nonce

If your Content Security Policy uses `nonce-*`, pass the nonce to the script component
to include it on the inline `<script>` tag (SSR scenario):

```razor
<BlazorApplicationInsightsScript Nonce="@cspNonce" />
```

---

## How it works

- **Static SSR and prerendering**: `BlazorApplicationInsightsScript` renders an inline `<script>` tag with the SDK snippet. The SDK initializes immediately in the browser as part of the HTML response.
- **Interactive after prerendering (Server / WASM)**: the script was already emitted inline during prerendering, so hydration skips re-injection. A `PersistentComponentState` flag signals this to the interactive phase.
- **Interactive without prerendering**: the snippet is injected via `eval()` on initialization, because no inline tag was emitted.
- All `IBlazorApplicationInsights` calls are silently ignored during prerendering and forwarded to the JS SDK once the interactive circuit is established.
