using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// A custom HTTP header to be added to outgoing telemetry requests.
/// C# representation of the <c>{ header: string, value: string }</c> shape used in <c>IConfig.customHeaders</c>.
/// </summary>
public record class CustomHeader
{
	[JsonPropertyName("header")]
	public string Header { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; }
}
