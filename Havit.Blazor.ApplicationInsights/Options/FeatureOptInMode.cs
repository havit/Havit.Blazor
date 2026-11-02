// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/enums/ai/FeatureOptInEnums.ts

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// Controls whether a feature steered by the SDK remote (CDN) configuration is applied.
/// C# representation of the JavaScript <c>FeatureOptInMode</c> enum.
/// Serialized as an integer value as expected by the Application Insights JS SDK.
/// </summary>
public enum FeatureOptInMode
{
	/// <summary>Not set, completely depends on the CDN configuration.</summary>
	None = 1,

	/// <summary>Try to not apply the configuration from the CDN.</summary>
	Disable = 2,

	/// <summary>Try to apply the configuration from the CDN.</summary>
	Enable = 3
}
