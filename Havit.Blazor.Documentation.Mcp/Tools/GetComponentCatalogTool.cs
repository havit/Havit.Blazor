using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;
using ModelContextProtocol.Server;

namespace Havit.Blazor.Documentation.Mcp.Tools;

/// <summary>
/// MCP tool that returns a catalog of all HAVIT Blazor components with their summaries.
/// </summary>
internal class GetComponentCatalogTool
{
	private readonly IDocumentationCatalogService _catalogService;
	private readonly IApiDocModelBuilder _modelBuilder;

	public GetComponentCatalogTool(IDocumentationCatalogService catalogService, IApiDocModelBuilder modelBuilder)
	{
		_catalogService = catalogService;
		_modelBuilder = modelBuilder;
	}

	/// <summary>
	/// Returns the full catalog of HAVIT Blazor components with name and summary.
	/// </summary>
	[McpServerTool(Name = "get_component_catalog", Title = "Get catalog of components", Destructive = false, Idempotent = true, ReadOnly = true)]
	[Description("Returns a list of all HAVIT Blazor components (Hx*) with their name and summary description. Use this to discover available components.")]
	public string GetComponentCatalog()
	{
		using Activity activity = McpToolActivitySource.Source.StartActivity("get_component_catalog");

		IReadOnlyList<DocumentationCatalogItem> components = _catalogService.GetComponents();

		activity?.SetTag("mcp.tool.componentCount", components.Count);

		StringBuilder sb = new StringBuilder();
		sb.AppendLine("# HAVIT Blazor Component Catalog");
		sb.AppendLine();
		sb.AppendLine($"Total components: {components.Count}");
		sb.AppendLine();
		sb.AppendLine("| Component | Summary |");
		sb.AppendLine("|-----------|---------|");

		foreach (DocumentationCatalogItem item in components)
		{
			string summary = GetComponentSummary(item.Title);
			sb.AppendLine($"| {item.Title} | {summary} |");
		}

		return sb.ToString();
	}

	private string GetComponentSummary(string componentTitle)
	{
		// Strip non-type suffixes like "[Core]" from titles such as "HxInputFile[Core]"
		string typeName = Regex.Replace(componentTitle, @"\[.*?\]", string.Empty);

		Type type = ApiTypeHelper.GetType(typeName, includeTypesContainingTypeName: true);
		if (type is null)
		{
			return string.Empty;
		}

		ApiDocModel model = _modelBuilder.BuildModel(type);
		string summary = model.Class?.Comments?.Summary;

		return StripHtml(summary);
	}

	private static string StripHtml(string html)
	{
		if (string.IsNullOrEmpty(html))
		{
			return string.Empty;
		}

		string result = Regex.Replace(html, @"<code>(.*?)</code>", "`$1`");
		result = Regex.Replace(result, @"<br\s*/?>", " ");
		result = Regex.Replace(result, @"<[^>]+>", string.Empty);
		result = System.Net.WebUtility.HtmlDecode(result);
		result = Regex.Replace(result, @"\s+", " ").Trim();
		result = result.Replace("|", "\\|");

		return result;
	}
}
