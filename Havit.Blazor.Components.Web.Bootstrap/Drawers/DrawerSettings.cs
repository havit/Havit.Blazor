namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxDrawer"/> component.
/// </summary>
public record DrawerSettings
{
	/// <summary>
	/// Close icon to be used in the header.
	/// If set to <c>null</c>, the Bootstrap default close button will be used.
	/// </summary>
	public IconBase CloseButtonIcon { get; set; }

	/// <summary>
	/// Indicates whether the drawer shows a close button in the header.
	/// Use the <see cref="CloseButtonIcon"/> property to change the shape of the button.
	/// </summary>
	public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// Indicates whether to apply a backdrop on the body while the drawer is open.
	/// </summary>
	public DrawerBackdrop? Backdrop { get; set; }

	/// <summary>
	/// The breakpoint below which the contents are rendered outside the viewport in an drawer (above this breakpoint, the drawer body is rendered inside the viewport).
	/// </summary>
	public DrawerResponsiveBreakpoint? ResponsiveBreakpoint { get; set; }

	/// <summary>
	/// The placement of the drawer.
	/// </summary>
	public DrawerPlacement? Placement { get; set; }

	/// <summary>
	/// The size of the drawer.
	/// </summary>
	/// <remarks>
	/// Consider customizing the drawer CSS variables instead of changing the size application-wide.
	/// </remarks>
	public DrawerSize? Size { get; set; }

	/// <summary>
	/// Indicates whether the drawer closes when the escape key is pressed.
	/// </summary>
	public bool? CloseOnEscape { get; set; }

	/// <summary>
	/// Indicates whether body (page) scrolling is allowed while the drawer is open.
	/// </summary>
	public bool? ScrollingEnabled { get; set; }

	/// <summary>
	/// When <c>true</c>, renders the drawer as a flush-to-edge sheet (no inset, border radius, or shadow) using the <c>drawer-sheet</c> variant (new in Bootstrap 6).
	/// </summary>
	public bool? Sheet { get; set; }

	/// <summary>
	/// Additional CSS class for the drawer. Added to the root div (<c>.drawer</c>).
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the header.
	/// </summary>
	public string HeaderCssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the body.
	/// </summary>
	public string BodyCssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the footer.
	/// </summary>
	public string FooterCssClass { get; set; }
}
