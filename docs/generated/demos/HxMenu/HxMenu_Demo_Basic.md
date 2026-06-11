# HxMenu_Demo_Basic.razor

```razor
<HxMenu>
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Menu button</HxMenuToggleButton>
	</Toggle>
	<Content>
		<HxMenuItemNavLink Href="/components/HxMenu">Link with Href</HxMenuItemNavLink>
		<HxMenuItem OnClick="HandleMenuItemClick">Item with OnClick action</HxMenuItem>
		<HxMenuDivider />
		<HxMenuText>Some text only</HxMenuText>
	</Content>
</HxMenu>
@message
@code {
	private string message;
	private void HandleMenuItemClick() => message = "Menu item clicked";
}

```
