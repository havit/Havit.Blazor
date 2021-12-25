using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxModal"/> component
	/// </summary>
	public record ModalSettings
	{
		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// </summary>
		public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c> (default), Bootstrap default close-button will be used.
		/// </summary>
		public IconBase CloseButtonIcon { get; set; } = null;

		/// <summary>
		/// Size of the modal. Default is <see cref="ModalSize.Regular"/>.
		/// </summary>
		public ModalSize Size { get; set; } = ModalSize.Regular;

		/// <summary>
		/// Fullscreen behavior of the modal. Default is <see cref="ModalFullscreen.Disabled"/>.
		/// </summary>
		public ModalFullscreen Fullscreen { get; set; } = ModalFullscreen.Disabled;

		/// <summary>
		/// Allows scrolling the modal body. Default is <c>false</c>.
		/// </summary>
		public bool Scrollable { get; set; } = false;

		/// <summary>
		/// Allows vertical centering of the modal. Default is <c>false</c> (horizontal only).
		/// </summary>
		public bool Centered { get; set; } = false;

		/// <summary>
		/// Additional CSS class for the main element (<c>div.modal</c>).
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the dialog (<c>div.modal-dialog</c> element).
		/// </summary>
		public string DialogCssClass { get; set; }

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		public string BodyCssClass { get; set; }

		/// <summary>
		/// Footer css class.
		/// </summary>
		public string FooterCssClass { get; set; }
	}
}
