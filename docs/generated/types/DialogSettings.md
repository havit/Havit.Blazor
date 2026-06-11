# DialogSettings

Settings for the `HxDialog` component

## Properties

| Name | Type | Description |
|------|------|-------------|
| Animation | `DialogAnimation?` | Open/close animation of the dialog. |
| Backdrop | `DialogBackdrop?` | Indicates whether to apply a backdrop to the body while the dialog is open. If set to `DialogBackdrop.Static`, the dialog cannot be closed by clicking on the backdrop. The default value (from `HxDialog.Defaults`) is `DialogBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class (`div.dialog-body`). |
| CloseButtonIcon | `IconBase` | The close icon to be used in the header. |
| CloseOnEscape | `bool?` | Indicates whether the dialog closes when the escape key is pressed. |
| CssClass | `string` | Additional CSS class for the main `<dialog>` element. |
| FooterCssClass | `string` | Additional footer CSS class (`div.dialog-footer`). |
| Fullscreen | `DialogFullscreen?` | The fullscreen behavior of the dialog. |
| HeaderCssClass | `string` | Additional header CSS class (`div.dialog-header`). |
| NonModal | `bool?` | When `true`, the dialog opens as non-modal (no backdrop, no focus trap, page stays interactive). |
| Scrollable | `bool?` | Allows scrolling of the dialog body. |
| ShowCloseButton | `bool?` | Indicates whether the dialog shows a close button in the header. |
| Size | `DialogSize?` | The size of the dialog. |

