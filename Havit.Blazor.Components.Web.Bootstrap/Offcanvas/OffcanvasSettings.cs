namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxOffcanvas"/> component.
/// </summary>
public record OffcanvasSettings
{
	/// <summary>
	/// Close icon to be used in the header.
	/// If set to <c>null</c>, the Bootstrap default close button will be used.
	/// </summary>
	public IconBase CloseButtonIcon { get; set; }

	/// <summary>
	/// Indicates whether the modal shows a close button in the header.
	/// Use the <see cref="CloseButtonIcon"/> property to change the shape of the button.
	/// </summary>
	public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// Indicates whether to apply a backdrop on the body while the offcanvas is open.
	/// </summary>
	public OffcanvasBackdrop? Backdrop { get; set; }

	/// <summary>
	/// The breakpoint below which the contents are rendered outside the viewport in an offcanvas (above this breakpoint, the offcanvas body is rendered inside the viewport).
	/// </summary>
	public OffcanvasResponsiveBreakpoint? ResponsiveBreakpoint { get; set; }

	/// <summary>
	/// The placement of the offcanvas.
	/// </summary>
	public OffcanvasPlacement? Placement { get; set; }

	/// <summary>
	/// The size of the offcanvas.
	/// </summary>
	/// <remarks>
	/// Consider customizing the offcanvas CSS variables instead of changing the size application-wide.
	/// </remarks>
	public OffcanvasSize? Size { get; set; }

	/// <summary>
	/// Indicates whether the offcanvas closes when the escape key is pressed.
	/// </summary>
	public bool? CloseOnEscape { get; set; }

	/// <summary>
	/// Indicates whether body (page) scrolling is allowed while the offcanvas is open.
	/// </summary>
	public bool? ScrollingEnabled { get; set; }

	/// <summary>
	/// Additional CSS class for the offcanvas. Added to the root div (<c>.offcanvas</c>).
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
