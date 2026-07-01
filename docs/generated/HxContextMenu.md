# HxContextMenu

Ready-made context menu (based on Bootstrap Menu) with built-in support for confirmation messages after clicking on the menu items.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| ChildContent | `RenderFragment` | Items of the context menu. Use `HxContextMenuItem` components. |
| CssClass | `string` | Additional CSS class(es) for the context menu. |
| FloatingStrategy | `FloatingStrategy` | Floating UI positioning strategy. Default is `FloatingStrategy.Absolute`. |
| Icon | `IconBase` | Icon carrying the menu (use `BootstrapIcon` or any other `IconBase`). The default is `BootstrapIcon.ThreeDotsVertical`. |
| IconCssClass | `string` | Additional CSS class(es) for the context menu icon. |
| MenuCssClass | `string` | Additional CSS class(es) for the context menu `.menu` element. |
| MenuPlacement | `MenuPlacement?` | Placement of the context menu. The default is `MenuPlacement.BottomEnd`. |
| Settings | `ContextMenuSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ContextMenuSettings` | Application-wide defaults for `HxContextMenu` and derived components. |

## Available demo samples

- HxContextMenu_Demo.razor
- HxContextMenu_Demo_WithIcons.razor

