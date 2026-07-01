namespace Havit.Blazor.Components.Web.Bootstrap;

public static class DropdownMenuAlignmentExtensions
{
	/// <summary>
	/// Gets the CSS class for the specified <see cref="DropdownMenuAlignment"/>.
	/// </summary>
	public static string GetCssClass(this DropdownMenuAlignment dropdownMenuAlignment)
	{
		return dropdownMenuAlignment switch
		{
			DropdownMenuAlignment.Start => "dropdown-menu-start",
			DropdownMenuAlignment.End => "dropdown-menu-end",
			_ => throw new InvalidOperationException($"Unknown {nameof(DropdownMenuAlignment)} value {dropdownMenuAlignment}.")
		};
	}
}
