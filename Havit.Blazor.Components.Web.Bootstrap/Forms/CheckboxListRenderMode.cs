namespace Havit.Blazor.Components.Web.Bootstrap;

public enum CheckboxListRenderMode
{
	/// <summary>
	/// Renders regular checkboxes.
	/// </summary>
	Checkbox,

	/// <summary>
	/// Renders switches.
	/// </summary>
	Switch,

	/// <summary>
	/// Renders switches with native haptic feedback (mobile Safari, iOS 17.4+).
	/// </summary>
	NativeSwitch,

	/// <summary>
	/// Renders toggle buttosn.<br />
	/// </summary>
	ToggleButton,

	/// <summary>
	/// Renders toggle buttons in a button group.<br />
	/// </summary>
	ButtonGroup
}
