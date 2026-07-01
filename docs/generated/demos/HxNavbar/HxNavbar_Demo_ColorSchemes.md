# HxNavbar_Demo_ColorSchemes.razor

```razor
<HxNavbar ColorMode="ColorMode.Dark" Color="ThemeColor.Inverse">
	<HxNavbarBrand>Navbar</HxNavbarBrand>
	<HxNavbarToggler />
	<HxNavbarDrawer>
		<HxNav CssClass="me-auto mb-2 lg:mb-0">
			<HxNavLink Href="/components/HxNavbar">Home</HxNavLink>
			<HxNavLink Href="#">Link</HxNavLink>
			<HxMenu>
				<Toggle>
					<HxMenuToggleElement ElementName="a" role="button">Menu</HxMenuToggleElement>
				</Toggle>
				<Content>
					<HxMenuItemNavLink Href="#">Action</HxMenuItemNavLink>
					<HxMenuItemNavLink Href="#">Another action</HxMenuItemNavLink>
					<HxMenuDivider />
					<HxMenuItemNavLink Href="#">Something else here</HxMenuItemNavLink>
				</Content>
			</HxMenu>
			<HxNavLink Enabled="false">Disabled</HxNavLink>
		</HxNav>
		<div class="d-flex">
			<HxInputText CssClass="me-2" Placeholder="Search" @bind-Value="@query" />
			<HxSubmit Color="ThemeColor.Secondary" Variant="ButtonVariant.Outline">Search</HxSubmit>
		</div>
	</HxNavbarDrawer>
</HxNavbar>

<HxNavbar ColorMode="ColorMode.Dark" Color="ThemeColor.Primary" CssClass="mt-3">
	<HxNavbarBrand>Navbar</HxNavbarBrand>
	<HxNavbarToggler />
	<HxNavbarDrawer>
		<HxNav CssClass="me-auto mb-2 lg:mb-0">
			<HxNavLink Href="/components/HxNavbar">Home</HxNavLink>
			<HxNavLink Href="#">Link</HxNavLink>
			<HxMenu>
				<Toggle>
					<HxMenuToggleElement ElementName="a" role="button">Menu</HxMenuToggleElement>
				</Toggle>
				<Content>
					<HxMenuItemNavLink Href="#">Action</HxMenuItemNavLink>
					<HxMenuItemNavLink Href="#">Another action</HxMenuItemNavLink>
					<HxMenuDivider />
					<HxMenuItemNavLink Href="#">Something else here</HxMenuItemNavLink>
				</Content>
			</HxMenu>
			<HxNavLink Enabled="false">Disabled</HxNavLink>
		</HxNav>
		<div class="d-flex">
			<HxInputText CssClass="me-2" Placeholder="Search" @bind-Value="@query" />
			<HxSubmit Color="ThemeColor.Secondary" Variant="ButtonVariant.Outline">Search</HxSubmit>
		</div>
	</HxNavbarDrawer>
</HxNavbar>

<HxNavbar Color="ThemeColor.Warning" CssClass="mt-3">
	<HxNavbarBrand>Navbar</HxNavbarBrand>
	<HxNavbarToggler />
	<HxNavbarDrawer>
		<HxNav CssClass="me-auto mb-2 lg:mb-0">
			<HxNavLink Href="/components/HxNavbar">Home</HxNavLink>
			<HxNavLink Href="#">Link</HxNavLink>
			<HxMenu>
				<Toggle>
					<HxMenuToggleElement ElementName="a" role="button">Menu</HxMenuToggleElement>
				</Toggle>
				<Content>
					<HxMenuItemNavLink Href="#">Action</HxMenuItemNavLink>
					<HxMenuItemNavLink Href="#">Another action</HxMenuItemNavLink>
					<HxMenuDivider />
					<HxMenuItemNavLink Href="#">Something else here</HxMenuItemNavLink>
				</Content>
			</HxMenu>
			<HxNavLink Enabled="false">Disabled</HxNavLink>
		</HxNav>
		<div class="d-flex">
			<HxInputText CssClass="me-2" Placeholder="Search" @bind-Value="@query" />
			<HxSubmit Color="ThemeColor.Secondary" Variant="ButtonVariant.Outline">Search</HxSubmit>
		</div>
	</HxNavbarDrawer>
</HxNavbar>

@code {
	private string query = String.Empty;
}
```
