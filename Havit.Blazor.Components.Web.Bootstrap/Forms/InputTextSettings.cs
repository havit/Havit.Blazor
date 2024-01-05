using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputText"/> and derived components.
/// </summary>
public record InputTextSettings : InputSettings, IInputSettingsWithSize
{
	/// <summary>
	/// Size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Hint to browsers regarding the type of virtual keyboard configuration to use when editing.
	/// </summary>
	public InputMode? InputMode { get; set; }
}
