using System.Reflection;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides access to demo resources embedded in the documentation assembly.
/// </summary>
public class ComponentDemosProvider : IComponentDemosProvider
{
	private static readonly Assembly s_documentationAssembly = typeof(ComponentDemosProvider).Assembly;

	/// <summary>
	/// Returns the file names of all demos for the specified component.
	/// </summary>
	public IReadOnlyList<string> GetComponentDemoFileNames(string componentName)
	{
		// Normalize component name by stripping generic type arguments (e.g., "HxGrid<TItem>" -> "HxGrid")
		int openingBracePosition = componentName.IndexOf('<');
		if (openingBracePosition > 0)
		{
			componentName = componentName[..openingBracePosition];
		}

		return s_documentationAssembly.GetManifestResourceNames()
			.Where(r => r.EndsWith(".razor", StringComparison.Ordinal)
				&& r.Contains("Demo", StringComparison.Ordinal)
				&& ExtractFileName(r).StartsWith(componentName + "_", StringComparison.OrdinalIgnoreCase))
			.OrderBy(r => r, StringComparer.Ordinal)
			.Select(r => ExtractFileName(r))
			.ToList();
	}

	/// <summary>
	/// Returns the resource names of all demo samples for the specified component.
	/// </summary>
	public IReadOnlyList<string> GetComponentDemoResourceNames(string componentName)
	{
		return s_documentationAssembly.GetManifestResourceNames()
			.Where(r => r.EndsWith(".razor", StringComparison.Ordinal)
				&& r.Contains("Demo", StringComparison.Ordinal)
				&& ExtractFileName(r).StartsWith(componentName + "_", StringComparison.OrdinalIgnoreCase))
			.OrderBy(r => r, StringComparer.Ordinal)
			.ToArray();
	}

	/// <summary>
	/// Reads the content of a demo by its file name.
	/// Returns <c>null</c> if the sample is not found.
	/// </summary>
	public string GetDemoContentByFileName(string demoFileName)
	{
		string resourceName = s_documentationAssembly.GetManifestResourceNames()
			.Where(r => r.EndsWith(".razor", StringComparison.Ordinal)
				&& r.Contains("Demo", StringComparison.Ordinal)
				&& ExtractFileName(r) == demoFileName)
			.FirstOrDefault();

		if (resourceName is null)
		{
			return null;
		}

		using Stream stream = s_documentationAssembly.GetManifestResourceStream(resourceName);
		using StreamReader reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}

	/// <summary>
	/// Reads the content of a demo sample by its full embedded resource name.
	/// </summary>
	public string GetDemoContentByResourceName(string resourceName)
	{
		using Stream stream = s_documentationAssembly.GetManifestResourceStream(resourceName);
		using StreamReader reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}

	/// <summary>
	/// Extracts the file name from an embedded resource name.
	/// E.g. "Havit.Blazor.Documentation.Pages.Components.HxAccordionDoc.HxAccordion_PlainDemo.razor" → "HxAccordion_PlainDemo.razor".
	/// </summary>
	public static string ExtractFileName(string resourceName)
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
