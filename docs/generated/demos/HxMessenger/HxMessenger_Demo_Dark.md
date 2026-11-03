# HxMessenger_Demo_Dark.razor

```razor
<HxButton Text="Show dark message" OnClick="HandleAddDarkMessageButtonClick" Color="ThemeColor.Primary" />

@code {
	[Inject] protected IHxMessengerService Messenger { get; set; }

	private void HandleAddDarkMessageButtonClick()
	{
		Messenger.AddMessage(new BootstrapMessengerMessage
		{
			Color = ThemeColor.Primary,
			Title = "Dark message",
			Text = "This message is rendered in dark mode.",
			AutohideDelay = 5000,
			AdditionalAttributes = new Dictionary<string, object> { ["data-bs-theme"] = "dark" }
		});
	}
}

```
