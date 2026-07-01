namespace Havit.Blazor.Components.Web.Bootstrap;

internal static class MenuToggleExtensions
{
	internal static string GetMenuDataBsReference(this IHxMenuToggle toggle)
	{
		if (String.IsNullOrWhiteSpace(toggle.MenuReference))
		{
			return null;
		}
		if (IsKnownMenuReference(toggle.MenuReference))
		{
			return toggle.MenuReference;
		}
		return null;
	}

	internal static string GetMenuJsOptionsReference(this IHxMenuToggle toggle)
	{
		if (String.IsNullOrWhiteSpace(toggle.MenuReference))
		{
			return null;
		}
		if (!IsKnownMenuReference(toggle.MenuReference))
		{
			return toggle.MenuReference;
		}
		return null;
	}

	private static bool IsKnownMenuReference(string menuReference)
	{
		return (menuReference is not null)
					&& ((menuReference.Equals("toggle", StringComparison.OrdinalIgnoreCase)
						|| menuReference.Equals("parent", StringComparison.OrdinalIgnoreCase)
						)
				);
	}
}
