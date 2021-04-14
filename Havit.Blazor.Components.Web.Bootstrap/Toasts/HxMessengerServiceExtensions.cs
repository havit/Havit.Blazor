using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web;
using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Extension methods for <see cref="IHxMessengerService"/>.
	/// </summary>
	public static class HxMessengerServiceExtensions
	{
		/// <summary>
		/// Adds and shows an informational message. Message is automatically hidden 5 seconds after showing up.
		/// </summary>
		public static void AddInformation(this IHxMessengerService messenger, string message)
		{
			AddInformation(messenger, title: null, message);
		}

		/// <summary>
		/// Adds and shows an informational message. Message is automatically hidden 5 seconds after showing up.
		/// </summary>
		public static void AddInformation(this IHxMessengerService messenger, string title, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.InfoCircleFill,
				CssClass = "hx-messenger-information",
				AutohideDelay = 5000,
				Title = title,
				Text = message
			});
		}

		/// <summary>
		/// Adds and shows a warning message.
		/// </summary>
		public static void AddWarning(this IHxMessengerService messenger, string message)
		{
			AddWarning(messenger, title: null, message);
		}

		/// <summary>
		/// Adds and shows a warning message.
		/// </summary>
		public static void AddWarning(this IHxMessengerService messenger, string title, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.ExclamationCircleFill,
				CssClass = "hx-messenger-warning",
				AutohideDelay = null,
				Title = title,
				Text = message
			});

		}

		/// <summary>
		/// Adds and shows an error message.
		/// </summary>
		public static void AddError(this IHxMessengerService messenger, string message)
		{
			AddError(messenger, title: null, message);
		}

		/// <summary>
		/// Adds and shows an error message.
		/// </summary>
		public static void AddError(this IHxMessengerService messenger, string title, string message)
		{
			Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

			messenger.AddMessage(new MessengerMessage
			{
				Icon = BootstrapIcon.XCircleFill,
				CssClass = "hx-messenger-error",
				AutohideDelay = null,
				Title = title,
				Text = message
			});
		}
	}
}
