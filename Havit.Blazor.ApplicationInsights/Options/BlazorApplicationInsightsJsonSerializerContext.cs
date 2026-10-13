using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false)]
[JsonSerializable(typeof(BlazorApplicationInsightsJsSdkOptions))]
[JsonSerializable(typeof(IDictionary<string, string>), TypeInfoPropertyName = "TelemetryInitializerDictionary")]
// runtime types of the FeatureOptInDetails.OnCfg / OffCfg dictionary values (object)
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(string))]
internal partial class BlazorApplicationInsightsJsonSerializerContext : JsonSerializerContext
{
}
