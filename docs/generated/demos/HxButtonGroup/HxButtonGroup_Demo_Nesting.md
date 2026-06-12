# HxButtonGroup_Demo_Nesting.razor

```razor
<HxButtonGroup>
    <HxButton Text="1" Color="ThemeColor.Primary" />
    <HxButton Text="2" Color="ThemeColor.Primary" />
    <HxMenu>
        <Toggle>
            <HxMenuToggleButton Color="ThemeColor.Primary">Menu</HxMenuToggleButton>
        </Toggle>
        <Content>
            <HxMenuItemNavLink Href="#">
                Menu link
            </HxMenuItemNavLink>
            <HxMenuItemNavLink Href="#">
                Menu link
            </HxMenuItemNavLink>
        </Content>
    </HxMenu>
</HxButtonGroup>

```
