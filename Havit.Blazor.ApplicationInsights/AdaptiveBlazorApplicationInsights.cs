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
	public async Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackEventAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackPageViewAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackExceptionAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackTraceAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackMetricAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task StartTrackPageAsync(string name = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StartTrackPageAsync(name));

	/// <inheritdoc/>
	public async Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StopTrackPageAsync(name, url, properties, measurements));

	/// <inheritdoc/>
	public async Task StartTrackEventAsync(string name)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StartTrackEventAsync(name));

	/// <inheritdoc/>
	public async Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.StopTrackEventAsync(name, properties, measurements));

	/// <inheritdoc/>
	public async Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackPageViewPerformanceAsync(telemetry, customProperties));

	/// <inheritdoc/>
	public async Task SetAuthenticatedUserContextAsync(string authenticatedUserId, string accountId = null, bool storeInCookie = false)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.SetAuthenticatedUserContextAsync(authenticatedUserId, accountId, storeInCookie));

	/// <inheritdoc/>
	public async Task ClearAuthenticatedUserContextAsync()
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.ClearAuthenticatedUserContextAsync());

	/// <inheritdoc/>
	public async Task TrackDependencyDataAsync(DependencyTelemetry dependency)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.TrackDependencyDataAsync(dependency));

	/// <inheritdoc/>
	public async Task FlushAsync()
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.FlushAsync());

	/// <inheritdoc/>
	public async Task AddTelemetryInitializerAsync(TelemetryInitializer initializer)
		=> await InvokeJsSafeAsync(() => _browserBlazorApplicationInsights.AddTelemetryInitializerAsync(initializer));
}
