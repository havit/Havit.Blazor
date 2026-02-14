using System.ComponentModel;
using System.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Services;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that provides type API documentation in Markdown format.
/// Targets supporting types such as enums, settings classes, delegates, etc.
/// </summary>
internal class GetTypeDocTool
{
	private readonly IApiDocModelBuilder _modelBuilder;
	private readonly McpDocMarkdownRenderer _renderer;

	public GetTypeDocTool(IApiDocModelBuilder modelBuilder, McpDocMarkdownRenderer renderer)
	{
		_modelBuilder = modelBuilder;
		_renderer = renderer;
	}

	/// <summary>
	/// Returns the API documentation for a HAVIT Blazor type (enum, settings class, delegate, etc.) in Markdown format.
	/// </summary>
	[McpServerTool(Name = "get_type_doc", Title = "Get type documentation", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns the API documentation (properties, enum values, delegate signature) for a HAVIT Blazor supporting type such as an enum, settings class, or delegate. Provide the type name, e.g. 'ThemeColor', 'GridSettings', 'CalendarDateCustomizationProviderDelegate'.")]
	public string GetTypeDoc(
		[Description("Name of the type to get documentation for, e.g. 'ThemeColor', 'GridSettings', 'CalendarDateCustomizationProviderDelegate'.")] string typeName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_type_doc");
		activity?.SetTag("mcp.tool.parameter.typeName", typeName);

		Type type = ApiTypeHelper.GetType(typeName, includeTypesContainingTypeName: true);
		if (type is null)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"Type '{typeName}' not found. Make sure the name matches a HAVIT Blazor type (e.g. ThemeColor, GridSettings, CalendarDateCustomizationProviderDelegate).";
		}

		ApiDocModel model = _modelBuilder.BuildModel(type);
		string markdown = _renderer.RenderTypeDoc(model);

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.resolvedType", type.FullName);

		return markdown;
	}
}
