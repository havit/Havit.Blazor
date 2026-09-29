using Havit.Blazor.ApplicationInsights.Components;
using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Browser implementation of <see cref="IBlazorApplicationInsights"/>.
/// Delegates every call to the Application Insights JS SDK via <see cref="IJSRuntime"/>, through the readiness gate
/// (<c>window.havitBlazorAppInsights</c>) installed by the bootstrap module of <see cref="HxApplicationInsights"/>.
/// </summary>
public class BrowserBlazorApplicationInsights : IBlazorApplicationInsights
{
	// IJSRuntime is a SCOPED dependency.
	private readonly IJSRuntime _jsRuntime;

	private Task _gateTask;

	/// <summary>
	/// Constructs a new instance of <see cref="BrowserBlazorApplicationInsights"/>.
	/// </summary>
	public BrowserBlazorApplicationInsights(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	/// <inheritdoc/>
	public Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackEvent", telemetry, customProperties);

	/// <inheritdoc/>
	public Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackPageView", telemetry, customProperties);

	/// <inheritdoc/>
	public Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackException", telemetry, customProperties);

	/// <inheritdoc/>
	public Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackTrace", telemetry, customProperties);

	/// <inheritdoc/>
	public Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackMetric", telemetry, customProperties);

	/// <inheritdoc/>
	public Task StartTrackPageAsync(string name = null)
		=> InvokeGateAsync("startTrackPage", name);

	/// <inheritdoc/>
	public Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> InvokeGateAsync("stopTrackPage", name, url, properties, measurements);

	/// <inheritdoc/>
	public Task StartTrackEventAsync(string name)
		=> InvokeGateAsync("startTrackEvent", name);

	/// <inheritdoc/>
	public Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> InvokeGateAsync("stopTrackEvent", name, properties, measurements);

	/// <inheritdoc/>
	public Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeGateAsync("trackPageViewPerformance", telemetry, customProperties);

	/// <inheritdoc/>
	public Task SetAuthenticatedUserContextAsync(string authenticatedUserId, string accountId = null, bool storeInCookie = false)
		=> InvokeGateAsync("setAuthenticatedUserContext", authenticatedUserId, accountId, storeInCookie);

	/// <inheritdoc/>
	public Task ClearAuthenticatedUserContextAsync()
		=> InvokeGateAsync("clearAuthenticatedUserContext");

	/// <inheritdoc/>
	public Task TrackDependencyDataAsync(DependencyTelemetry dependency)
		=> InvokeGateAsync("trackDependencyData", dependency);

	/// <inheritdoc/>
	public Task FlushAsync()
		=> InvokeGateAsync("flush");

	/// <inheritdoc/>
	public Task AddTelemetryInitializerAsync(TelemetryInitializer initializer)
		=> InvokeGateAsync("addTelemetryInitializer", initializer.GetTags());

	/// <summary>
	/// Makes sure the gate exists and invokes its method. The gate holds the call until the SDK is initialized, so a call made
	/// before <see cref="HxApplicationInsights"/> got to run the snippet waits instead of failing with
	/// "Could not find 'havitBlazorAppInsights.…'".
	/// </summary>
	private async Task InvokeGateAsync(string method, params object[] args)
	{
		await EnsureGateAsync();
		await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights." + method, args);
	}

	private Task EnsureGateAsync()
	{
		var gateTask = _gateTask;
		if ((gateTask == null) || gateTask.IsFaulted || gateTask.IsCanceled)
		{
			// A faulted attempt is not kept: during prerendering the JS runtime throws (AdaptiveBlazorApplicationInsights
			// relies on seeing that exception), and the import has to be retried once the circuit is interactive.
			gateTask = ImportBootstrapModuleAsync();
			_gateTask = gateTask;
		}

		return gateTask;
	}

	private async Task ImportBootstrapModuleAsync()
	{
		// The module has no exports - importing it is what installs window.havitBlazorAppInsights (idempotently).
		// The module reference itself is of no further use.
		var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", HxApplicationInsights.BootstrapModulePath);
		if (module != null)
		{
			await module.DisposeAsync();
		}
	}
}
