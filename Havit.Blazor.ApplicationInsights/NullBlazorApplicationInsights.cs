using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// No-op implementation of <see cref="IBlazorApplicationInsights"/>.
/// All calls are silently ignored. Useful for server-side rendering or environments
/// where Application Insights tracking is not desired.
/// </summary>
internal class NullBlazorApplicationInsights : IBlazorApplicationInsights
{
	public Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task StartTrackPageAsync(string name = null)
		=> Task.CompletedTask;

	public Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> Task.CompletedTask;

	public Task StartTrackEventAsync(string name)
		=> Task.CompletedTask;

	public Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null)
		=> Task.CompletedTask;

	public Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null)
		=> Task.CompletedTask;

	public Task SetAuthenticatedUserContextAsync(string authenticatedUserId, string accountId = null, bool storeInCookie = false)
		=> Task.CompletedTask;

	public Task ClearAuthenticatedUserContextAsync()
		=> Task.CompletedTask;

	public Task TrackDependencyDataAsync(DependencyTelemetry dependency)
		=> Task.CompletedTask;

	public Task FlushAsync()
		=> Task.CompletedTask;

	public Task AddTelemetryInitializerAsync(TelemetryInitializer initializer)
		=> Task.CompletedTask;
}
