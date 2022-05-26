namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Placement of the popover for <see cref="HxPopover"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/PopoverPlacement">https://havit.blazor.eu/types/PopoverPlacement</see>
	/// </summary>
	public enum PopoverPlacement
	{
		Top = TooltipPlacement.Top,
		Bottom = TooltipPlacement.Bottom,
		Left = TooltipPlacement.Left,
		Right = TooltipPlacement.Right,

		/// <summary>
		/// When is specified, it will dynamically reorient the popover.
		/// </summary>
		Auto = TooltipPlacement.Auto
	}
}
