using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Extension methods for <see cref="IHxMessengerService"/>.
	/// </summary>
	public static class HxMessengerServiceExtensions
	{
		/// <summary>
		/// Default values for extension methods.
		/// </summary>
		public static MessengerServiceExtensionsSettings Defaults { get; } = new MessengerServiceExtensionsSettings();

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
				Icon = Defaults.InformationIcon,
				CssClass = Defaults.InformationCssClass,
				AutohideDelay = Defaults.InformationAutohideDelay,
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
				Icon = Defaults.WarningIcon,
				CssClass = Defaults.WarningCssClass,
				AutohideDelay = Defaults.WarningAutohideDelay,
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
				Icon = Defaults.ErrorIcon,
				CssClass = Defaults.ErrorCssClass,
				AutohideDelay = Defaults.ErrorAutohideDelay,
				Title = title,
				Text = message
			});
		}
	}
}
