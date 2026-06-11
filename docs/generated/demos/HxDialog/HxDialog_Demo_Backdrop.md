# HxDialog_Demo_Backdrop.razor

```razor
<HxButton OnClick="HandleShowClick" Color="ThemeColor.Primary">Show dialog</HxButton>

<HxDialog @ref="myDialog" Title="Dialog title" Backdrop="DialogBackdrop.Static">
    <BodyTemplate>
        Dialog body
    </BodyTemplate>
    <FooterTemplate>
        <HxButton Text="Close" OnClick="HandleHideClick" Color="ThemeColor.Primary" />
    </FooterTemplate>
</HxDialog>

@code
{
    private HxDialog myDialog;

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
