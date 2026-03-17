using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Server-side implementation of <see cref="IBlazorApplicationInsights"/> that adapts to the current
/// render mode. During prerendering (where JS is unavailable), all calls are silently ignored.
/// Once the interactive circuit is established and JS becomes available, calls are forwarded
/// to <see cref="BrowserBlazorApplicationInsights"/>.
/// </summary>
/// <remarks>
/// Use this implementation on the server side (registered automatically by
/// <c>AddBlazorApplicationInsights()</c> when not running in a browser/WASM context).
/// </remarks>
internal class AdaptiveBlazorApplicationInsights : IBlazorApplicationInsights
{
	private readonly BrowserBlazorApplicationInsights _browserBlazorApplicationInsights;
	private bool? _isJsAvailable; // null = not yet tried, false = prerender/SSR, true = JS works

	public AdaptiveBlazorApplicationInsights(BrowserBlazorApplicationInsights browserBlazorApplicationInsights)
	{
		_browserBlazorApplicationInsights = browserBlazorApplicationInsights;
	}

	private async Task InvokeJsSafeAsync(Func<Task> browserInvocation)
	{
		if (_isJsAvailable == false)
		{
			return;
		}

		try
		{
			await browserInvocation();
			_isJsAvailable = true;
		}
		catch (InvalidOperationException) when (_isJsAvailable is null)
		{
			// JS runtime is not available during prerendering.
			// The 'when' guard ensures this handler only fires on the initial probe
			// (_isJsAvailable == null). Once JS availability has been confirmed
			// (_isJsAvailable == true), any subsequent exception propagates normally.
			_isJsAvailable = false;
		}
	}

	/// <inheritdoc/>
	public Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackEventAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackPageViewAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackExceptionAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackTraceAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackMetricAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public Task StartTrackPageAsync(string name = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StartTrackPageAsync(name));

	/// <inheritdoc/>
	public Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StopTrackPageAsync(name, url, properties, measurements));

	/// <inheritdoc/>
	public Task StartTrackEventAsync(string name)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StartTrackEventAsync(name));

	/// <inheritdoc/>
	public Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StopTrackEventAsync(name, properties, measurements));

	/// <inheritdoc/>
	public Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackPageViewPerformanceAsync(telemetry, customProperties));
}
