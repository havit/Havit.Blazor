# HxMenu_Demo_Reference.razor

```razor
<HxMenu>
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary" MenuReference="#my-reference">Menu button</HxMenuToggleButton>
	</Toggle>
	<Content>
		<HxMenuText>Text item 1</HxMenuText>
		<HxMenuText>Text item 2</HxMenuText>
		<HxMenuDivider />
		<HxMenuText>Text item 3</HxMenuText>
	</Content>
</HxMenu>

<div id="my-reference" class="bg-info d-inline-block px-3 py-1 ms-5">Reference element</div>
```
