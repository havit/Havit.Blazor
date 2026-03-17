// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/ITraceTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Trace (log) telemetry.
/// C# representation of the JavaScript <c>ITraceTelemetry</c> interface.
/// </summary>
public class TraceTelemetry
{
	/// <summary>A message string.</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	/// <summary>Severity level used for filtering in the portal.</summary>
	[JsonPropertyName("severityLevel")]
	public SeverityLevel? SeverityLevel { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }
}
