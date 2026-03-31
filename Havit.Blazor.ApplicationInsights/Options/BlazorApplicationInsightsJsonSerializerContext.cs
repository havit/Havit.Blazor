using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false)]
[JsonSerializable(typeof(BlazorApplicationInsightsJsSdkOptions))]
[JsonSerializable(typeof(IDictionary<string, string>), TypeInfoPropertyName = "TelemetryInitializerDictionary")]
internal partial class BlazorApplicationInsightsJsonSerializerContext : JsonSerializerContext
{
}
