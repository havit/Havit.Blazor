using System.ComponentModel;
using System.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Services;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that provides component API documentation in Markdown format.
/// </summary>
internal class GetComponentDocsTool
{
	private readonly IComponentApiDocModelBuilder _modelBuilder;
	private readonly ComponentDocMarkdownRenderer _renderer;

	public GetComponentDocsTool(IComponentApiDocModelBuilder modelBuilder, ComponentDocMarkdownRenderer renderer)
	{
		_modelBuilder = modelBuilder;
		_renderer = renderer;
	}

	/// <summary>
	/// Returns the API documentation for a HAVIT Blazor component (or type) in Markdown format.
	/// </summary>
	[McpServerTool(Name = "get_component_docs", Title = "Get component documentation", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns the API documentation (parameters, properties, events, methods) for a HAVIT Blazor component or type. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")]
	public string GetComponentDocs(
		[Description("Name of the component or type to get documentation for, e.g. 'HxButton', 'HxGrid', 'ThemeColor'.")] string componentName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_docs");
		activity?.SetTag("mcp.tool.parameter.componentName", componentName);

		Type type = ApiTypeHelper.GetType(componentName, includeTypesContainingTypeName: true);
		if (type is null)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"Component or type '{componentName}' not found. Make sure the name matches a HAVIT Blazor component (e.g. HxButton, HxGrid, HxInputText, ThemeColor).";
		}

		ComponentApiDocModel model = _modelBuilder.BuildModel(type);
		string markdown = _renderer.Render(model);

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.resolvedType", type.FullName);

		return markdown;
	}
}
