using System.Net;

namespace Havit.Blazor.Components.Web.Bootstrap;

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
	/// Adds and shows an informational message. The message is automatically hidden 5 seconds after showing up.
	/// </summary>
	public static void AddInformation(this IHxMessengerService messenger, string message)
	{
		AddInformation(messenger, title: null, message);
	}

	/// <summary>
	/// Adds and shows an informational message. The message is automatically hidden 5 seconds after showing up.
	/// </summary>
	public static void AddInformation(this IHxMessengerService messenger, string title, string message)
	{
		Contract.Requires<ArgumentNullException>(messenger != null, nameof(messenger));

		messenger.AddMessage(new BootstrapMessengerMessage()
		{
			Color = Defaults.InformationColor,
			AutohideDelay = Defaults.InformationAutohideDelay,
			ContentTemplate = BuildContentTemplate(title, message)
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

		messenger.AddMessage(new BootstrapMessengerMessage()
		{
			Color = Defaults.WarningColor,
			AutohideDelay = Defaults.WarningAutohideDelay,
			ContentTemplate = BuildContentTemplate(title, message)
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

		messenger.AddMessage(new BootstrapMessengerMessage()
		{
			Color = Defaults.ErrorColor,
			AutohideDelay = Defaults.ErrorAutohideDelay,
			ContentTemplate = BuildContentTemplate(title, message)
		});
	}

	private static RenderFragment BuildContentTemplate(string title, string text)
	{
		return (RenderTreeBuilder builder) =>
		{
			if (title != null)
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", "fw-bold");
				builder.AddContent(3, ProcessLineEndings(title));
				builder.CloseElement();
			}

			builder.AddContent(10, ProcessLineEndings(text));
		};
	}

	private static MarkupString ProcessLineEndings(string text)
	{
		return new MarkupString(WebUtility.HtmlEncode(text)?.ReplaceLineEndings("<br />"));
	}
}
