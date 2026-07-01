# HxDialog_Demo_Scrollable.razor

```razor
<HxButton OnClick="() => myDialog.ShowAsync()" Color="ThemeColor.Primary">Launch demo dialog - Scrollable</HxButton>

<HxDialog @ref="myDialog" Title="Dialog title" Scrollable="true">
	<BodyTemplate>
		<p style="margin-bottom: 100vh;">
			This is some placeholder content to show the scrolling behavior for dialogs. Instead of repeating the text the dialog, we use an inline style set a minimum height, thereby extending the length of the overall dialog and demonstrating the overflow scrolling. When content becomes longer than the height of the viewport, scrolling will move the dialog as needed.
		</p>
		<p>This content should appear at the bottom after you scroll.</p>
	</BodyTemplate>
	<FooterTemplate>
		<HxButton OnClick="() => myDialog.HideAsync()" Color="ThemeColor.Primary">Close</HxButton>
	</FooterTemplate>
</HxDialog>

@code
{
	private HxDialog myDialog;
}

```
