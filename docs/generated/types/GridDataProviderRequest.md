# GridDataProviderRequest

Data provider request for grid data.

## Properties

| Name | Type | Description |
|------|------|-------------|
| CancellationToken | `CancellationToken` | The `CancellationToken` used to relay cancellation of the request. |
| Count | `int?` | The number of records to return. In paging mode, it equals the size of the page. |
| Sorting | `IReadOnlyList<SortingItem<TItem>>` | Current sorting. |
| StartIndex | `int` | The number of records to skip. In paging mode, it equals the page size multiplied by the requested page index. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| ApplyTo(IEnumerable<TItem> data) | `GridDataProviderResult<TItem>` | Processes data on the client side (sorting & paging) and returns the result for the grid. |

