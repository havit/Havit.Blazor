# HxMenu_Demo_Submenus.razor

```razor
<HxMenu>
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Submenus</HxMenuToggleButton>
	</Toggle>
	<Content>
		<HxMenuItem OnClick='() => Select("New")'>New</HxMenuItem>
		<HxSubmenu Text="Open recent" Icon="BootstrapIcon.ClockHistory">
			<HxMenuItem OnClick='() => Select("project-a.sln")'>project-a.sln</HxMenuItem>
			<HxMenuItem OnClick='() => Select("project-b.sln")'>project-b.sln</HxMenuItem>
			<HxMenuDivider />
			<HxSubmenu Text="More…">
				<HxMenuItem OnClick='() => Select("archived.sln")'>archived.sln</HxMenuItem>
			</HxSubmenu>
		</HxSubmenu>
		<HxMenuDivider />
		<HxSubmenu Text="Export" Icon="BootstrapIcon.BoxArrowUp">
			<HxMenuItem OnClick='() => Select("PDF")'>PDF</HxMenuItem>
			<HxMenuItem OnClick='() => Select("PNG")'>PNG</HxMenuItem>
		</HxSubmenu>
		<HxSubmenu Text="Disabled submenu" Enabled="false">
			<HxMenuItem>Unreachable</HxMenuItem>
		</HxSubmenu>
	</Content>
</HxMenu>
@if (message is not null)
{
	<p class="mt-2">Selected: <strong>@message</strong></p>
}
@code {
	private string message;
	private void Select(string value) => message = value;
}

```
