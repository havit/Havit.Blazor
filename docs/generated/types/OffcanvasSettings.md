# OffcanvasSettings

Settings for the `HxOffcanvas` component.

## Properties

| Name | Type | Description |
|------|------|-------------|
| Backdrop | `OffcanvasBackdrop?` | Indicates whether to apply a backdrop on the body while the offcanvas is open. |
| BodyCssClass | `string` | Additional CSS class for the body. |
| CloseButtonIcon | `IconBase` | Close icon to be used in the header. If set to `null`, the Bootstrap default close button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the offcanvas closes when the escape key is pressed. |
| CssClass | `string` | Additional CSS class for the offcanvas. Added to the root div (`.offcanvas`). |
| FooterCssClass | `string` | Additional CSS class for the footer. |
| HeaderCssClass | `string` | Additional CSS class for the header. |
| Placement | `OffcanvasPlacement?` | The placement of the offcanvas. |
| ResponsiveBreakpoint | `OffcanvasResponsiveBreakpoint?` | The breakpoint below which the contents are rendered outside the viewport in an offcanvas (above this breakpoint, the offcanvas body is rendered inside the viewport). |
| ScrollingEnabled | `bool?` | Indicates whether body (page) scrolling is allowed while the offcanvas is open. |
| ShowCloseButton | `bool?` | Indicates whether the modal shows a close button in the header. Use the `CloseButtonIcon` property to change the shape of the button. |
| Size | `OffcanvasSize?` | The size of the offcanvas. |

