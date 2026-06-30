# HxDialog_Demo_Footer.razor

```razor
<HxButton OnClick="HandleShowClick" Color="ThemeColor.Primary">Show dialog</HxButton>

<HxDialog @ref="myDialog" Title="Edit profile">
	<BodyTemplate>
		<HxInputText Label="Name" @bind-Value="name" />
	</BodyTemplate>
	<FooterTemplate>
		<HxButton Text="Cancel" OnClick="HandleHideClick" Color="ThemeColor.Secondary" />
		<HxButton Text="Save changes" OnClick="HandleHideClick" Color="ThemeColor.Primary" />
	</FooterTemplate>
</HxDialog>

@code
{
	private HxDialog myDialog;
	private string name;

	private async Task HandleShowClick()
	{
		await myDialog.ShowAsync();
	}

	private async Task HandleHideClick()
	{
		await myDialog.HideAsync();
	}
}

```
