using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

public class AiTelemetryData
{
	[JsonPropertyName("baseType")]
	public string BaseType { get; set; }

	[JsonPropertyName("baseData")]
	public AiBaseData BaseData { get; set; }
}
