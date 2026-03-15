# HxMultiSelect

MultiSelect. Unlike a normal select, multiselect allows the user to select multiple options at once.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| AllowFiltering | `bool?` | Enables filtering capabilities. |
| AllowSelectAll | `bool?` | Enables select all capabilities. |
| AutoSort | `bool` | When set to `false`, items will no longer be sorted. Default value is `true`. |
| ClearFilterOnHide | `bool?` | When enabled the filter will be cleared when the dropdown is closed. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| Data | `IEnumerable<TItem>` | Items to display. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| EmptyText | `string` | Text to display when the selection is empty (the `Value` property is `null` or empty). |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| FilterClearIcon | `IconBase` | Icon displayed in filter input for clearing the filter. |
| FilterEmptyResultTemplate | `RenderFragment` | Template that defines what should be rendered in case of empty items. |
| FilterEmptyResultText | `string` | Text to display when the filtered results list is empty and when not using `FilterEmptyResultTemplate`. |
| FilterPredicate | `Func<TItem, string, bool>` | Defines a custom filtering predicate to apply to the list of items. If not specified, the default behavior filters items based on whether the item text (obtained via TextSelector) contains the filter query string. |
| FilterSearchIcon | `IconBase` | Icon displayed in filter input for searching the filter. |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupEndTemplate | `RenderFragment` | Input-group at the end of the input. |
| InputGroupEndText | `string` | Input-group at the end of the input. |
| InputGroupStartTemplate | `RenderFragment` | Input-group at the beginning of the input. |
| InputGroupStartText | `string` | Input-group at the beginning of the input. |
| InputSize | `InputSize?` | Size of the input. |
| InputText | `string` | Text to display in the input (default is a list of selected values). |
| InputTextSelector | `Func<IEnumerable<TItem>, string>` | Function to build the text to be displayed in the input from selected items (default is a list of selected values). |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| NullDataText | `string` | Text to display when `Data` is `null`. |
| SelectAllText | `string` | Text to display for the select all dropdown option. |
| Settings | `MultiSelectSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| SortKeySelector | `Func<TItem, IComparable>` | Selects value for item sorting. When not set, `TextSelector` property will be used. If you need complex sorting, pre-sort data manually or create a custom comparable property. |
| TextSelector | `Func<TItem, string>` | Selects text to display from an item. When not set, `ToString()` is used. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `List<TValue>` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<List<TValue>>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<List<TValue>>` | A callback that updates the bound value. |
| ValueSelector | `Func<TItem, TValue>` | Selects value from an item. Not required when `TValue` is the same as `TItem`. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the multi select component. |
| HideDropdownAsync() | `Task` | Hides the dropdown. |
| ShowDropdownAsync() | `Task` | Shows the dropdown. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `MultiSelectSettings` | Application-wide defaults for the `HxMultiSelect`. |

## Available demo samples

- HxMultiSelect_Demo_BasicUsage.razor
- HxMultiSelect_Demo_CustomFiltering.razor
- HxMultiSelect_Demo_Filtering.razor
- HxMultiSelect_Demo_SelectAll.razor

