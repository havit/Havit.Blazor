namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxModal"/> component<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/ModalSettings">https://havit.blazor.eu/types/ModalSettings</see>
	/// </summary>
	public record ModalSettings
	{
		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// </summary>
		public bool? ShowCloseButton { get; set; }

		/// <summary>
		/// Close icon to be used in header.
		/// </summary>
		public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Indicates whether the modal closes when escape key is pressed.
		/// </summary>
		public bool? CloseOnEscape { get; set; }

		/// <summary>
		/// Size of the modal.
		/// </summary>
		public ModalSize? Size { get; set; }

		/// <summary>
		/// Fullscreen behavior of the modal.
		/// </summary>
		public ModalFullscreen? Fullscreen { get; set; }

		/// <summary>
		/// Indicates whether the modal uses a static backdrop.
		/// </summary>
		public bool? UseStaticBackdrop { get; set; }

		/// <summary>
		/// Allows scrolling the modal body.
		/// </summary>
		public bool? Scrollable { get; set; }

		/// <summary>
		/// Allows vertical centering of the modal.
		/// </summary>
		public bool? Centered { get; set; }

		/// <summary>
		/// Additional CSS class for the main element (<c>div.modal</c>).
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the dialog (<c>div.modal-dialog</c> element).
		/// </summary>
		public string DialogCssClass { get; set; }

		/// <summary>
		/// Additional header CSS class (<c>div.modal-header</c>).
		/// </summary>
		public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional content CSS class (<c>div.modal-content</c>).
		/// </summary>
		public string ContentCssClass { get; set; }

		/// <summary>
		/// Additional body CSS class (<c>div.modal-body</c>).
		/// </summary>
		public string BodyCssClass { get; set; }

		/// <summary>
		/// Additional footer CSS class (<c>div.modal-footer</c>).
		/// </summary>
		public string FooterCssClass { get; set; }
	}
}
