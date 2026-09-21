using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Browser implementation of <see cref="IBlazorApplicationInsights"/>.
/// Delegates every call to the Application Insights JS SDK via <see cref="IJSRuntime"/>.
/// </summary>
public class BrowserBlazorApplicationInsights : IBlazorApplicationInsights
{
	// IJSRuntime is a SCOPED dependency.
	private readonly IJSRuntime _jsRuntime;

	/// <summary>
	/// Constructs a new instance of <see cref="BrowserBlazorApplicationInsights"/>.
	/// </summary>
	public BrowserBlazorApplicationInsights(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	/// <inheritdoc/>
	public async Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackEvent", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackPageView", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackException", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackTrace", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackMetric", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task StartTrackPageAsync(string name = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.startTrackPage", name);

	/// <inheritdoc/>
	public async Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.stopTrackPage", name, url, properties, measurements);

	/// <inheritdoc/>
	public async Task StartTrackEventAsync(string name)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.startTrackEvent", name);

	/// <inheritdoc/>
	public async Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.stopTrackEvent", name, properties, measurements);

	/// <inheritdoc/>
	public async Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackPageViewPerformance", telemetry, customProperties);

	/// <inheritdoc/>
	public async Task SetAuthenticatedUserContextAsync(string authenticatedUserId, string accountId = null, bool storeInCookie = false)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.setAuthenticatedUserContext", authenticatedUserId, accountId, storeInCookie);

	/// <inheritdoc/>
	public async Task ClearAuthenticatedUserContextAsync()
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.clearAuthenticatedUserContext");

	/// <inheritdoc/>
	public async Task TrackDependencyDataAsync(DependencyTelemetry dependency)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.trackDependencyData", dependency);

	/// <inheritdoc/>
	public async Task FlushAsync()
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.flush");

	/// <inheritdoc/>
	public async Task AddTelemetryInitializerAsync(TelemetryInitializer initializer)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.addTelemetryInitializer", initializer.GetTags());
}
