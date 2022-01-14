namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxOffcanvas"/> component.
	/// </summary>
	public record OffcanvasSettings
	{
		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Use <see cref="CloseButtonIcon"/> to change shape of the button.
		/// </summary>
		public bool? ShowCloseButton { get; set; }

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// </summary>
		public bool? BackdropEnabled { get; set; }

		/// <summary>
		/// Placement of the offcanvas.
		/// </summary>
		public OffcanvasPlacement? Placement { get; set; }

		/// <summary>
		/// Size of the offcanvas.<br/>
		/// </summary>
		/// <remarks>
		/// Consider customization of the offcanvas CSS variables instead of changing the Size application-wide.
		/// </remarks>
		public OffcanvasSize? Size { get; set; }

		/// <summary>
		/// Indicates whether the offcanvas closes when escape key is pressed.
		/// </summary>
		public bool? CloseOnEscape { get; set; }

		/// <summary>
		/// Indicates whether body (page) scrolling is allowed while offcanvas is open.
		/// </summary>
		public bool? ScrollingEnabled { get; set; }

		/// <summary>
		/// Offcanvas additional CSS class. Added to root div (<c>.offcanvas</c>).
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		public string BodyCssClass { get; set; }

		/// <summary>
		/// Additional footer CSS class.
		/// </summary>
		public string FooterCssClass { get; set; }
	}
}
