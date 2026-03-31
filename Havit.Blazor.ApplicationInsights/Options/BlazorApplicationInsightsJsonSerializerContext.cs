using System.Text.Json.Serialization;
using Havit.Blazor.ApplicationInsights.JsSdk;

namespace Havit.Blazor.ApplicationInsights.Options;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false)]
[JsonSerializable(typeof(BlazorApplicationInsightsJsSdkOptions))]
[JsonSerializable(typeof(CookieMgrConfig))]
[JsonSerializable(typeof(IDictionary<string, string>), TypeInfoPropertyName = "TelemetryInitializerDictionary")]
internal partial class BlazorApplicationInsightsJsonSerializerContext : JsonSerializerContext
{
}
