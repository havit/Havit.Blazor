# HxMenu_Demo_HeaderDisabledActive.razor

```razor
<HxMenu>
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Menu</HxMenuToggleButton>
	</Toggle>
	<Content>
		<HxMenuHeader>Menu header</HxMenuHeader>
		<HxMenuItemNavLink Href="#">Link with Href</HxMenuItemNavLink>
		<HxMenuItemNavLink Href="#" Enabled="false">Disabled item</HxMenuItemNavLink>
		<HxMenuItemNavLink Href="#">Some other item</HxMenuItemNavLink>
		<HxMenuItemNavLink Href="#" CssClass="active">Active link with Href</HxMenuItemNavLink>
		<HxMenuText>Some other item</HxMenuText>
	</Content>
</HxMenu>
```
