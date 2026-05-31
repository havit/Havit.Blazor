// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IEventTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Custom event telemetry.
/// C# representation of the JavaScript <c>IEventTelemetry</c> interface.
/// </summary>
public class EventTelemetry
{
	/// <summary>An event name string.</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }
}
