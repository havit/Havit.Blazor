namespace Havit.Blazor.Components.Web.Bootstrap;

public static class MessageBoxServiceExtensions
{
	public static Task<MessageBoxButtons> ShowAsync(this IHxMessageBoxService messageBoxService, string title, string text, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxButtons? primaryButton = null, string customButtonText = null)
	{
		return messageBoxService.ShowAsync(new MessageBoxRequest()
		{
			Title = title,
			Text = text,
			Buttons = buttons,
			PrimaryButton = primaryButton,
			CustomButtonText = customButtonText
		});
	}

	public static async Task<bool> ConfirmAsync(this IHxMessageBoxService messageBoxService, string title, MarkupString markupText)
	{
		var result = await messageBoxService.ShowAsync(new MessageBoxRequest()
		{
			Title = title,
			BodyTemplate = builder => builder.AddMarkupContent(0, markupText.Value),
			Buttons = MessageBoxButtons.OkCancel
		});

		return (result == MessageBoxButtons.Ok);
	}

	public static async Task<bool> ConfirmAsync(this IHxMessageBoxService messageBoxService, MarkupString markupText)
	{
		return await messageBoxService.ConfirmAsync(title: "Confirmation", markupText); // TODO Localization
	}

	public static async Task<bool> ConfirmAsync(this IHxMessageBoxService messageBoxService, string title, string text)
	{
		var result = await messageBoxService.ShowAsync(title, text, MessageBoxButtons.OkCancel);

		return (result == MessageBoxButtons.Ok);
	}

	public static Task<bool> ConfirmAsync(this IHxMessageBoxService messageBoxService, string text)
	{
		return messageBoxService.ConfirmAsync(title: "Confirmation", text); // TODO Localization
	}
}
