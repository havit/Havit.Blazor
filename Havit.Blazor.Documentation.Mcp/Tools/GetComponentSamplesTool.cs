using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that returns component demo samples from embedded resources.
/// </summary>
internal class GetComponentSamplesTool
{
	private readonly IComponentDemosProvider _componentDemosProvider;
	private readonly IDocumentationCatalogService _catalogService;

	public GetComponentSamplesTool(IComponentDemosProvider componentDemosProvider, IDocumentationCatalogService catalogService)
	{
		_componentDemosProvider = componentDemosProvider;
		_catalogService = catalogService;
	}

	/// <summary>
	/// Returns all demo samples for a given HAVIT Blazor component in Markdown format.
	/// </summary>
	[McpServerTool(Name = "get_component_samples", Title = "Get component samples", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns all demo/sample code snippets (Razor source) for a HAVIT Blazor component. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'. Also resolves derived components with custom prefixes (e.g. 'BtPager', 'PxButton') to their base Hx component.")]
	public string GetComponentSamples(
		[Description("Name of the component to get samples for, e.g. 'HxButton', 'HxGrid', 'HxInputText'. Derived components with custom prefixes (e.g. 'BtPager', 'PxButton') are resolved to their base Hx component.")] string componentName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_samples");
		activity?.SetTag("mcp.tool.parameter.componentName", componentName);

		IReadOnlyList<string> resourceNames = _componentDemosProvider.GetComponentDemoResourceNames(componentName);

		// If no samples found directly, try to resolve as a derived component with a custom prefix (e.g. BtPager -> HxPager)
		string resolvedHxComponentName = null;
		if (resourceNames.Count == 0)
		{
			resolvedHxComponentName = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);
			if (resolvedHxComponentName is not null)
			{
				resourceNames = _componentDemosProvider.GetComponentDemoResourceNames(resolvedHxComponentName);
				activity?.SetTag("mcp.tool.resolvedFromDerived", true);
				activity?.SetTag("mcp.tool.resolvedHxComponentName", resolvedHxComponentName);
			}
		}

		if (resourceNames.Count == 0)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"No demo samples found for component '{componentName}'.";
		}

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.sampleCount", resourceNames.Count);

		string displayName = resolvedHxComponentName ?? componentName;

		StringBuilder sb = new StringBuilder();

		if (resolvedHxComponentName is not null)
		{
			sb.AppendLine($"> **Note:** `{componentName}` is likely a derived component based on `{resolvedHxComponentName}`. The samples below are for `{resolvedHxComponentName}` — the derived component will have the same or similar API and usage.");
			sb.AppendLine();
		}

		sb.AppendLine($"# {displayName} — Demo Samples");
		sb.AppendLine();

		foreach (string resourceName in resourceNames)
		{
			string fileName = ComponentDemosProvider.ExtractFileName(resourceName);
			string content = _componentDemosProvider.GetDemoContentByResourceName(resourceName);

			sb.AppendLine($"## {fileName}");
			sb.AppendLine();
			sb.AppendLine("```razor");
			sb.AppendLine(content);
			sb.AppendLine("```");
			sb.AppendLine();
		}

		return sb.ToString();
	}
}
