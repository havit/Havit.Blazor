using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Services;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that provides component API documentation (including list of available demo samples) in Markdown format.
/// </summary>
internal class GetComponentDocsTool
{
	private static readonly Assembly s_documentationAssembly = typeof(DocumentationCatalogService).Assembly;

	private readonly IComponentApiDocModelBuilder _modelBuilder;
	private readonly McpDocMarkdownRenderer _renderer;

	public GetComponentDocsTool(IComponentApiDocModelBuilder modelBuilder, McpDocMarkdownRenderer renderer)
	{
		_modelBuilder = modelBuilder;
		_renderer = renderer;
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

		ComponentApiDocModel model = _modelBuilder.BuildModel(type);
		IReadOnlyList<string> sampleNames = GetSampleNames(componentName);
		string markdown = _renderer.RenderComponentDoc(model, sampleNames);

		activity?.SetTag("mcp.tool.result", "success");
		activity?.SetTag("mcp.tool.resolvedType", type.FullName);
		activity?.SetTag("mcp.tool.sampleCount", sampleNames.Count);

		return markdown;
	}

	private static IReadOnlyList<string> GetSampleNames(string componentName)
	{
		return s_documentationAssembly.GetManifestResourceNames()
			.Where(r => r.EndsWith(".razor", StringComparison.Ordinal)
				&& r.Contains("Demo", StringComparison.Ordinal)
				&& ExtractFileName(r).StartsWith(componentName + "_", StringComparison.OrdinalIgnoreCase))
			.OrderBy(r => r, StringComparer.Ordinal)
			.Select(r => ExtractFileName(r))
			.ToList();
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
