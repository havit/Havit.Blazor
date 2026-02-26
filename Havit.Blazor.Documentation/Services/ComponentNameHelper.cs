using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Resolves component names with custom prefixes (e.g. BtPager, MedTabPanel, PxButton)
/// to matching Hx-prefixed HAVIT Blazor components by matching the component name suffix.
/// </summary>
public static class ComponentNameHelper
{
	/// <summary>
	/// Attempts to find a matching Hx component for a component name with a non-standard prefix.
	/// Returns the matched Hx component name (e.g. "HxButton" for "PxButton"), or <c>null</c> if no match is found.
	/// </summary>
	public static string TryResolveHxComponentName(string componentName, IDocumentationCatalogService catalogService)
	{
		if (string.IsNullOrWhiteSpace(componentName))
		{
			return null;
		}

		// Strip generic type arguments (e.g. "BtGrid<TItem>" -> "BtGrid")
		int openingBracePosition = componentName.IndexOf('<');
		if (openingBracePosition > 0)
		{
			componentName = componentName[..openingBracePosition];
		}

		// If already an Hx component, no fallback needed
		if (componentName.StartsWith("Hx", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}

		IReadOnlyList<DocumentationCatalogItem> components = catalogService.GetComponents();

		// Try to find an Hx component whose suffix matches the end of the queried name.
		// E.g. "PxButton" ends with "Button" which matches "HxButton" (suffix after "Hx" is "Button").
		// We prefer the longest suffix match to avoid false positives.
		string bestMatch = null;
		int bestSuffixLength = 0;

		foreach (DocumentationCatalogItem item in components)
		{
			string hxName = item.Title;

			if (!hxName.StartsWith("Hx", StringComparison.Ordinal))
			{
				continue;
			}

			string hxSuffix = hxName[2..]; // Part after "Hx", e.g. "Button", "Grid", "Pager"

			if (hxSuffix.Length == 0)
			{
				continue;
			}

			if (componentName.EndsWith(hxSuffix, StringComparison.OrdinalIgnoreCase) && hxSuffix.Length > bestSuffixLength)
			{
				bestMatch = hxName;
				bestSuffixLength = hxSuffix.Length;
			}
		}

		return bestMatch;
	}
}
