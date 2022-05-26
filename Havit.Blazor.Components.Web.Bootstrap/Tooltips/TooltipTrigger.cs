namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Triggers for <see cref="HxTooltip"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/TooltipTrigger">https://havit.blazor.eu/types/TooltipTrigger</see>
	/// </summary>
	[Flags]
	public enum TooltipTrigger
	{
		Click = 1,
		Hover = 2,
		Focus = 4,
		Manual = 8
	}
}
