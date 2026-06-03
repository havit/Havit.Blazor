// Based on the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCommon/src/Interfaces/IAppInsights.ts

using System.ComponentModel;
using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Core Application Insights telemetry tracking interface.
/// C# representation of the JavaScript <c>IAppInsights</c> interface
/// from <c>@microsoft/applicationinsights-common</c>.
/// </summary>
/// <remarks>
/// Adapted members (exposed with a C# friendly API, not a direct 1:1 mapping):
/// <list type="bullet">
///   <item><c>addTelemetryInitializer()</c> – exposed via <see cref="IApplicationInsights.AddTelemetryInitializerAsync"/></item>
/// </list>
/// Skipped members (JS-specific, not applicable in C#):
/// <list type="bullet">
///   <item><c>_onerror()</c> – internal auto-exception handler for <c>window.onerror</c></item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IAppInsights
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
