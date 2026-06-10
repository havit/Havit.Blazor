namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/">Menu header</see> for <see cref="HxMenu"/>. Renders an <c>h6.menu-header</c> element.
/// </summary>
public partial class HxMenuHeader
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
}
