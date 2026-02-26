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
	private readonly IDocumentationCatalogService _catalogService;

	public GetComponentDocsTool(IApiDocModelProvider modelProvider, IDocMarkdownRenderer renderer, IComponentDemosProvider componentDemosProvider, IDocumentationCatalogService catalogService)
	{
		_modelProvider = modelProvider;
		_renderer = renderer;
		_componentDemosProvider = componentDemosProvider;
		_catalogService = catalogService;
	}

	/// <summary>
	/// Returns the API documentation for a HAVIT Blazor component in Markdown format, including demo samples.
	/// </summary>
	[McpServerTool(Name = "get_component_docs", Title = "Get component documentation", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns the API documentation (parameters, properties, events, methods) for a HAVIT Blazor component or type. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'. Also resolves derived components with custom prefixes (e.g. 'BtPager', 'PxButton') to their base Hx component.")]
	public string GetComponentDocs(
		[Description("Name of the component or type to get documentation for, e.g. 'HxButton', 'HxGrid', 'HxInputText'. Derived components with custom prefixes (e.g. 'BtPager', 'PxButton') are resolved to their base Hx component.")] string componentName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_docs");
		activity?.SetTag("mcp.tool.parameter.componentName", componentName);

		Type type = ApiTypeHelper.GetType(componentName, includeTypesContainingTypeName: true);

		// If not found directly, try to resolve as a derived component with a custom prefix (e.g. BtPager -> HxPager)
		string resolvedHxComponentName = null;
		if (type is null)
		{
			resolvedHxComponentName = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);
			if (resolvedHxComponentName is not null)
			{
				type = ApiTypeHelper.GetType(resolvedHxComponentName, includeTypesContainingTypeName: true);
				activity?.SetTag("mcp.tool.resolvedFromDerived", true);
				activity?.SetTag("mcp.tool.resolvedHxComponentName", resolvedHxComponentName);
			}
		}

		if (type is null)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"Component '{componentName}' not found. Make sure the name matches a HAVIT Blazor component (e.g. HxButton, HxGrid, HxInputText). For supporting types (enums, settings), use the get_type_doc tool.";
		}

		string lookupName = resolvedHxComponentName ?? componentName;
		ApiDocModel model = _modelProvider.GetApiDocModel(type);
		IReadOnlyList<string> sampleNames = _componentDemosProvider.GetComponentDemoFileNames(lookupName);
		string markdown = _renderer.RenderComponentDoc(model, sampleNames);

		if (resolvedHxComponentName is not null)
		{
			markdown = $"> **Note:** `{componentName}` is likely a derived component based on `{resolvedHxComponentName}`. The documentation below is for `{resolvedHxComponentName}` — the derived component will have the same or similar API and usage.\n\n" + markdown;
		}

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.resolvedType", type.FullName);
		activity?.SetTag("mcp.tool.sampleCount", sampleNames.Count);

		return markdown;
	}
}
