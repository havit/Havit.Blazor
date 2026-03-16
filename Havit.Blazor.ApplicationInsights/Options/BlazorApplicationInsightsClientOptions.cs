using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

public record class BlazorApplicationInsightsClientOptions // record for easy cloning by with { }
{
	[JsonPropertyName("connectionString")]
	public string ConnectionString { get; set; }

	internal void MergeTo(BlazorApplicationInsightsClientOptions target)
	{
		ArgumentNullException.ThrowIfNull(target);

		// TODO: Implement
	}
}