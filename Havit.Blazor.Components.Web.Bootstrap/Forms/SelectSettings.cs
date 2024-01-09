using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxSelect{TValue, TItem}"/> component.
/// </summary>
public record SelectSettings : InputSettings, IInputSettingsWithSize
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }
}
