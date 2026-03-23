namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Provides static tag values applied to every telemetry envelope by the Application Insights
/// JavaScript SDK — including auto-collected telemetry (page views, XHR requests, unhandled exceptions).
/// </summary>
/// <seealso cref="TelemetryInitializer"/>
public interface ITelemetryInitializer
{
	/// <summary>
	/// Returns the dictionary of AI SDK tags to be applied to all telemetry envelopes.
	/// </summary>
	IDictionary<string, string> GetTags();
}