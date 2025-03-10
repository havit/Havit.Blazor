using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxAutosuggest{TItem, TValue} "/> and derived components.
/// </summary>
public record AutosuggestSettings : InputSettings
{
	/// <summary>
	/// Icon displayed in the input when no item is selected.
	/// </summary>
	public IconBase SearchIcon { get; set; }

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
	/// Defines whether the input may be checked for spelling errors.
	/// </summary>
	public bool? Spellcheck { get; set; }
}
