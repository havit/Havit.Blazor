using System.Text.Json;
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
					OnCfg = new Dictionary<string, object> { ["throttleMgrCfg.109.disabled"] = false, ["maxBatchSizeInBytes"] = 1000, ["appId"] = "x" },
					BlockCdnCfg = true
				}
			}
		};

		// Act
		string json = JsonSerializer.Serialize(options, BlazorApplicationInsightsJsonSerializerContext.Default.BlazorApplicationInsightsJsSdkOptions);

		// Assert
		Assert.Equal("""{"featureOptIn":{"zipPayload":{"mode":2,"onCfg":{"throttleMgrCfg.109.disabled":false,"maxBatchSizeInBytes":1000,"appId":"x"},"blockCdnCfg":true}}}""", json);
	}
}
