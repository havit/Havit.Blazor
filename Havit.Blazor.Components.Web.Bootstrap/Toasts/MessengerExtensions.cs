using System;
using System.Collections.Generic;
using System.Text;
using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Extension methods for <see cref="IMessenger"/>.
	/// </summary>
	public static class MessengerExtensions
	{
		/// <summary>
		/// Adds and shows an informational message. Message is automatically hidden 5 seconds after showing up.
		/// </summary>
		public static void AddInformation(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.InfoCircle,
				CssClass = "toast-information",
				AutohideDelay = 5000,
				Text = message
			});
		}

		/// <summary>
		/// Adds and shows a warning message.
		/// </summary>
		public static void AddWarning(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.ExclamationCircle,
				CssClass = "toast-warning",
				AutohideDelay = null,
				Text = message
			});

		}

		/// <summary>
		/// Adds and shows an error message.
		/// </summary>
		public static void AddError(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.ExclamationCircleFill,
				CssClass = "toast-error",
				AutohideDelay = null,
				Text = message
			});
		}
	}
}
