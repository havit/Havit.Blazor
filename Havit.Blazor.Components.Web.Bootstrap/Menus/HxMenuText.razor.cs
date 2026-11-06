namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text item for the <see cref="HxMenu"/>. Renders a <c>span.menu-text</c> element.
/// </summary>
public partial class HxMenuText
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }
}
