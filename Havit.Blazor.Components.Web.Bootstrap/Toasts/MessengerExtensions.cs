using System;
using System.Collections.Generic;
using System.Text;
using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	public static class MessengerExtensions
	{
		public static void AddInformation(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new Message
			{
				Icon = BootstrapIcon.InfoCircle,
				CssClass = "toast-information",
				AutohideDelay = 5000,
				Text = message
			});
		}

		public static void AddWarning(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new Message
			{
				Icon = BootstrapIcon.ExclamationCircle,
				CssClass = "toast-warning",
				AutohideDelay = null,
				Text = message
			});

		}

		public static void AddError(this IMessenger messenger, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new Message
			{
				Icon = BootstrapIcon.ExclamationCircleFill,
				CssClass = "toast-error",
				AutohideDelay = null,
				Text = message
			});
		}
	}
}
