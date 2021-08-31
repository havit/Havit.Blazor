namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class ModalDefaults
	{
		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// </summary>
		public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Size of the modal.
		/// </summary>
		public ModalSize Size { get; set; }

		/// <summary>
		/// Fullscreen behavior of the modal.
		/// </summary>
		public ModalFullscreen Fullscreen { get; set; } = ModalFullscreen.Disabled;
	}
}
