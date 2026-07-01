# HxNavbar_Demo_Basic.razor

```razor
<HxNavbar CssClass="bg-body-tertiary">
    <HxNavbarBrand>Navbar</HxNavbarBrand>
    <HxNavbarToggler />
    <HxNavbarDrawer>
        <HxNav CssClass="me-auto mb-2 lg:mb-0">
            <HxNavLink Href="/components/HxNavbar">Home</HxNavLink>
            <HxNavLink Href="#">Link</HxNavLink>
            <HxMenu>
                <Toggle>
                    <HxMenuToggleElement ElementName="a" role="button">Menu</HxMenuToggleElement>
                </Toggle>
                <Content>
                    <HxMenuItemNavLink Href="#">Action</HxMenuItemNavLink>
                    <HxMenuItemNavLink Href="#">Another action</HxMenuItemNavLink>
                    <HxMenuDivider />
                    <HxMenuItemNavLink Href="#">Something else here</HxMenuItemNavLink>
                </Content>
            </HxMenu>
            <HxNavLink Enabled="false">Disabled</HxNavLink>
        </HxNav>
        <EditForm Model="@query">
            <div class="d-flex">
                <HxInputText CssClass="me-2" Placeholder="Search" @bind-Value="@query" />
                <HxSubmit Color="ThemeColor.Success" Variant="ButtonVariant.Outline">Search</HxSubmit>
            </div>
        </EditForm>
    </HxNavbarDrawer>
</HxNavbar>

@code {
    private string query = String.Empty;
}
```
