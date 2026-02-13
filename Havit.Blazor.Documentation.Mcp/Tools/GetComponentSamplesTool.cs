using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
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
	private static readonly Assembly s_documentationAssembly = typeof(DocumentationCatalogService).Assembly;

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

		string[] resourceNames = s_documentationAssembly.GetManifestResourceNames()
			.Where(r => r.EndsWith(".razor", StringComparison.Ordinal)
				&& r.Contains("Demo", StringComparison.Ordinal)
				&& ExtractFileName(r).StartsWith(componentName + "_", StringComparison.OrdinalIgnoreCase))
			.OrderBy(r => r, StringComparer.Ordinal)
			.ToArray();

		if (resourceNames.Length == 0)
		{
			activity?.SetTag("mcp.tool.result", "not_found");
			return $"No demo samples found for component '{componentName}'.";
		}

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.sampleCount", resourceNames.Length);

		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"# {componentName} — Demo Samples");
		sb.AppendLine();

		foreach (string resourceName in resourceNames)
		{
			string fileName = ExtractFileName(resourceName);

			using Stream stream = s_documentationAssembly.GetManifestResourceStream(resourceName);
			using StreamReader reader = new StreamReader(stream);
			string content = reader.ReadToEnd();

			sb.AppendLine($"## {fileName}");
			sb.AppendLine();
			sb.AppendLine("```razor");
			sb.AppendLine(content);
			sb.AppendLine("```");
			sb.AppendLine();
		}

		return sb.ToString();
	}

	/// <summary>
	/// Extracts the file name from an embedded resource name.
	/// E.g. "Havit.Blazor.Documentation.Pages.Components.HxAccordionDoc.HxAccordion_PlainDemo.razor" → "HxAccordion_PlainDemo.razor".
	/// </summary>
	private static string ExtractFileName(string resourceName)
	{
		int extensionIndex = resourceName.LastIndexOf(".razor", StringComparison.Ordinal);
		if (extensionIndex < 0)
		{
			return resourceName;
		}

		string withoutExtension = resourceName[..extensionIndex];
		int lastDot = withoutExtension.LastIndexOf('.');
		if (lastDot < 0)
		{
			return resourceName;
		}

		return withoutExtension[(lastDot + 1)..] + ".razor";
	}
}
