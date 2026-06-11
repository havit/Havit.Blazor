# HxMenu_Demo_AutoClose.razor

```razor
<HxMenu AutoClose="MenuAutoClose.True" CssClass="p-2">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Default menu</HxMenuToggleButton>
	</Toggle>
	<Content>
		Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus elementum bibendum arcu elementum molestie.
	</Content>
</HxMenu>

<HxMenu AutoClose="MenuAutoClose.Outside" CssClass="p-2">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Clickable inside</HxMenuToggleButton>
	</Toggle>
	<Content>
		Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus elementum bibendum arcu elementum molestie.
	</Content>
</HxMenu>

<HxMenu AutoClose="MenuAutoClose.Inside" CssClass="p-2">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Clickable outside</HxMenuToggleButton>
	</Toggle>
	<Content>
		Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus elementum bibendum arcu elementum molestie.
	</Content>
</HxMenu>

<HxMenu AutoClose="MenuAutoClose.False" CssClass="p-2">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Manual close</HxMenuToggleButton>
	</Toggle>
	<Content>
		Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus elementum bibendum arcu elementum molestie.
	</Content>
</HxMenu>

```
