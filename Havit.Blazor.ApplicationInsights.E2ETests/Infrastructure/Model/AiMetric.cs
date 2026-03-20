using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

public class AiMetric
{
	[JsonPropertyName("name")]
	public string Name { get; set; }
}
