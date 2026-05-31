// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCommon/src/Interfaces/IDependencyTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Dependency call telemetry for manually tracking outgoing HTTP requests or other external calls.
/// C# representation of the JavaScript <c>IDependencyTelemetry</c> interface.
/// </summary>
public class DependencyTelemetry
{
	/// <summary>
	/// Unique identifier for this dependency call instance.
	/// Used for correlation between the dependency call and the request that initiated it.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>Name of the dependency (e.g. HTTP method + URL host).</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Duration of the dependency call in milliseconds.</summary>
	[JsonPropertyName("duration")]
	public double? Duration { get; set; }

	/// <summary>Whether the dependency call was successful.</summary>
	[JsonPropertyName("success")]
	public bool? Success { get; set; }

	/// <summary>Time when the dependency call was initiated.</summary>
	[JsonPropertyName("startTime")]
	public DateTimeOffset? StartTime { get; set; }

	/// <summary>HTTP status code or result code of the dependency call.</summary>
	[JsonPropertyName("responseCode")]
	public int ResponseCode { get; set; }

	/// <summary>Correlation context passed via request headers.</summary>
	[JsonPropertyName("correlationContext")]
	public string CorrelationContext { get; set; }

	/// <summary>
	/// Type of the dependency (e.g. <c>"Http"</c>, <c>"SQL"</c>, <c>"ServiceBus"</c>).
	/// Controls how the call appears in the Application Insights portal.
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; }

	/// <summary>
	/// Command or request detail — for HTTP calls typically the full URL,
	/// for database calls the SQL statement.
	/// </summary>
	[JsonPropertyName("data")]
	public string Data { get; set; }

	/// <summary>Target host or resource (e.g. <c>"example.com"</c> for HTTP calls).</summary>
	[JsonPropertyName("target")]
	public string Target { get; set; }

	/// <summary>Custom string properties to attach to this dependency call.</summary>
	[JsonPropertyName("properties")]
	public Dictionary<string, object> Properties { get; set; }

	/// <summary>Custom numeric measurements to attach to this dependency call.</summary>
	[JsonPropertyName("measurements")]
	public Dictionary<string, double> Measurements { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }
}
