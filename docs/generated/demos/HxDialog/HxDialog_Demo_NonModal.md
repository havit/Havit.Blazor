# HxDialog_Demo_NonModal.razor

```razor
<HxButton OnClick="() => myDialog.ShowAsync()" Color="ThemeColor.Primary">Open non-modal dialog</HxButton>

<HxDialog @ref="myDialog" Title="Non-modal dialog" NonModal="true">
	<BodyTemplate>
		This dialog is opened with the browser-native <code>show()</code> instead of <code>showModal()</code>. There is no backdrop, no focus trap, and the rest of the page stays interactive &ndash; you can keep clicking the button behind it.
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
