using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

public class AiException
{
	[JsonPropertyName("typeName")]
	public string TypeName { get; set; }
}
