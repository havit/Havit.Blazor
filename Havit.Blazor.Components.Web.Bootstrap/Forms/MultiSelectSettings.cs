using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxMultiSelect{TValue, TItem}"/> component.
/// </summary>
public record MultiSelectSettings : InputSettings, IInputSettingsWithSize
{
	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Enables filtering capabilities.
	/// </summary>
	public bool AllowFiltering { get; set; }

	/// <summary>
	/// Enables select all capabilities.
	/// </summary>
	public bool AllowSelectAll { get; set; }

	/// <summary>
	/// When enabled the filter will be cleared when the dropdown is closed.
	/// </summary>
	public bool ClearFilterOnHide { get; set; }

	/// <summary>
	/// Icon displayed in filter input for searching the filter.
	/// </summary>
	public IconBase FilterSearchIcon { get; set; } = BootstrapIcon.Search;

	/// <summary>
	/// Icon displayed in filter input for clearing the filter.
	/// </summary>
	public IconBase FilterClearIcon { get; set; } = BootstrapIcon.XLg;
}