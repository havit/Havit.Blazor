# HxPopover_Demo_Custom.razor

```razor
<style>
	/* Demonstration purposes only. Put this CSS into your application's global CSS file. Isolated (.razor.css) styles do not apply, as the floating element is rendered outside the component. */
	.custom-popover {
		--bs-popover-max-width: 200px;
		--bs-popover-border-color: var(--bs-theme-bg);
		--bs-popover-header-bg: var(--bs-theme-bg);
		--bs-popover-header-color: var(--bs-theme-contrast);
		--bs-popover-body-padding-x: 1rem;
		--bs-popover-body-padding-y: .5rem;
	}
</style>

<HxPopover Title="Custom popover" Content="This popover is themed via CSS variables." Placement="PopoverPlacement.Right" CssClass="custom-popover theme-primary">
	<HxButton Color="ThemeColor.Secondary" Text="Primary popover" />
</HxPopover>
<HxPopover Title="Custom popover" Content="This popover is themed via CSS variables." Placement="PopoverPlacement.Right" CssClass="custom-popover theme-success">
	<HxButton Color="ThemeColor.Secondary" Text="Success popover" />
</HxPopover>

```
