# HxGrid

Grid to display tabular data from data source. Includes support for client-side and server-side paging & sorting (or virtualized scrolling as needed).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Columns **[REQUIRED]** | `RenderFragment` | Grid columns. |
| DataProvider **[REQUIRED]** | `GridDataProviderDelegate<TItem>` | Data provider delegate for the grid. The data provider is responsible for fetching items to be rendered in the grid. It must always return an instance of `GridDataProviderResult` and cannot return null. |
| ContentNavigationMode | `GridContentNavigationMode?` | The strategy for how data items are displayed and loaded into the grid. Supported modes include pagination, load more, and infinite scroll. |
| CurrentUserState | `GridUserState` | Gets or sets the current state of the grid, including pagination and sorting information. This state can be used to restore the grid to a specific configuration or to synchronize it with external state management systems. |
| CurrentUserStateChanged | `EventCallbackGridUserState>` | Event that fires when the `CurrentUserState` property changes. This event can be used to react to changes in the grid's state, such as sorting or pagination adjustments. |
| EmptyDataTemplate | `RenderFragment` | Template for rendering when the data source is empty but not null. |
| FooterRowAdditionalAttributes | `Dictionary<string, object>` | Provides a dictionary of additional attributes to apply to the footer `tr` element of the grid. This allows for custom styling or behavior of the footer row. |
| FooterRowCssClass | `string` | A custom CSS class for the footer `tr` element in the grid. This allows styling of the grid footer independently of other grid elements. |
| HeaderRowAdditionalAttributes | `Dictionary<string, object>` | Provides a dictionary of additional attributes to apply to the header `tr` element of the grid. This allows for custom styling or behavior of the header row. |
| HeaderRowCssClass | `string` | Custom CSS class for the header `tr` element in the grid. Enables specific styling for the header row separate from the rest of the grid. |
| Hover | `bool?` | Enables or disables the hover state on table rows within a `<tbody>`. When not set, the table is hoverable by default if selection is enabled. This property customizes the hover behavior of the grid rows. |
| InProgress | `bool?` | Gets or sets a value indicating whether the grid is currently processing data, such as loading or refreshing items. When set to `null`, the progress state is automatically managed based on the data provider's activity. |
| ItemKeySelector | `Func<TItem, object>` | Optionally defines a value for @key on each rendered row. Typically this should be used to specify a unique identifier, such as a primary key value, for each data item. This allows the grid to preserve the association between row elements and data items based on their unique identifiers, even when the TGridItem instances are replaced by new copies (for example, after a new query against the underlying data store). If not set, the @key will be the TItem instance itself. |
| ItemRowAdditionalAttributes | `Dictionary<string, object>` | Provides a dictionary of additional attributes to apply to all body `tr` elements in the grid. These attributes can be used to customize the appearance or behavior of rows. |
| ItemRowAdditionalAttributesSelector | `Func<TItem, Dictionary<string, object>>` | Defines a function that returns additional attributes for a specific `tr` element based on the item it represents. This allows for custom behavior or event handling on a per-row basis. |
| ItemRowCssClass | `string` | Custom CSS class for the data `tr` elements in the grid. This class is applied to each row of data, providing a way to customize the styling of data rows. |
| ItemRowCssClassSelector | `Func<TItem, string>` | Function that defines a custom CSS class for each data `tr` element based on the item it represents. This allows for conditional styling of rows based on their data. |
| ItemRowHeight | `float?` | Height of each item row, used in calculations for infinite scrolling (`GridContentNavigationMode.InfiniteScroll`). The default value (41px) corresponds to the typical row height in the Bootstrap 5 default theme. The row height is not applied for other navigation modes, use CSS for that. |
| LoadingDataTemplate | `RenderFragment` | Defines a template for the initial data loading phase. This template is not used when loading data for sorting or paging operations. |
| LoadMoreButtonSettings | `ButtonSettings` | Configuration for the "Load more" button, including appearance and behavior settings. Relevant in grid modes that use a "Load more" button for data navigation. |
| LoadMoreButtonText | `string` | The text for the "Load more" button, used in the `GridContentNavigationMode.LoadMore` navigation mode. The default text is obtained from localization resources. |
| LoadMoreTemplate | `RenderFragmentGridLoadMoreTemplateContext>` | Template for the "load more" button (or other UI element). |
| MultiSelectionEnabled | `bool` | Enables or disables multi-item selection using checkboxes in the first column. Can be used with single selection. Defaults to `false`. |
| OverscanCount | `int?` | Defines the number of additional items to be rendered before and after the visible region in an infinite scrolling scenario. This helps to reduce the frequency of rendering during scrolling, though higher values increase the number of elements present in the page. Default is 3. |
| PagerSettings | `PagerSettings` | Pager settings. |
| PageSize | `int?` | The number of items to display per page. Applicable for grid modes such as pagination and load more. Set to 0 to disable paging. |
| PaginationTemplate | `RenderFragmentGridPaginationTemplateContext>` | Template for rendering custom pagination. |
| PlaceholdersRowCount | `int?` | The number of placeholder rows to be rendered in the grid. Placeholders are used when loading data or when `LoadingDataTemplate` is not set. Set to 0 to disable placeholders. Default value is 5. |
| PreserveSelection | `bool?` | Gets or sets a value indicating whether the current selection (either `SelectedDataItem` for single selection or `SelectedDataItems` for multiple selection) should be preserved during data operations, such as paging, sorting, filtering, or manual invocation of `HxGrid.RefreshDataAsync`. Default value is `false` (can be set by using `HxGrid.Defaults`). |
| ProgressIndicatorDelay | `int?` | Delay in milliseconds before the progress indicator is displayed. The default value is `300 ms`. |
| Responsive | `bool?` | Determines if the grid should be scrollable horizontally across different breakpoints. When set to true, the `table-responsive` class is added to the table. Default is false. |
| SelectedDataItem | `TItem` | Represents the currently selected data item in the grid for data binding and state synchronization. Changes trigger `SelectedDataItemChanged`. |
| SelectedDataItemChanged | `EventCallback<TItem>` | Event that fires when the `SelectedDataItem` property changes. This event is intended for data binding and state synchronization. |
| SelectedDataItems | `HashSet<TItem>` | Represents the collection of currently selected data items in the grid, primarily for data binding and state management in multi-selection scenarios. |
| SelectedDataItemsChanged | `EventCallback<HashSet<TItem>>` | Event that fires when the collection of selected data items changes. This is particularly relevant in multi-selection scenarios. It is intended for data binding and state synchronization. |
| SelectionEnabled | `bool` | Enables or disables single item selection by row click. Can be used alongside multi-selection. Defaults to `true`. |
| Settings | `GridSettings` | Specifies the grid settings. Overrides default settings in `Defaults` and can be further overridden by individual parameters. |
| ShowFooterWhenEmptyData | `bool?` | Determines whether the grid footer is rendered when the grid's data source is empty. The default value is `false`. |
| SortAscendingIcon | `IconBase` | Icon to indicate the ascending sort direction in the column header. This icon is displayed when a column is sorted in ascending order. |
| SortDescendingIcon | `IconBase` | Icon to indicate the descending sort direction in the column header. This icon is shown when a column is sorted in descending order. |
| Striped | `bool?` | Adds zebra-striping to any table row within the `<tbody>` for better readability. Rows will have alternating background colors. Default is false. |
| TableContainerCssClass | `string` | Custom CSS class for the `div` element that wraps the main `table` element. Excludes the `HxPager` which is not wrapped in this `div` element. |
| TableCssClass | `string` | Custom CSS class for the main `table` element of the grid. This class allows for styling and customization of the grid's appearance. |
| TableHeaderCssClass | `string` | Custom CSS class for the `thead` element of the grid. This class allows for styling and customization of the grid's appearance. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| PagerCurrentPageIndexChanged(int newPageIndex) | `Task` |  |
| RefreshDataAsync() | `Task` | Requests a data refresh from the `HxGrid.DataProvider`. Useful for updating the grid when external data may have changed. To reset grid state (e.g., position), use `HxGrid.RefreshDataAsync` instead. |
| RefreshDataAsync(GridStateResetOptions resetOptions) | `Task` | Requests a data refresh from the `HxGrid.DataProvider`. Useful for updating the grid when external data may have changed. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `GridSettings` | Application-wide defaults for the `HxGrid` and derived components. |

## Available demo samples

- HxGrid_Demo.razor
- HxGrid_Demo_ApplyTo.razor
- HxGrid_Demo_ApplyTo_Async.razor
- HxGrid_Demo_ContextMenu.razor
- HxGrid_Demo_CustomPagination.razor
- HxGrid_Demo_EmptyData.razor
- HxGrid_Demo_HeaderFiltering.razor
- HxGrid_Demo_Hover.razor
- HxGrid_Demo_InfiniteScroll.razor
- HxGrid_Demo_InlineEditing.razor
- HxGrid_Demo_LoadMore.razor
- HxGrid_Demo_Multiselect.razor
- HxGrid_Demo_Queryable.razor
- HxGrid_Demo_RefreshData.razor
- HxGrid_Demo_StatePersisting.razor
- HxGrid_Demo_Striped.razor

