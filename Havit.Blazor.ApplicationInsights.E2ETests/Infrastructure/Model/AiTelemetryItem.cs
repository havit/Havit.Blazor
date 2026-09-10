using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;

public class AiTelemetryItem
{
	[JsonPropertyName("data")]
	public AiTelemetryData Data { get; set; }

	[JsonPropertyName("tags")]
	public Dictionary<string, string> Tags { get; set; }

	public string BaseType => Data?.BaseType;
	public string AuthUserId => Tags?.GetValueOrDefault("ai.user.authUserId");
	public string CloudRoleName => Tags?.GetValueOrDefault("ai.cloud.role");
}
