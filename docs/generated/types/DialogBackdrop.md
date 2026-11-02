# DialogBackdrop

Options for controlling the behavior of the `HxDialog.Backdrop`.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| True | 0 | A backdrop will be rendered. The dialog will be closed upon clicking on the backdrop. |
| False | 1 | No (visible) backdrop will be rendered and clicking outside the dialog does not close it. The dialog is still modal, the rest of the page stays inert. Use `HxDialog.NonModal` to let users interact with the page. |
| Static | 2 | A static backdrop will be rendered. The dialog will not be closed upon clicking on the backdrop. |

