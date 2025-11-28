namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public interface ITooltipInternalSettings
{
	/// <summary>
	/// Apply a CSS fade transition to the tooltip (enable/disable).
	/// </summary>
	bool? Animation { get; set; }

	/// <summary>
	/// Custom CSS class to add.
	/// </summary>
	string CssClass { get; set; }

	/// <summary>
	/// Appends the tooltip/popover to a specific element. Default is <c>body</c>.
	/// </summary>
	string Container { get; set; }

	/// <summary>
	/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
	/// </summary>
	string WrapperCssClass { get; set; }

	/// <summary>
	/// Offset of the component relative to its target (ChildContent).
	/// </summary>
	(int X, int Y)? Offset { get; set; }

	/// <summary>
	/// Tooltip/Popover trigger(s).
	/// </summary>
	TooltipTrigger? Trigger { get; }

	/// <summary>
	/// Tooltip/Popover placement.
	/// </summary>
	TooltipPlacement? Placement { get; }
}