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
	/// [Obsolete] Renders the same as <see cref="Switch"/>.
	/// Since Bootstrap 6, every switch renders the <c>switch</c> attribute (native haptic feedback in mobile Safari, iOS 17.4+).
	/// </summary>
	[Obsolete($"Use {nameof(Switch)} instead. Since Bootstrap 6, every switch renders the switch attribute (native haptic feedback).")]
	NativeSwitch,

	/// <summary>
	/// Renders toggle button.
	/// </summary>
	ToggleButton,
}
