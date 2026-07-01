# DrawerRenderMode

Drawer render mode.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| OpenOnly | 0 | Drawer content is rendered only when it is open. Suitable for item-detail, item-edit, etc. This setting applies only when `DrawerResponsiveBreakpoint.None` is set. For all other values, the content is always rendered (to be available for the mobile version). |
| Always | 1 | Drawer content is always rendered (and hidden with CSS if not open). Needed for HxFilterForm with HxChipList. |

