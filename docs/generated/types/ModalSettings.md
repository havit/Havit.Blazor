# ModalSettings

Settings for the `HxModal` component

## Properties

| Name | Type | Description |
|------|------|-------------|
| Animated | `bool?` | For modals that simply appear rather than fade in to view, setting `false` removes the `.fade` class from your modal markup. The default value is `true`. |
| Backdrop | `ModalBackdrop?` | Indicates whether to apply a backdrop to the body while the modal is open. If set to `ModalBackdrop.Static`, the modal cannot be closed by clicking on the backdrop. The default value (from `HxModal.Defaults`) is `ModalBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class (`div.modal-body`). |
| Centered | `bool?` | Allows vertical centering of the modal. |
| CloseButtonIcon | `IconBase` | The close icon to be used in the header. |
| CloseOnEscape | `bool?` | Indicates whether the modal closes when the escape key is pressed. |
| ContentCssClass | `string` | Additional content CSS class (`div.modal-content`). |
| CssClass | `string` | Additional CSS class for the main element (`div.modal`). |
| DialogCssClass | `string` | Additional CSS class for the dialog (`div.modal-dialog` element). |
| FooterCssClass | `string` | Additional footer CSS class (`div.modal-footer`). |
| Fullscreen | `ModalFullscreen?` | The fullscreen behavior of the modal. |
| HeaderCssClass | `string` | Additional header CSS class (`div.modal-header`). |
| Scrollable | `bool?` | Allows scrolling of the modal body. |
| ShowCloseButton | `bool?` | Indicates whether the modal shows a close button in the header. |
| Size | `ModalSize?` | The size of the modal. |

