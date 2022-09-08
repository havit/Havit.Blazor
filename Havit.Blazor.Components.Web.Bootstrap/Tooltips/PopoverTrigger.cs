namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Triggers for <see cref="HxPopover"/>.
/// </summary>
[Flags]
public enum PopoverTrigger
{
	Click = TooltipTrigger.Click,
	Hover = TooltipTrigger.Hover,
	Focus = TooltipTrigger.Focus,
	Manual = TooltipTrigger.Manual
}
