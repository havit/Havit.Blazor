using System.ComponentModel;
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
	[McpServerTool(Name = "get_component_docs")]
	[Description("Returns the API documentation (parameters, properties, events, methods) for a HAVIT Blazor component or type. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")]
	public string GetComponentDocs(
		[Description("Name of the component or type to get documentation for, e.g. 'HxButton', 'HxGrid', 'ThemeColor'.")] string componentName)
	{
		Type type = ApiTypeHelper.GetType(componentName, includeTypesContainingTypeName: false);
		if (type is null)
		{
			return $"Component or type '{componentName}' not found. Make sure the name matches a HAVIT Blazor component (e.g. HxButton, HxGrid, HxInputText, ThemeColor).";
		}

		ComponentApiDocModel model = _modelBuilder.BuildModel(type);
		string markdown = _renderer.Render(model);

		return markdown;
	}
}
