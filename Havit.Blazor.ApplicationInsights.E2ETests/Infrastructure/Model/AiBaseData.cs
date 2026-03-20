using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

public class AiBaseData
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

	[JsonPropertyName("metrics")]
	public AiMetric[] Metrics { get; set; }

	[JsonPropertyName("exceptions")]
	public AiException[] Exceptions { get; set; }
}
