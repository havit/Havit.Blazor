# HxOffcanvas

Bootstrap Offcanvas component (aka Drawer).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Backdrop | `OffcanvasBackdrop?` | Indicates whether to apply a backdrop on the body while the offcanvas is open. If set to `OffcanvasBackdrop.Static`, the offcanvas cannot be closed by clicking on the backdrop. The default value (from `Defaults`) is `OffcanvasBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class. |
| BodyTemplate | `RenderFragment` | Body content. |
| CloseButtonIcon | `IconBase` | The close icon to be used in the header. If set to `null`, the Bootstrap default close button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the offcanvas closes when the escape key is pressed. The default value is `true`. |
| CssClass | `string` | Offcanvas additional CSS class. Added to root `div` (`.offcanvas`). |
| FooterCssClass | `string` | Additional footer CSS class. |
| FooterTemplate | `RenderFragment` | Footer content. |
| HeaderCssClass | `string` | Additional header CSS class. |
| HeaderTemplate | `RenderFragment` | Content for the header. (Is rendered directly into `offcanvas-header` - in contrast to `Title` which is rendered into `<h5 class="offcanvas-title">`). |
| Placement | `OffcanvasPlacement?` | Placement of the offcanvas. The default is `OffcanvasPlacement.End` (right). |
| RenderMode | `OffcanvasRenderMode` | Determines whether the content is always rendered or only if the offcanvas is open. The default is `OffcanvasRenderMode.OpenOnly`. Please note, this setting applies only when `OffcanvasResponsiveBreakpoint.None` is set. For all other values, the content is always rendered (to be available for the mobile version). |
| ResponsiveBreakpoint | `OffcanvasResponsiveBreakpoint?` | Breakpoint below which the contents are rendered outside the viewport in an offcanvas (above this breakpoint, the offcanvas body is rendered inside the viewport). |
| ScrollingEnabled | `bool?` | Indicates whether body (page) scrolling is allowed while the offcanvas is open. The default value (from `Defaults`) is `false`. |
| Settings | `OffcanvasSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowCloseButton | `bool?` | Indicates whether the modal shows a close button in the header. The default value is `true`. Use `CloseButtonIcon` to change the shape of the button. |
| Size | `OffcanvasSize?` | Size of the offcanvas. The default is `OffcanvasSize.Regular`. |
| Title | `string` | Text for the title in the header. (Is rendered into `<h5 class="offcanvas-title">` - in contrast to `HeaderTemplate` which is rendered directly into `offcanvas-header`). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallback` | This event is fired when an offcanvas element has been hidden from the user (will wait for CSS transitions to complete). |
| OnHiding | `EventCallbackOffcanvasHidingEventArgs>` | Fired immediately when the 'hide' instance method is called. To cancel hiding, set `OffcanvasHidingEventArgs.Cancel` to `true`. |
| OnShown | `EventCallback` | This event is fired when an offcanvas element has been made visible to the user (will wait for CSS transitions to complete). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| HideAsync() | `Task` | Hides the offcanvas (if opened). |
| ShowAsync() | `Task` | Shows the offcanvas. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `OffcanvasSettings` | Application-wide defaults for the `HxOffcanvas` and derived components. |

## Available demo samples

- HxOffcanvas_Demo.razor
- HxOffcanvas_Demo_Backdrop.razor
- HxOffcanvas_Demo_CustomCloseButtonIcon.razor
- HxOffcanvas_Demo_Placement.razor
- HxOffcanvas_Demo_Responsive.razor
- HxOffcanvas_Demo_Size.razor
- HxOffcanvas_Demo_StaticBackdrop.razor

