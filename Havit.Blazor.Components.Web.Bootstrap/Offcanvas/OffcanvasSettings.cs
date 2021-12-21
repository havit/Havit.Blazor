using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxOffcanvas"/> component.
	/// </summary>
	public class OffcanvasSettings
	{
		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// Use <see cref="CloseButtonIcon"/> to change shape of the button.
		/// </summary>
		public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// Default value is <c>true</c>.
		/// </summary>
		public bool BackdropEnabled { get; set; } = true;

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
