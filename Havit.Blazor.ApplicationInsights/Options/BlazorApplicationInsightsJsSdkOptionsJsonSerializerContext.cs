using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false)]
[JsonSerializable(typeof(BlazorApplicationInsightsJsSdkOptions))]
internal partial class BlazorApplicationInsightsJsSdkOptionsJsonSerializerContext : JsonSerializerContext
{
}
