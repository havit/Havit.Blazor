using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web;
using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Defaults for <see cref="HxMessengerServiceExtensions"/>.
	/// </summary>
	public record MessengerServiceExtensionsSettings
	{
		/// <summary>
		/// Default icon for information. Default is <see cref="BootstrapIcon.InfoCircleFill"/>.
		/// </summary>
		public IconBase InformationIcon { get; set; } = BootstrapIcon.InfoCircleFill;

		/// <summary>
		/// Default css class for information. Default is <c>hx-messenger-information</c>.
		/// </summary>
		public string InformationCssClass { get; set; } = "hx-messenger-information";

		/// <summary>
		/// Default autohide delay for information (in miliseconds). Default is <c>5000</c> ms.
		/// </summary>
		public int? InformationAutohideDelay { get; set; } = 5000;

		/// <summary>
		/// Default icon for warnings. Default is <see cref="BootstrapIcon.ExclamationCircleFill"/>.
		/// </summary>
		public IconBase WarningIcon { get; set; } = BootstrapIcon.ExclamationCircleFill;

		/// <summary>
		/// Default css class for warnings. Default is <c>hx-messenger-warning</c>.
		/// </summary>
		public string WarningCssClass { get; set; } = "hx-messenger-warning";

		/// <summary>
		/// Default autohide delay for warnings (in miliseconds). Default is <c>null</c> (do not autohide).
		/// </summary>
		public int? WarningAutohideDelay { get; set; } = null;

		/// <summary>
		/// Default icon for errors. Default is <see cref="BootstrapIcon.XCircleFill"/>.
		/// </summary>
		public IconBase ErrorIcon { get; set; } = BootstrapIcon.XCircleFill;

		/// <summary>
		/// Default css class for errors. Default is <c>hx-messenger-error</c>.
		/// </summary>
		public string ErrorCssClass { get; set; } = "hx-messenger-error";

		/// <summary>
		/// Default autohide delay for errors (in miliseconds). Default is <c>null</c> (do not autohide).
		/// </summary>
		public int? ErrorAutohideDelay { get; set; } = null;
	}
}
