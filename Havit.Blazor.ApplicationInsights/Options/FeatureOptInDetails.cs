// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IFeatureOptIn.ts

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// Opt-in details of a single feature steered by the SDK remote (CDN) configuration.
/// C# representation of the JavaScript <c>IFeatureOptInDetails</c> interface.
/// </summary>
public class FeatureOptInDetails
{
	/// <summary>
	/// Sets the feature opt-in mode.
	/// </summary>
	[JsonPropertyName("mode")]
	public FeatureOptInMode? Mode { get; set; }

	/// <summary>
	/// Identifies configuration override values when the given feature is enabled.
	/// Use flat string field names, e.g. <c>"extensionConfig.Analytics.disableAjaxTracking"</c>.
	/// </summary>
	[JsonPropertyName("onCfg")]
	public IDictionary<string, JsonNode> OnCfg { get; set; }

	/// <summary>
	/// Identifies configuration override values when the given feature is disabled.
	/// Use flat string field names, e.g. <c>"extensionConfig.Analytics.disableAjaxTracking"</c>.
	/// </summary>
	[JsonPropertyName("offCfg")]
	public IDictionary<string, JsonNode> OffCfg { get; set; }

	/// <summary>
	/// Defines whether any changes from the CDN configuration should be blocked.
	/// If set to <c>true</c>, the configured value is applied under all scenarios. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("blockCdnCfg")]
	public bool? BlockCdnCfg { get; set; }
}
