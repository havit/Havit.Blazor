# HxTreeView

Component to display a hierarchy data structure.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Items **[REQUIRED]** | `IEnumerable<TItem>` | Collection of hierarchical data to display. |
| CssClass | `string` | Additional CSS class to be applied. |
| ItemCssClass | `string` | Item CSS class (same for all items). |
| ItemCssClassSelector | `Func<TItem, string>` | Selector for the item CSS class. |
| ItemChildrenSelector | `Func<TItem, IEnumerable<TItem>>` | Selector to display the children collection for the current data item. The children collection should have the same type as the current item. |
| ItemIconSelector | `Func<TItem, IconBase>` | Selector to display the icon from the data item. |
| ItemInitialExpandedSelector | `Func<TItem, bool>` | Selector for the initial expansion state of the data item. The default state is `false` (collapsed). |
| ItemTemplate | `RenderFragment<TItem>` | Template for the item content. |
| ItemTitleSelector | `Func<TItem, string>` | Selector to display the item title from the data item. |
| SelectedItem | `TItem` | Selected data item. |
| SelectedItemChanged | `EventCallback<TItem>` | Event fired when the selected data item changes. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnItemCollapsed | `EventCallback<TItem>` | Event fired when an item is collapsed. |
| OnItemExpanded | `EventCallback<TItem>` | Event fired when an item is expanded. |

## Available demo samples

- HxTreeView_Demo_BasicUsage.razor
- HxTreeView_Demo_CustomContent.razor

