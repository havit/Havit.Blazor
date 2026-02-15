using System.Text;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.RepoDumpGenerator.Services;

/// <summary>
/// Generates markdown documentation files for all components and types from the documentation catalog.
/// </summary>
internal class DocDumpService
{
	private readonly IDocumentationCatalogService _catalogService;
	private readonly IApiDocModelProvider _modelProvider;
	private readonly IDocMarkdownRenderer _renderer;
	private readonly IComponentDemosProvider _componentDemosProvider;

	public DocDumpService(
		IDocumentationCatalogService catalogService,
		IApiDocModelProvider modelProvider,
		IDocMarkdownRenderer renderer,
		IComponentDemosProvider componentDemosProvider)
	{
		_catalogService = catalogService;
		_modelProvider = modelProvider;
		_renderer = renderer;
		_componentDemosProvider = componentDemosProvider;
	}

	/// <summary>
	/// Runs the documentation dump to the specified output directory.
	/// </summary>
	public void Run(string outputRoot)
	{
		// Clean output directory
		if (Directory.Exists(outputRoot))
		{
			Directory.Delete(outputRoot, recursive: true);
		}

		var allItems = _catalogService.GetAll();

		var componentCount = 0;
		var demoCount = 0;
		var typeCount = 0;

		componentCount = DumpComponents(allItems, outputRoot, ref demoCount);
		typeCount = DumpTypes(allItems, outputRoot);

		Console.WriteLine();
		Console.WriteLine($"Done. Generated {componentCount} component docs, {demoCount} demo files, {typeCount} type docs.");
		Console.WriteLine($"Output: {outputRoot}");
	}

	private int DumpComponents(IReadOnlyList<DocumentationCatalogItem> allItems, string outputRoot, ref int demoCount)
	{
		var componentCount = 0;

		foreach (var item in allItems)
		{
			if (!item.Href.StartsWith("/components/Hx", StringComparison.Ordinal))
			{
				continue;
			}

			// Extract component name from href
			// For anchored hrefs like /components/HxNavLink#HxNavLink, extract the anchor part
			// Skip sub-pages like /components/HxGrid#InfiniteScroll where anchor != component
			string componentName;
			if (item.Href.Contains('#'))
			{
				int anchorIndex = item.Href.IndexOf('#');
				var anchorPart = item.Href.Substring(anchorIndex + 1);

				if (string.IsNullOrEmpty(anchorPart))
				{
					continue; // Skip malformed hrefs with empty anchors
				}

				// Only include if the anchor is a component name (starts with Hx)
				if (!anchorPart.StartsWith("Hx", StringComparison.Ordinal))
				{
					continue; // Skip sub-pages like HxGrid#InfiniteScroll
				}

				componentName = anchorPart;
			}
			else
			{
				// Extract component name from path (e.g., "/components/HxButton" -> "HxButton")
				// LastIndexOf('/') is guaranteed to find at least one '/' due to the filter above
				int lastSlashIndex = item.Href.LastIndexOf('/');
				componentName = item.Href.Substring(lastSlashIndex + 1);
			}
			var type = ApiTypeHelper.GetType(componentName, includeTypesContainingTypeName: true);
			if (type is null)
			{
				Console.WriteLine($"  SKIP component (type not found): {componentName}");
				continue;
			}

			// Component API doc
			var model = _modelProvider.GetApiDocModel(type);
			var sampleNames = _componentDemosProvider.GetComponentDemoFileNames(componentName);
			var markdown = _renderer.RenderComponentDoc(model, sampleNames, includeMcpToolHint: false);

			var componentDir = Path.Combine(outputRoot, "./");
			Directory.CreateDirectory(componentDir);
			var filePath = Path.Combine(componentDir, $"{componentName}.md");
			File.WriteAllText(filePath, markdown, Encoding.UTF8);
			componentCount++;
			Console.WriteLine($"  Component: {componentName}");

			// Demos
			if (sampleNames.Count > 0)
			{
				var demosDir = Path.Combine(outputRoot, "demos", componentName);
				Directory.CreateDirectory(demosDir);

				foreach (var sampleName in sampleNames)
				{
					var content = _componentDemosProvider.GetDemoContentByFileName(sampleName);
					if (content is not null)
					{
						var mdFileName = Path.ChangeExtension(sampleName, ".md");
						var demoFilePath = Path.Combine(demosDir, mdFileName);
						var demoMarkdown = $"# {sampleName}\n\n```razor\n{content}\n```\n";
						File.WriteAllText(demoFilePath, demoMarkdown, Encoding.UTF8);
						demoCount++;
					}
				}
			}
		}

		return componentCount;
	}

	private int DumpTypes(IReadOnlyList<DocumentationCatalogItem> allItems, string outputRoot)
	{
		var typeCount = 0;

		foreach (var item in allItems)
		{
			if (!item.Href.StartsWith("/types/", StringComparison.Ordinal))
			{
				continue;
			}

			var typeName = item.Href.Split('/').Last();
			// Some catalog entries have spaces in the href (e.g. "/types/MultiSelect Settings") — normalize
			typeName = typeName.Replace(" ", string.Empty);

			var type = ApiTypeHelper.GetType(typeName, includeTypesContainingTypeName: true);
			if (type is null)
			{
				Console.WriteLine($"  SKIP type (not found): {typeName}");
				continue;
			}

			var model = _modelProvider.GetApiDocModel(type);
			var markdown = _renderer.RenderTypeDoc(model);

			var typesDir = Path.Combine(outputRoot, "types");
			Directory.CreateDirectory(typesDir);
			var filePath = Path.Combine(typesDir, $"{typeName}.md");
			File.WriteAllText(filePath, markdown, Encoding.UTF8);
			typeCount++;
			Console.WriteLine($"  Type: {typeName}");
		}

		return typeCount;
	}
}
