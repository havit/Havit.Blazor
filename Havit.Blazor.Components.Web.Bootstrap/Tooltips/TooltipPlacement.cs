namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Placement of the tooltip for <see cref="HxTooltip"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/TooltipPlacement">https://havit.blazor.eu/types/TooltipPlacement</see>
	/// </summary>
	public enum TooltipPlacement
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,

		/// <summary>
		/// When is specified, it will dynamically reorient the tooltip.
		/// </summary>
		Auto = 4
	}
}
