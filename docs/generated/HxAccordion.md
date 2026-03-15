# HxAccordion

Bootstrap accordion component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CssClass | `string` | Additional CSS classes for the accordion container. |
| ExpandedItemId | `string` | ID of the single expanded item. If `StayOpen` is `true`, this parameter is ignored, and you must use `ExpandedItemIds` instead. When `StayOpen` is `true`, the `ExpandedItemIdChanged` event callback will not be invoked. Do not use a constant value as it reverts the accordion to that item on every roundtrip. Use `InitialExpandedItemId` to set the initial state. |
| ExpandedItemIdChanged | `EventCallback<string>` |  |
| ExpandedItemIds | `List<string>` | IDs of the expanded items (if `StayOpen` is `true`, several items can be expanded). Do not use a constant value as it resets the accordion on every roundtrip. Use `InitialExpandedItemIds` to set the initial state. When `StayOpen` is `true`, this parameter must be used to manage expanded items, and the `ExpandedItemIdsChanged` event callback will be invoked for changes. |
| ExpandedItemIdsChanged | `EventCallback<List<string>>` |  |
| ChildContent | `RenderFragment` | Content of the component. |
| InitialExpandedItemId | `string` | ID of the item you want to expand at the very beginning (overwrites `ExpandedItemId` if set). |
| InitialExpandedItemIds | `List<string>` | IDs of the items you want to expand at the very beginning (overwrites `ExpandedItemIds` if set). |
| StayOpen | `bool` | Set to `true` to make accordion items stay open when another item is opened. The default is `false`, opening another item collapses the current item. When `true`, `ExpandedItemIds` must be used to manage expanded items. |

## Available demo samples

- HxAccordion_ComplexDemo.razor
- HxAccordion_Demo_Flush.razor
- HxAccordion_Demo_StayOpen.razor
- HxAccordion_PlainDemo.razor

