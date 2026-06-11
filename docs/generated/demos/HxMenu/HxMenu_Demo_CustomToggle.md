# HxMenu_Demo_CustomToggle.razor

```razor
<HxMenu>
	<Toggle>
		<HxMenuToggleElement ElementName="div" CssClass="bg-success p-3 text-white" role="button">
			Custom menu toggle element
			<HxIcon Icon="BootstrapIcon.Trash" />
		</HxMenuToggleElement>
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
