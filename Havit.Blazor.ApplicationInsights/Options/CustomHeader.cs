// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IConfig.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// A custom HTTP header to be added to outgoing telemetry requests.
/// C# representation of the <c>{ header: string, value: string }</c> shape used in <c>IConfig.customHeaders</c>.
/// </summary>
public record class CustomHeader
{
	/// <summary>
	/// Header.
	/// </summary>
	[JsonPropertyName("header")]
	public string Header { get; set; }

	/// <summary>
	/// Value.
	/// </summary>
	[JsonPropertyName("value")]
	public string Value { get; set; }
}
