# HxDialog_Demo_Swapping.razor

```razor
<HxButton OnClick="() => firstDialog.ShowAsync()" Color="ThemeColor.Primary">Open first dialog</HxButton>

<HxDialog @ref="firstDialog" Title="First dialog">
	<BodyTemplate>
		Opening the second dialog automatically swaps it in place of this one &ndash; the backdrop stays visible the whole time, with no flicker.
	</BodyTemplate>
	<FooterTemplate>
		<HxButton OnClick="() => firstDialog.HideAsync()" Color="ThemeColor.Secondary">Close</HxButton>
		<HxButton OnClick="() => secondDialog.ShowAsync()" Color="ThemeColor.Primary">Open second dialog</HxButton>
	</FooterTemplate>
</HxDialog>

<HxDialog @ref="secondDialog" Title="Second dialog">
	<BodyTemplate>
		This is the second dialog. You can swap back to the first one.
	</BodyTemplate>
	<FooterTemplate>
		<HxButton OnClick="() => secondDialog.HideAsync()" Color="ThemeColor.Secondary">Close</HxButton>
		<HxButton OnClick="() => firstDialog.ShowAsync()" Color="ThemeColor.Primary">Back to first dialog</HxButton>
	</FooterTemplate>
</HxDialog>

@code
{
	private HxDialog firstDialog;
	private HxDialog secondDialog;
}

```
