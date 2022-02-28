using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

internal static class DropdownToggleExtensions
{
	internal static string GetDropdownDataBsReference(this IHxDropdownToggle toggle)
	{
		if (String.IsNullOrWhiteSpace(toggle.DropdownReference))
		{
			return null;
		}
		if (IsKnownDropdownReference(toggle.DropdownReference))
		{
			return toggle.DropdownReference;
		}
		return null;
	}

	internal static string GetDropdownJsOptionsReference(this IHxDropdownToggle toggle)
	{
		if (String.IsNullOrWhiteSpace(toggle.DropdownReference))
		{
			return null;
		}
		if (!IsKnownDropdownReference(toggle.DropdownReference))
		{
			return toggle.DropdownReference;
		}
		return null;
	}

	private static bool IsKnownDropdownReference(string dropdownReference)
	{
		return (dropdownReference is not null)
					&& ((dropdownReference.Equals("toggle", StringComparison.OrdinalIgnoreCase)
						|| dropdownReference.Equals("parent", StringComparison.OrdinalIgnoreCase)
						)
				);
	}
}
