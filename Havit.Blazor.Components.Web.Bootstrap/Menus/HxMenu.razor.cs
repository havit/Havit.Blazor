namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/">Bootstrap 6 Menu</see> component (replaces the Bootstrap 5 Dropdown).<br />
/// Renders the <see cref="Toggle"/> and the <c>.menu</c> element (with <see cref="Content"/>) as direct siblings — Bootstrap 6 requires no wrapper element
/// (sibling position is also used by the CSS, e.g. for split-button border radius via <c>:has(+ .menu)</c>).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMenu">https://havit.blazor.eu/components/HxMenu</see>
/// </summary>
public partial class HxMenu : ComponentBase, IMenuContainer
{
	/// <summary>
	/// The toggle which opens the menu (typically <see cref="HxMenuToggleButton"/> or <see cref="HxMenuToggleElement"/>).
	/// Rendered as a direct sibling of the <c>.menu</c> element.
	/// </summary>
	[Parameter] public RenderFragment Toggle { get; set; }

	/// <summary>
	/// The menu content (typically <see cref="HxMenuItem"/>, <see cref="HxMenuItemNavLink"/>, <see cref="HxMenuDivider"/>, <see cref="HxMenuHeader"/>, <see cref="HxMenuText"/>, or any custom content).
	/// </summary>
	[Parameter] public RenderFragment Content { get; set; }

	/// <summary>
	/// Placement of the menu relative to the toggle. The default is <see cref="MenuPlacement.BottomStart"/> (Bootstrap default).
	/// </summary>
	[Parameter] public MenuPlacement Placement { get; set; } = MenuPlacement.BottomStart;

	/// <summary>
	/// Raw <c>data-bs-placement</c> value allowing responsive placements (e.g. <c>bottom-start md:bottom-end</c>).
	/// Takes precedence over <see cref="Placement"/>.
	/// See <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#responsive">Bootstrap documentation</see>.
	/// </summary>
	[Parameter] public string ResponsivePlacement { get; set; }

	/// <summary>
	/// By default, the menu is closed when clicking inside or outside the menu (<see cref="MenuAutoClose.True"/>).
	/// You can use the AutoClose parameter to change this behavior of the menu.
	/// See <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior</see> for more information.
	/// </summary>
	[Parameter] public MenuAutoClose AutoClose { get; set; } = MenuAutoClose.True;

	/// <summary>
	/// Any additional CSS class to apply to the <c>.menu</c> element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>.menu</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	bool IMenuContainer.IsOpen { get; set; }
}
