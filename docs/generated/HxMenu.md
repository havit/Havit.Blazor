# HxMenu

Bootstrap 6 Menu component (replaces the Bootstrap 5 Dropdown). Renders the `HxMenu.Toggle` and the `.menu` element (with `HxMenu.Content`) as direct siblings — Bootstrap 6 requires no wrapper element (sibling position is also used by the CSS, e.g. for split-button border radius via `:has(+ .menu)`).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the underlying `.menu` element. |
| AutoClose | `MenuAutoClose` | By default, the menu is closed when clicking inside or outside the menu (`MenuAutoClose.True`). You can use the AutoClose parameter to change this behavior of the menu. See https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior for more information. |
| Content | `RenderFragment` | The menu content (typically `HxMenuItem`, `HxMenuItemNavLink`, `HxMenuDivider`, `HxMenuHeader`, `HxMenuText`, or any custom content). |
| CssClass | `string` | Any additional CSS class to apply to the `.menu` element. |
| Placement | `MenuPlacement` | Placement of the menu relative to the toggle. The default is `MenuPlacement.BottomStart` (Bootstrap default). |
| ResponsivePlacement | `string` | Raw `data-bs-placement` value allowing responsive placements (e.g. `bottom-start md:bottom-end`). Takes precedence over `Placement`. See Bootstrap documentation. |
| Toggle | `RenderFragment` | The toggle which opens the menu (typically `HxMenuToggleButton` or `HxMenuToggleElement`). Rendered as a direct sibling of the `.menu` element. |

## Available demo samples

- HxMenu_Demo_AutoClose.razor
- HxMenu_Demo_Basic.razor
- HxMenu_Demo_CustomToggle.razor
- HxMenu_Demo_Dark.razor
- HxMenu_Demo_Descriptions.razor
- HxMenu_Demo_Dividers.razor
- HxMenu_Demo_Forms.razor
- HxMenu_Demo_HeaderDisabledActive.razor
- HxMenu_Demo_Icons.razor
- HxMenu_Demo_OffsetAndReference.razor
- HxMenu_Demo_Placement.razor
- HxMenu_Demo_Reference.razor
- HxMenu_Demo_Scrollable.razor
- HxMenu_Demo_Selected.razor
- HxMenu_Demo_SplitButton.razor
- HxMenu_Demo_Submenus.razor
- HxMenu_Demo_Text.razor
- HxMenu_Demo_Translucent.razor

