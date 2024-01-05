namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Triggers for <see cref="HxPopover"/>.
/// </summary>
[Flags]
public enum PopoverTrigger
{
	/// <summary>
	/// Specifies that the popover should be triggered by a click event.
	/// </summary>
	Click = TooltipTrigger.Click,

	/// <summary>
	/// Specifies that the popover should be triggered by a hover event.
	/// </summary>
	Hover = TooltipTrigger.Hover,

	/// <summary>
	/// Specifies that the popover should be triggered by a focus event.
	/// </summary>
	Focus = TooltipTrigger.Focus,

	/// <summary>
	/// Specifies that the popover should be triggered manually.
	/// </summary>
	Manual = TooltipTrigger.Manual
}
