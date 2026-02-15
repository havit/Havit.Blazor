# HxRadioButtonList

Data-based list of radio buttons. Consider creating a custom picker derived from `HxRadioButtonListBase`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| AutoSort | `bool` | When `true`, items are sorted before displaying in the select. The default value is `true`. |
| Color | `ThemeColor?` | Color for `RadioButtonListRenderMode.ToggleButtons` and `RadioButtonListRenderMode.ButtonGroup`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| Data | `IEnumerable<TItem>` | Items to display. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| Inline | `bool` | Allows grouping radios on the same horizontal row by rendering them inline. Default is `false`. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| ItemCssClass | `string` | Additional CSS class(es) for underlying radio-buttons (wrapping `div` element). |
| ItemCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for underlying radio-buttons (wrapping `div` element). |
| ItemInputCssClass | `string` | Additional CSS class(es) for the `input` element of underlying radio-buttons. |
| ItemInputCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for the `input` element of underlying radio-button. |
| ItemSortKeySelector | `Func<TItem, IComparable>` | Selects the value to sort items. Uses the `ItemTextSelector` property when not set. When complex sorting is required, sort the data manually and don't let this component sort them. Alternatively, create a custom comparable property. |
| ItemTemplate | `RenderFragment<TItem>` | Gets the HTML to display from the item. When not set, `ItemTextSelector` is used. |
| ItemTextCssClass | `string` | Additional CSS class(es) for the text of the underlying radio-buttons. |
| ItemTextCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for the text of the underlying radio-buttons. |
| ItemTextSelector | `Func<TItem, string>` | Selects the text to display from the item. Also used for chip text. When not set, `ToString()` is used. |
| ItemValueSelector | `Func<TItem, TValue>` | Selects the value from the item. Not required when `TValue` is the same as `TItem`. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Outline | `bool?` | Indicates whether to use Bootstrap "outline" buttons. for `RadioButtonListRenderMode.ToggleButtons` and `RadioButtonListRenderMode.ButtonGroup`. |
| RenderMode | `RadioButtonListRenderMode` | Radio button list render mode. The default value is `RadioButtonListRenderMode.RadioButtons`. |
| Settings | `RadioButtonListSettings` | Set of settings to be applied to the component instance (overrides , overridden by individual parameters). |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `RadioButtonListSettings` | Application-wide defaults for `HxRadioButtonList` and derived components. |

## Available demo samples

- HxRadioButtonList_Demo.razor
- HxRadioButtonList_Demo_Inline.razor
- HxRadioButtonList_Demo_RenderModes.razor

