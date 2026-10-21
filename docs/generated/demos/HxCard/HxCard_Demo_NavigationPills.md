# HxCard_Demo_NavigationPills.razor

```razor
<HxCard>
    <HeaderTemplate>
        <HxNav Variant="NavVariant.Pills">
            <HxNavLink Href="" CssClass="active">Active</HxNavLink>
            <HxNavLink Href="#">Link</HxNavLink>
            <HxNavLink Enabled="false">Disabled</HxNavLink>
        </HxNav>
    </HeaderTemplate>
    <BodyTemplate>
        <HxCardTitle>Special title treatment</HxCardTitle>
        <HxCardText>With supporting text below as a natural lead-in to additional content.</HxCardText>
        <HxButton Color="ThemeColor.Primary">Go somewhere</HxButton>
    </BodyTemplate>
</HxCard>
```
