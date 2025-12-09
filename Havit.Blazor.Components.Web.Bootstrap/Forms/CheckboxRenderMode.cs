namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Render mode for <see cref="HxCheckbox"/>.
/// </summary>
public enum CheckboxRenderMode
{
	/// <summary>
	/// Renders regular checkbox.
	/// </summary>
	Checkbox,

	/// <summary>
	/// Renders switch.
	/// </summary>
	Switch,

	/// <summary>
	/// Renders switch with native haptic feedback (mobile Safari, iOS 17.4+).
	/// </summary>
	NativeSwitch,

	/// <summary>
	/// Renders toggle button.
	/// </summary>
	ToggleButton,
}
