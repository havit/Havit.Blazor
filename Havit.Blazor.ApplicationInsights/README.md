# Havit.Blazor.ApplicationInsights

Blazor integration for the [Application Insights JavaScript SDK](https://github.com/microsoft/ApplicationInsights-JS).
Supports all Blazor rendering modes — Static SSR, Blazor Server, and Blazor WebAssembly — including mixed Blazor Web App scenarios.

Requires .NET 9 or .NET 10.

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
> The full `JsSdkOptions` configured server-side are automatically propagated to the client via `PersistentComponentState`,
> so the WASM client project can omit them if they are only available server-side (e.g. connection strings from `appsettings.json`).

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

Track a raw `ExceptionTelemetry`:
```csharp
await AppInsights.TrackExceptionAsync(new ExceptionTelemetry { ... });
```

Or use the convenience extension that accepts a C# `Exception` directly:
```csharp
await AppInsights.TrackExceptionAsync(exception, SeverityLevel.Error);
```

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

- **Static SSR**: `BlazorApplicationInsightsScript` renders an inline `<script>` tag with the SDK snippet.
- **Interactive (Server / WASM)**: the snippet is injected via `eval()` on the first interactive render.
- **Blazor Web App (SSR + WASM)**: options configured server-side (e.g. connection strings from `appsettings.json`) are persisted via `PersistentComponentState` and merged into the WASM client options during hydration.
- All `IBlazorApplicationInsights` calls are no-ops during SSR prerendering and are forwarded to the JS SDK if the interactive circuit is established.
