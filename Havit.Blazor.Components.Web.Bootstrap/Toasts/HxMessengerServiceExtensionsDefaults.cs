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
	public class HxMessengerServiceExtensionsDefaults
	{
		/// <summary>
		/// Default icon for information.
		/// </summary>
		public IconBase InformationIcon { get; set; } = BootstrapIcon.InfoCircleFill;

		/// <summary>
		/// Default css class for information.
		/// </summary>
		public string InformationCssClass { get; set; } = "hx-messenger-information";

		/// <summary>
		/// Default autohide delay for information.
		/// </summary>
		public int? InformationAutohideDelay { get; set; } = 5000;

		/// <summary>
		/// Default icon for warnings.
		/// </summary>
		public IconBase WarningIcon { get; set; } = BootstrapIcon.ExclamationCircleFill;

		/// <summary>
		/// Default css class for warnings.
		/// </summary>
		public string WarningCssClass { get; set; } = "hx-messenger-warning";

		/// <summary>
		/// Default autohide delay for warnings.
		/// </summary>
		public int? WarningAutohideDelay { get; set; } = null;

		/// <summary>
		/// Default icon for errors.
		/// </summary>
		public IconBase ErrorIcon { get; set; } = BootstrapIcon.XCircleFill;

		/// <summary>
		/// Default css class for errors.
		/// </summary>
		public string ErrorCssClass { get; set; } = "hx-messenger-error";

		/// <summary>
		/// Default autohide delay for errors.
		/// </summary>
		public int? ErrorAutohideDelay { get; set; } = null;

	}
}
