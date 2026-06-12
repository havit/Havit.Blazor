# HxNav_Demo_Menus.razor

```razor
<HxNav>
	<HxNavLink Href="/components/HxNav" Match="NavLinkMatch.Prefix">Active</HxNavLink>
	<HxMenu>
		<Toggle>
			<HxMenuToggleElement ElementName="a" role="button">Menu</HxMenuToggleElement>
		</Toggle>
		<Content>
			<HxMenuItemNavLink Href="#">Item 1</HxMenuItemNavLink>
			<HxMenuItemNavLink Href="#">Item 2</HxMenuItemNavLink>
			<HxMenuItemNavLink Href="#">Item 3</HxMenuItemNavLink>
		</Content>
	</HxMenu>
	<HxNavLink Href="#" Text="Link" />
	<HxNavLink Href="#" Text="Link" />
	<HxNavLink Href="#" Text="Disabled" Enabled="false"/>
</HxNav>
```
