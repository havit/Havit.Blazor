namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Placement of the popover for <see cref="HxPopover"/>.
/// </summary>
public enum PopoverPlacement
{
	Top = TooltipPlacement.Top,
	Bottom = TooltipPlacement.Bottom,
	Left = TooltipPlacement.Left,
	Right = TooltipPlacement.Right,

	/// <summary>
	/// When specified, it will dynamically reorient the popover towards the side with the most available space.
	/// (Emulated by Havit.Blazor, Bootstrap 6 has no <c>auto</c> placement.)
	/// </summary>
	Auto = TooltipPlacement.Auto
}
