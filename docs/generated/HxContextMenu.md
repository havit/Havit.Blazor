# HxContextMenu

Ready-made context menu (based on Bootstrap Dropdown) with built-in support for confirmation messages after clicking on the menu items.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| ChildContent | `RenderFragment` | Items of the context menu. Use `HxContextMenuItem` components. |
| CssClass | `string` | Additional CSS class(es) for the context menu. |
| DropdownCssClass | `string` | Additional CSS class(es) for the context menu dropdown (container). |
| DropdownMenuAlignment | `DropdownMenuAlignment?` | Alignment for the context menu dropdown menu. The default is `DropdownMenuAlignment.End`. |
| DropdownMenuCssClass | `string` | Additional CSS class(es) for the context menu dropdown menu. |
| Icon | `IconBase` | Icon carrying the menu (use `BootstrapIcon` or any other `IconBase`). The default is `BootstrapIcon.ThreeDotsVertical`. |
| IconCssClass | `string` | Additional CSS class(es) for the context menu icon. |
| PopperStrategy | `PopperStrategy` | Popper positioning strategy. Default is `PopperStrategy.Absolute`. |
| Settings | `ContextMenuSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ContextMenuSettings` | Application-wide defaults for `HxContextMenu` and derived components. |

## Available demo samples

- HxContextMenu_Demo.razor
- HxContextMenu_Demo_WithIcons.razor

