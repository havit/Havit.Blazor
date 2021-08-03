using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Application wide defaults for <see cref="HxOffcanvas"/>.
	/// </summary>
	public class OffcanvasDefaults
	{
		public IconBase CloseButtonIcon { get; set; }

		public bool ShowCloseButton { get; set; } = true;

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

		/// <summary>
		/// Offcanvas additional CSS class. Added to root div (.offcanvas).
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// Default value is <c>true</c>.
		/// </summary>
		public bool BackdropEnabled { get; set; } = true;
	}
}
