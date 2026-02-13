using System.Diagnostics;

namespace Havit.Blazor.Documentation.Mcp.Diagnostics;

/// <summary>
/// Shared ActivitySource for MCP tool invocations.
/// Activities are exported as OpenTelemetry spans to Application Insights (dependencies table).
/// </summary>
internal static class McpToolActivitySource
{
	public const string Name = "Havit.Blazor.Mcp.Tools";

	public static readonly ActivitySource Source = new(Name);
}
