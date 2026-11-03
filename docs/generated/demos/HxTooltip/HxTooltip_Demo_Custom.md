# HxTooltip_Demo_Custom.razor

```razor
<style>
	/* Demonstration purposes only. Put this CSS into your application's global CSS file. Isolated (.razor.css) styles do not apply, as the floating element is rendered outside the component. */
	.custom-tooltip {
		--bs-tooltip-bg: var(--bs-theme-bg);
		--bs-tooltip-color: var(--bs-theme-contrast);
	}
</style>

<HxTooltip Text="This tooltip is themed via CSS variables." CssClass="custom-tooltip theme-primary">
	<HxButton Color="ThemeColor.Secondary" Text="Primary tooltip" />
</HxTooltip>
<HxTooltip Text="This tooltip is themed via CSS variables." CssClass="custom-tooltip theme-success">
	<HxButton Color="ThemeColor.Secondary" Text="Success tooltip" />
</HxTooltip>
<HxTooltip Text="This tooltip is themed via CSS variables." CssClass="custom-tooltip theme-danger">
	<HxButton Color="ThemeColor.Secondary" Text="Danger tooltip" />
</HxTooltip>

```
