using System.ComponentModel;
using System.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that provides component API documentation (including list of available demo samples) in Markdown format.
/// </summary>
internal class GetComponentDocsTool
{
	private readonly IApiDocModelProvider _modelProvider;
	private readonly IDocMarkdownRenderer _renderer;
	private readonly IComponentDemosProvider _componentDemosProvider;

	public GetComponentDocsTool(IApiDocModelProvider modelProvider, IDocMarkdownRenderer renderer, IComponentDemosProvider componentDemosProvider)
	{
		_modelProvider = modelProvider;
		_renderer = renderer;
		_componentDemosProvider = componentDemosProvider;
	}

	/// <summary>
	/// Returns the API documentation for a HAVIT Blazor component in Markdown format, including demo samples.
	/// </summary>
	[McpServerTool(Name = "get_component_docs", Title = "Get component documentation", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns the API documentation (parameters, properties, events, methods) for a HAVIT Blazor component or type. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")]
	public string GetComponentDocs(
		[Description("Name of the component or type to get documentation for, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")] string componentName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_docs");
		activity?.SetTag("mcp.tool.parameter.componentName", componentName);

		Type type = ApiTypeHelper.GetType(componentName, includeTypesContainingTypeName: true);
		if (type is null)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"Component '{componentName}' not found. Make sure the name matches a HAVIT Blazor component (e.g. HxButton, HxGrid, HxInputText). For supporting types (enums, settings), use the get_type_doc tool.";
		}

		ApiDocModel model = _modelProvider.GetApiDocModel(type);
		IReadOnlyList<string> sampleNames = _componentDemosProvider.GetComponentDemoFileNames(componentName);
		string markdown = _renderer.RenderComponentDoc(model, sampleNames);

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.resolvedType", type.FullName);
		activity?.SetTag("mcp.tool.sampleCount", sampleNames.Count);

		return markdown;
	}
}
