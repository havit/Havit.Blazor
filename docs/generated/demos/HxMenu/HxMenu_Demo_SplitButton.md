# HxMenu_Demo_SplitButton.razor

```razor
<HxButtonGroup>
	<HxButton Color="ThemeColor.Secondary" OnClick="HandleMainButtonClick">Action button</HxButton>
	<HxMenu>
		<Toggle>
			<HxMenuToggleButton Color="ThemeColor.Secondary">
				<span class="visually-hidden">Toggle menu</span>@* OPTIONAL (for accessibility) *@
			</HxMenuToggleButton>
		</Toggle>
		<Content>
			<HxMenuItemNavLink Href="#">Link with Href</HxMenuItemNavLink>
			<HxMenuItem OnClick="HandleMenuItemClick">Item with OnClick action</HxMenuItem>
			<HxMenuDivider />
			<HxMenuText>Something else here</HxMenuText>
		</Content>
	</HxMenu>
</HxButtonGroup>

@message
@code {
	private string message;
	private void HandleMainButtonClick() => message = "Main button clicked";
	private void HandleMenuItemClick() => message = "Menu item clicked";
}

```
