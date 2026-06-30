# HxDrawer

Bootstrap Drawer component (aka Drawer).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Backdrop | `DrawerBackdrop?` | Indicates whether to apply a backdrop on the body while the drawer is open. If set to `DrawerBackdrop.Static`, the drawer cannot be closed by clicking on the backdrop. The default value (from `Defaults`) is `DrawerBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class. |
| BodyTemplate | `RenderFragment` | Body content. |
| CloseButtonIcon | `IconBase` | The close icon to be used in the header. If set to `null`, the Bootstrap default close button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the drawer closes when the escape key is pressed. The default value is `true`. |
| CssClass | `string` | Drawer additional CSS class. Added to root `div` (`.drawer`). |
| FooterCssClass | `string` | Additional footer CSS class. |
| FooterTemplate | `RenderFragment` | Footer content. |
| HeaderCssClass | `string` | Additional header CSS class. |
| HeaderTemplate | `RenderFragment` | Content for the header. (Is rendered directly into `drawer-header` - in contrast to `Title` which is rendered into `<h5 class="drawer-title">`). |
| Placement | `DrawerPlacement?` | Placement of the drawer. The default is `DrawerPlacement.End` (right). |
| RenderMode | `DrawerRenderMode` | Determines whether the content is always rendered or only if the drawer is open. The default is `DrawerRenderMode.OpenOnly`. Please note, this setting applies only when `DrawerResponsiveBreakpoint.None` is set. For all other values, the content is always rendered (to be available for the mobile version). |
| ResponsiveBreakpoint | `DrawerResponsiveBreakpoint?` | Breakpoint below which the contents are rendered outside the viewport in an drawer (above this breakpoint, the drawer body is rendered inside the viewport). |
| ScrollingEnabled | `bool?` | Indicates whether body (page) scrolling is allowed while the drawer is open. The default value (from `Defaults`) is `false`. |
| Settings | `DrawerSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Sheet | `bool?` | When `true`, renders the drawer as a flush-to-edge sheet (no inset, border radius, or shadow) using the `drawer-sheet` variant (new in Bootstrap 6). Default is `false`. |
| ShowCloseButton | `bool?` | Indicates whether the drawer shows a close button in the header. The default value is `true`. Use `CloseButtonIcon` to change the shape of the button. |
| Size | `DrawerSize?` | Size of the drawer. The default is `DrawerSize.Regular`. |
| Title | `string` | Text for the title in the header. (Is rendered into `<h5 class="drawer-title">` - in contrast to `HeaderTemplate` which is rendered directly into `drawer-header`). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallback` | This event is fired when an drawer element has been hidden from the user (will wait for CSS transitions to complete). |
| OnHiding | `EventCallbackDrawerHidingEventArgs>` | Fired immediately when the 'hide' instance method is called. To cancel hiding, set `DrawerHidingEventArgs.Cancel` to `true`. |
| OnShown | `EventCallback` | This event is fired when an drawer element has been made visible to the user (will wait for CSS transitions to complete). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| HideAsync() | `Task` | Hides the drawer (if opened). |
| ShowAsync() | `Task` | Shows the drawer. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `DrawerSettings` | Application-wide defaults for the `HxDrawer` and derived components. |

## Available demo samples

- HxDrawer_Demo.razor
- HxDrawer_Demo_Backdrop.razor
- HxDrawer_Demo_CustomCloseButtonIcon.razor
- HxDrawer_Demo_Footer.razor
- HxDrawer_Demo_Placement.razor
- HxDrawer_Demo_Responsive.razor
- HxDrawer_Demo_Sheet.razor
- HxDrawer_Demo_Size.razor
- HxDrawer_Demo_StaticBackdrop.razor

