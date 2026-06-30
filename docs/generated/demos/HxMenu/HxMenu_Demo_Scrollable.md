# HxMenu_Demo_Scrollable.razor

```razor
<HxMenu CssClass="menu-scrollable">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Scrollable menu</HxMenuToggleButton>
	</Toggle>
	<Content>
		@for (var i = 1; i <= 12; i++)
		{
			var index = i;
			<HxMenuItemNavLink Href="#">Item @index</HxMenuItemNavLink>
		}
	</Content>
</HxMenu>

```
