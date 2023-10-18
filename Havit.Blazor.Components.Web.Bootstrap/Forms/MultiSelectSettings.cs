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
}