namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Placement of the tooltip for <see cref="HxTooltip"/>.
/// </summary>
public enum TooltipPlacement
{
	Top = 0,
	Bottom = 1,
	Left = 2,
	Right = 3,

	/// <summary>
	/// When specified, it will dynamically reorient the tooltip towards the side with the most available space.
	/// (Emulated by Havit.Blazor, Bootstrap 6 has no <c>auto</c> placement.)
	/// </summary>
	Auto = 4
}
