namespace Havit.Blazor.Components.Web.Bootstrap;

public enum CheckboxListRenderMode
{
	/// <summary>
	/// Renders regular checkboxes.
	/// </summary>
	Checkboxes,

	/// <summary>
	/// Renders switches.
	/// </summary>
	Switches,

	/// <summary>
	/// Renders switches with native haptic feedback (mobile Safari, iOS 17.4+).
	/// </summary>
	NativeSwitches,

	/// <summary>
	/// Renders toggle buttons.<br />
	/// </summary>
	ToggleButtons,

	/// <summary>
	/// Renders toggle buttons in a button group.<br />
	/// </summary>
	ButtonGroup
}
