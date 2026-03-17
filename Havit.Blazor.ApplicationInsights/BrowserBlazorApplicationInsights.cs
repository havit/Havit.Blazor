using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Browser implementation of <see cref="IBlazorApplicationInsights"/>.
/// Delegates every call to the Application Insights JS SDK via <see cref="IJSRuntime"/>.
/// </summary>
public class BrowserBlazorApplicationInsights : IBlazorApplicationInsights
{
	private readonly IJSRuntime _jsRuntime;

	public BrowserBlazorApplicationInsights(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	/// <inheritdoc/>
	public Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackEvent", telemetry, customProperties).AsTask();

	/// <inheritdoc/>
	public Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackPageView", telemetry, customProperties).AsTask();

	/// <inheritdoc/>
	public Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackException", telemetry, customProperties).AsTask();

	/// <inheritdoc/>
	public Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackTrace", telemetry, customProperties).AsTask();

	/// <inheritdoc/>
	public Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackMetric", telemetry, customProperties).AsTask();

	/// <inheritdoc/>
	public Task StartTrackPageAsync(string name = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.startTrackPage", name).AsTask();

	/// <inheritdoc/>
	public Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.stopTrackPage", name, url, properties, measurements).AsTask();

	/// <inheritdoc/>
	public Task StartTrackEventAsync(string name)
		=> _jsRuntime.InvokeVoidAsync("appInsights.startTrackEvent", name).AsTask();

	/// <inheritdoc/>
	public Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.stopTrackEvent", name, properties, measurements).AsTask();

	/// <inheritdoc/>
	public Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> _jsRuntime.InvokeVoidAsync("appInsights.trackPageViewPerformance", telemetry, customProperties).AsTask();
}
