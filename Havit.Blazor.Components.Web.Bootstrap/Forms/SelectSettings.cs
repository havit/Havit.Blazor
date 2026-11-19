namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxSelect{TValue, TItem}"/> component.
/// </summary>
public record SelectSettings : InputSettings
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

	/// <summary>
	/// Enables filtering capabilities.
	/// </summary>
	public bool AllowFiltering { get; set; }

	/// <summary>
	/// When enabled, the filter will be cleared when the menu is closed.
	/// </summary>
	public bool ClearFilterOnHide { get; set; }

	/// <summary>
	/// Icon displayed in the filter input for searching the filter.
	/// </summary>
	public IconBase FilterSearchIcon { get; set; } = BootstrapIcon.Search;

	/// <summary>
	/// Icon displayed in the filter input for clearing the filter.
	/// </summary>
	public IconBase FilterClearIcon { get; set; } = BootstrapIcon.XLg;
}
