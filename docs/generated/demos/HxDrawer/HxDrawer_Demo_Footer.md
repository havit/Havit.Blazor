# HxDrawer_Demo_Footer.razor

```razor
<HxDrawer @ref="drawerComponent" Title="Edit profile">
	<BodyTemplate>
		<p>Content for the drawer goes here. The footer stays pinned to the bottom.</p>
		<HxInputText Label="Name" @bind-Value="name" />
	</BodyTemplate>
	<FooterTemplate>
		<HxButton OnClick="HandleHide" Text="Cancel" Color="ThemeColor.Secondary" />
		<HxButton OnClick="HandleHide" Text="Save changes" Color="ThemeColor.Primary" />
	</FooterTemplate>
</HxDrawer>

<HxButton OnClick="HandleShow" Text="Show" Color="ThemeColor.Primary" />

@code
{
	private HxDrawer drawerComponent;
	private string name;

	private async Task HandleShow()
	{
		await drawerComponent.ShowAsync();
	}

	private async Task HandleHide()
	{
		await drawerComponent.HideAsync();
	}
}

```
