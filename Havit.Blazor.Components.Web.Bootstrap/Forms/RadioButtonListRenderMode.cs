namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Render mode for <see cref="HxRadioButtonList{TValue, TItem}"/>
/// </summary>
public enum RadioButtonListRenderMode
{
	/// <summary>
	/// Renders regular radio buttons.
	/// </summary>
	RadioButtons,

	/// <summary>
	/// Renders toggle buttons.
	/// </summary>
	ToggleButtons,

	/// <summary>
	/// Renders toggle buttons in a button group.
	/// </summary>
	ButtonGroup
}
