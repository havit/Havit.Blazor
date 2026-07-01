# HxCard_Demo_Variants.razor

```razor
<HxCard Color="ThemeColor.Primary" CssClass="mb-3" style="max-width: 18rem;">
	<HeaderTemplate>Header</HeaderTemplate>
	<BodyTemplate>
		<HxCardTitle>Primary card title</HxCardTitle>
		<HxCardText>Some quick example text to build on the card title and make up the bulk of the card's content.</HxCardText>
	</BodyTemplate>
</HxCard>

<HxCard Variant="CardVariant.Subtle" Color="ThemeColor.Primary" CssClass="mb-3" style="max-width: 18rem;">
	<HeaderTemplate>Header</HeaderTemplate>
	<BodyTemplate>
		<HxCardTitle>Subtle primary card</HxCardTitle>
		<HxCardText>Some quick example text to build on the card title and make up the bulk of the card's content.</HxCardText>
	</BodyTemplate>
</HxCard>

<HxCard Variant="CardVariant.Subtle" Color="ThemeColor.Danger" CssClass="mb-3" style="max-width: 18rem;">
	<HeaderTemplate>Header</HeaderTemplate>
	<BodyTemplate>
		<HxCardTitle>Subtle danger card</HxCardTitle>
		<HxCardText>Some quick example text to build on the card title and make up the bulk of the card's content.</HxCardText>
	</BodyTemplate>
</HxCard>

<HxCard Variant="CardVariant.Translucent" CssClass="mb-3" style="max-width: 18rem;">
	<HeaderTemplate>Header</HeaderTemplate>
	<BodyTemplate>
		<HxCardTitle>Translucent card</HxCardTitle>
		<HxCardText>Frosted-glass translucency (new in Bootstrap 6).</HxCardText>
	</BodyTemplate>
</HxCard>

```
