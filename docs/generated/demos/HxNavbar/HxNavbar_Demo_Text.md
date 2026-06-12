# HxNavbar_Demo_Text.razor

```razor
<HxNavbar CssClass="bg-body-tertiary">
	<HxNavbarText>Navbar text with an inline element</HxNavbarText>
</HxNavbar>

<HxNavbar CssClass="bg-body-tertiary mt-3">
	<HxNavbarBrand>Navbar w/ text</HxNavbarBrand>
	<HxNavbarToggler />
	<HxNavbarDrawer>
		<HxNav CssClass="me-auto mb-2 lg:mb-0">
			<HxNavLink Href="/components/HxNavbar">Home</HxNavLink>
			<HxNavLink Href="#">Link</HxNavLink>
			<HxNavLink Enabled="false">Disabled</HxNavLink>
		</HxNav>
		<HxNavbarText>Navbar text with an inline element</HxNavbarText>
	</HxNavbarDrawer>
</HxNavbar>
```
