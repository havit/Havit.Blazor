using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Application Insights tracking service.
/// C# representation of the JavaScript <c>IAppInsights</c> interface.
/// </summary>
/// <remarks>
/// Skipped members (JS-specific, not applicable in C#):
/// <list type="bullet">
///   <item><c>getCookieMgr()</c> – returns a browser-specific cookie manager instance</item>
///   <item><c>addTelemetryInitializer()</c> – registers a JS callback</item>
///   <item><c>_onerror()</c> – internal auto-exception handler for <c>window.onerror</c></item>
/// </list>
/// </remarks>
public interface IBlazorApplicationInsights
{
	/// <summary>Tracks a custom event.</summary>
	Task TrackEventAsync(EventTelemetry telemetry, Dictionary<string, object> customProperties = null);

	/// <summary>Tracks a page view.</summary>
	Task TrackPageViewAsync(PageViewTelemetry telemetry = null, Dictionary<string, object> customProperties = null);

	/// <summary>Tracks an exception.</summary>
	Task TrackExceptionAsync(ExceptionTelemetry telemetry, Dictionary<string, object> customProperties = null);

	/// <summary>Tracks a trace (log) message.</summary>
	Task TrackTraceAsync(TraceTelemetry telemetry, Dictionary<string, object> customProperties = null);

	/// <summary>Tracks a custom metric.</summary>
	Task TrackMetricAsync(MetricTelemetry telemetry, Dictionary<string, object> customProperties = null);

	/// <summary>Starts timing a named page view. Call <see cref="StopTrackPageAsync"/> to complete it.</summary>
	Task StartTrackPageAsync(string name = null);

	/// <summary>
	/// Stops timing a page view started by <see cref="StartTrackPageAsync"/> and sends the telemetry.
	/// </summary>
	Task StopTrackPageAsync(string name = null, string url = null, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null);

	/// <summary>Starts timing a named event. Call <see cref="StopTrackEventAsync"/> to complete it.</summary>
	Task StartTrackEventAsync(string name);

	/// <summary>
	/// Stops timing an event started by <see cref="StartTrackEventAsync"/> and sends the telemetry.
	/// </summary>
	Task StopTrackEventAsync(string name, Dictionary<string, string> properties = null, Dictionary<string, double> measurements = null);

	/// <summary>Tracks page view performance metrics.</summary>
	Task TrackPageViewPerformanceAsync(PageViewPerformanceTelemetry telemetry, Dictionary<string, object> customProperties = null);
}
