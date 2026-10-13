using System.Text.Json;
using System.Text.Json.Nodes;
using Havit.Blazor.ApplicationInsights.Options;

namespace Havit.Blazor.ApplicationInsights.Tests;

public class JsSdkOptionsSerializationTests
{
	[Fact]
	public void JsSdkOptions_FeatureOptIn_SerializesAsExpectedByJsSdk()
	{
		// Arrange
		var options = new BlazorApplicationInsightsJsSdkOptions
		{
			FeatureOptIn = new Dictionary<string, FeatureOptInDetails>
			{
				["zipPayload"] = new FeatureOptInDetails
				{
					Mode = FeatureOptInMode.Disable,
					OnCfg = new Dictionary<string, JsonNode>
					{
						["throttleMgrCfg.109.disabled"] = false,
						["maxBatchSizeInBytes"] = 1000,
						["appId"] = "x",
						["samplingPercentage"] = 12.5m,
						["redactQueryParams"] = new JsonArray("sig", "token"),
						["cookieCfg"] = new JsonObject { ["enabled"] = true }
					},
					BlockCdnCfg = true
				}
			}
		};

		// Act
		string json = JsonSerializer.Serialize(options, BlazorApplicationInsightsJsonSerializerContext.Default.BlazorApplicationInsightsJsSdkOptions);

		// Assert
		Assert.Equal("""{"featureOptIn":{"zipPayload":{"mode":2,"onCfg":{"throttleMgrCfg.109.disabled":false,"maxBatchSizeInBytes":1000,"appId":"x","samplingPercentage":12.5,"redactQueryParams":["sig","token"],"cookieCfg":{"enabled":true}},"blockCdnCfg":true}}}""", json);
	}
}
