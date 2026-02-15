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

	public GetComponentSamplesTool(IComponentDemosProvider componentDemosProvider)
	{
		_componentDemosProvider = componentDemosProvider;
	}

	/// <summary>
	/// Returns all demo samples for a given HAVIT Blazor component in Markdown format.
	/// </summary>
	[McpServerTool(Name = "get_component_samples", Title = "Get component samples", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns all demo/sample code snippets (Razor source) for a HAVIT Blazor component. Provide the component name, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")]
	public string GetComponentSamples(
		[Description("Name of the component to get samples for, e.g. 'HxButton', 'HxGrid', 'HxInputText'.")] string componentName)
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_samples");
		activity?.SetTag("mcp.tool.parameter.componentName", componentName);

		IReadOnlyList<string> resourceNames = _componentDemosProvider.GetComponentDemoResourceNames(componentName);

		if (resourceNames.Count == 0)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"No demo samples found for component '{componentName}'.";
		}

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.sampleCount", resourceNames.Count);

		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"# {componentName} — Demo Samples");
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
