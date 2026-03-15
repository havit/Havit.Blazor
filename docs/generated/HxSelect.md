# HxSelect

Select - DropDownList - single-item picker. Consider creating a custom picker derived from `HxSelectBase`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| AutoSort | `bool` | When `true`, the items are sorted before displaying in the select. The default value is `true`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| Data | `IEnumerable<TItem>` | The items to display. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupEndTemplate | `RenderFragment` | Input group at the end of the input. |
| InputGroupEndText | `string` | Input group at the end of the input. |
| InputGroupStartTemplate | `RenderFragment` | Input group at the beginning of the input. |
| InputGroupStartText | `string` | Input group at the beginning of the input. |
| InputSize | `InputSize?` | Size of the input. |
| ItemDisabledSelector | `Func<TItem, bool>` | When set, determines whether an item is disabled (non-selectable and greyed out). When returns `true`, the corresponding option will be rendered with `disabled` attribute. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| Nullable | `bool?` | Indicates whether `null` is a valid value. |
| NullDataText | `string` | The text to display when `Data` is `null`. |
| NullText | `string` | The text to display for the `null` value. |
| Settings | `SelectSettings` | Set of settings to be applied to the component instance (overrides , overridden by individual parameters). |
| SortKeySelector | `Func<TItem, IComparable>` | Selects the value to sort items. Uses the `TextSelector` property when not set. When complex sorting is required, sort the data manually and don't let this component sort them. Alternatively, create a custom comparable property. |
| TextSelector | `Func<TItem, string>` | Selects the text to display from the item. When not set, `ToString()` is used. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |
| ValueSelector | `Func<TItem, TValue>` | Selects the value from the item. Not required when `TValueType` is the same as `TItemTime`. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the component. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `SelectSettings` | Application-wide defaults for the `HxSelect` (`HxSelectBase` and derived components, respectively). |

## Available demo samples

- HxSelect_Demo.razor
- HxSelect_Demo_InputGroups.razor
- HxSelect_Demo_ItemDisabledSelector.razor

