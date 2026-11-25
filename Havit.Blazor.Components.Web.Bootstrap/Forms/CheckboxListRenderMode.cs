namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Render mode for <see cref="HxCheckboxList{TValue, TItem}"/>
/// </summary>
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
	/// [Obsolete] Renders the same as <see cref="Switches"/>.
	/// Since Bootstrap 6, every switch renders the <c>switch</c> attribute (native haptic feedback in mobile Safari, iOS 17.4+).
	/// </summary>
	[Obsolete($"Use {nameof(Switches)} instead. Since Bootstrap 6, every switch renders the switch attribute (native haptic feedback).")]
	NativeSwitches,

	/// <summary>
	/// Renders toggle buttons.
	/// </summary>
	ToggleButtons,

	/// <summary>
	/// Renders toggle buttons in a button group.
	/// </summary>
	ButtonGroup
}
