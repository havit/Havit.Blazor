namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Triggers for <see cref="HxPopover"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/PopoverTrigger">https://havit.blazor.eu/types/PopoverTrigger</see>
	/// </summary>
	[Flags]
	public enum PopoverTrigger
	{
		Click = TooltipTrigger.Click,
		Hover = TooltipTrigger.Hover,
		Focus = TooltipTrigger.Focus,
		Manual = TooltipTrigger.Manual
	}
}
