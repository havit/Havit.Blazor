# GridSettings

Settings for the `HxGrid` and derived components.

## Properties

| Name | Type | Description |
|------|------|-------------|
| ContentNavigationMode | `GridContentNavigationMode?` | Strategy for displaying and loading data in the grid. |
| FooterRowCssClass | `string` | Custom CSS class to render with the footer `tr` element. |
| HeaderRowCssClass | `string` | Custom CSS class to render with the header `tr` element. |
| Hover | `bool?` | Enables the hover state on table rows within a `<tbody>` (sets the `table-hover` class on the table). |
| ItemRowCssClass | `string` | Custom CSS class to render with the data `tr` element. |
| ItemRowHeight | `float?` | Height of the item row (in pixels) used for infinite scroll calculations (`GridContentNavigationMode.InfiniteScroll`). The row height is not applied for other navigation modes, use CSS for that. |
| LoadMoreButtonSettings | `ButtonSettings` | Settings for the "Load more" navigation button (`GridContentNavigationMode.LoadMore` or `GridContentNavigationMode.PaginationAndLoadMore`). |
| OverscanCount | `int?` | Infinite scroll (`GridContentNavigationMode.InfiniteScroll`): Gets or sets a value that determines how many additional items will be rendered before and after the visible region. This helps to reduce the frequency of rendering during scrolling. However, higher values mean that more elements will be present on the page. |
| PagerSettings | `PagerSettings` | Pager settings. |
| PageSize | `int?` | Page size for `GridContentNavigationMode.Pagination`, `GridContentNavigationMode.LoadMore` and `GridContentNavigationMode.PaginationAndLoadMore`. Set `0` to disable paging. |
| PlaceholdersRowCount | `int?` | Number of rows with placeholders to render. |
| PreserveSelection | `bool?` | Gets or sets a value indicating whether the current selection (either `HxGrid.SelectedDataItem` for single selection or `HxGrid.SelectedDataItems` for multiple selection) should be preserved during data operations, such as paging, sorting, filtering, |
| ProgressIndicatorDelay | `int?` | Delay in milliseconds before the progress indicator is displayed. |
| Responsive | `bool?` | Allows the table to be scrolled horizontally with ease across any breakpoint (adds the `table-responsive` class to the table). |
| ShowFooterWhenEmptyData | `bool?` | Indicates whether to render the footer when data is empty. |
| SortAscendingIcon | `IconBase` | Icon to indicate ascending sort direction in column header. |
| SortDescendingIcon | `IconBase` | Icon to indicate descending sort direction in column header. |
| Striped | `bool?` | Adds zebra-striping to any table row within the `<tbody>` (alternating rows). |
| TableContainerCssClass | `string` | Custom CSS class to render with the `div` element wrapping the main `table` (`HxPager` is not wrapped in this `div` element). |
| TableCssClass | `string` | Custom CSS class to render with the main `table` element. |
| TableHeaderCssClass | `string` | Custom CSS class for the `thead` element of the grid. |

