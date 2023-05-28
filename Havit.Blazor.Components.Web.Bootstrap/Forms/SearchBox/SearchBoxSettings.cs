namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxSearchBox{TItem} "/> and derived components.
/// </summary>
public class SearchBoxSettings
{
	/// <summary>
	/// Icon displayed in input when no item is selected.
	/// </summary>
	public IconBase SearchIcon { get; set; }

	/// <summary>
	/// Icon displayed in input on selection clear button when item is selected.
	/// </summary>
	public IconBase ClearIcon { get; set; }

	/// <summary>
	/// Minimal number of characters to start suggesting.
	/// </summary>
	public int? MinimumLength { get; set; }

	/// <summary>
	/// Debounce delay in milliseconds.
	/// </summary>
	public int? Delay { get; set; }

	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Additional CSS classes for the wrapping <c>div</c>.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS classes for the items in the dropdown menu.
	/// </summary>
	public string ItemCssClass { get; set; }

	/// <summary>
	/// Additional CSS classes for the search box input.
	/// </summary>
	public string InputCssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with input-group span.
	/// </summary>
	public string InputGroupCssClass { get; set; }
}
