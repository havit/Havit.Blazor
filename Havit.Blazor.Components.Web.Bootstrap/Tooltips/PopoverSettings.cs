using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

public record class PopoverSettings : ITooltipInternalSettings
{
	/// <summary>
	/// Apply a CSS fade transition to the popover (enable/disable).
	/// </summary>
	public bool? Animation { get; set; }

	/// <summary>
	/// Custom CSS class to add.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Appends the popover to a specific element. Default is <c>body</c>.
	/// </summary>
	public string Container { get; set; }

	/// <summary>
	/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
	/// </summary>
	public string WrapperCssClass { get; set; }

	/// <summary>
	/// Offset of the component relative to its target (ChildContent).
	/// </summary>
	public (int X, int Y)? Offset { get; set; }

	/// <summary>
	/// Popover trigger(s).
	/// </summary>
	public PopoverTrigger? Trigger { get; set; }
	TooltipTrigger? ITooltipInternalSettings.Trigger => (TooltipTrigger?)Trigger;

	/// <summary>
	/// Popover placement.
	/// </summary>
	public PopoverPlacement? Placement { get; set; }
	TooltipPlacement? ITooltipInternalSettings.Placement => (TooltipPlacement?)Placement;
}
