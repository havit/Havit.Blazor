# DrawerBackdrop

Options for controlling the behavior of the `HxDrawer.Backdrop`.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| True | 0 | A backdrop will be rendered. The drawer will be closed upon clicking on the backdrop. |
| False | 1 | No (visible) backdrop will be rendered and clicking outside the drawer does not close it. To let users interact with other parts of the app while the drawer is open, set `HxDrawer.ScrollingEnabled` to `true` (the drawer opens as non-modal); otherwise the drawer is modal and the rest of the page stays inert. |
| Static | 2 | A static backdrop will be rendered. The drawer will not be closed upon clicking on the backdrop. |

