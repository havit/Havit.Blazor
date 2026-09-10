// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IMetricTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Custom metric telemetry.
/// C# representation of the JavaScript <c>IMetricTelemetry</c> interface.
/// </summary>
public class MetricTelemetry
{
	/// <summary>Name of this metric.</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Recorded value / average for this metric.</summary>
	[JsonPropertyName("average")]
	public double Average { get; set; }

	/// <summary>Number of samples represented by the average. Default: 1.</summary>
	[JsonPropertyName("sampleCount")]
	public int? SampleCount { get; set; }

	/// <summary>Smallest measurement in the sample. Defaults to <see cref="Average"/>.</summary>
	[JsonPropertyName("min")]
	public double? Min { get; set; }

	/// <summary>Largest measurement in the sample. Defaults to <see cref="Average"/>.</summary>
	[JsonPropertyName("max")]
	public double? Max { get; set; }

	/// <summary>Standard deviation of the sample. Defaults to zero.</summary>
	[JsonPropertyName("stdDev")]
	public double? StdDev { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }
}
