namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxSearchBox{TItem}"/> and derived components.
/// </summary>
public class SearchBoxSettings
{
	/// <summary>
	/// Icon displayed in the input when no item is selected.
	/// </summary>
	public IconBase SearchIcon { get; set; }

	/// <summary>
	/// Placement of the search icon.<br/>
	/// </summary>
	public SearchBoxSearchIconPlacement? SearchIconPlacement { get; set; }

	/// <summary>
	/// Icon displayed in the input on the selection clear button when an item is selected.
	/// </summary>
	public IconBase ClearIcon { get; set; }

	/// <summary>
	/// The minimum number of characters to start suggesting.
	/// </summary>
	public int? MinimumLength { get; set; }

	/// <summary>
	/// The debounce delay in milliseconds.
	/// </summary>
	public int? Delay { get; set; }

	/// <summary>
	/// The input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

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
	/// Custom CSS class to render with the input-group span.
	/// </summary>
	public string InputGroupCssClass { get; set; }

	/// <summary>
	/// The behavior when the item is selected.
	/// </summary>
	public SearchBoxItemSelectionBehavior? ItemSelectionBehavior { get; set; }

	/// <summary>
	/// Defines whether the input may be checked for spelling errors.
	/// </summary>
	public bool? Spellcheck { get; set; }
}
