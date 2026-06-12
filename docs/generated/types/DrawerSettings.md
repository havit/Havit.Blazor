# DrawerSettings

Settings for the `HxDrawer` component.

## Properties

| Name | Type | Description |
|------|------|-------------|
| Backdrop | `DrawerBackdrop?` | Indicates whether to apply a backdrop on the body while the drawer is open. |
| BodyCssClass | `string` | Additional CSS class for the body. |
| CloseButtonIcon | `IconBase` | Close icon to be used in the header. If set to `null`, the Bootstrap default close button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the drawer closes when the escape key is pressed. |
| CssClass | `string` | Additional CSS class for the drawer. Added to the root div (`.drawer`). |
| FooterCssClass | `string` | Additional CSS class for the footer. |
| HeaderCssClass | `string` | Additional CSS class for the header. |
| Placement | `DrawerPlacement?` | The placement of the drawer. |
| ResponsiveBreakpoint | `DrawerResponsiveBreakpoint?` | The breakpoint below which the contents are rendered outside the viewport in an drawer (above this breakpoint, the drawer body is rendered inside the viewport). |
| ScrollingEnabled | `bool?` | Indicates whether body (page) scrolling is allowed while the drawer is open. |
| Sheet | `bool?` | When `true`, renders the drawer as a flush-to-edge sheet (no inset, border radius, or shadow) using the `drawer-sheet` variant (new in Bootstrap 6). |
| ShowCloseButton | `bool?` | Indicates whether the drawer shows a close button in the header. Use the `CloseButtonIcon` property to change the shape of the button. |
| Size | `DrawerSize?` | The size of the drawer. |

