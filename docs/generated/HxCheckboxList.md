# HxCheckboxList

Renders a multi-selection list of `HxCheckbox` controls.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| AutoSort | `bool` | When `true`, items are sorted before displaying in the select. The default value is `true`. |
| Color | `ThemeColor?` | Color for `CheckboxListRenderMode.ToggleButtons`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| Data | `IEnumerable<TItem>` | Items to display. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| Inline | `bool` | Allows grouping checkboxes on the same horizontal row by rendering them inline. The default is `false`. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| ItemCssClass | `string` | Additional CSS class(es) for the underlying `HxCheckbox`. |
| ItemCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for the `HxCheckbox`. |
| ItemInputCssClass | `string` | Additional CSS class(es) for the input element of the `HxCheckbox`. |
| ItemInputCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for the input element of the `HxCheckbox`. |
| ItemSortKeySelector | `Func<TItem, IComparable>` | Selects the value for item sorting. When not set, the `ItemTextSelector` property will be used. If you need complex sorting, pre-sort the data manually or create a custom comparable property. |
| ItemTextCssClass | `string` | Additional CSS class(es) for the text of the `HxCheckbox`. |
| ItemTextCssClassSelector | `Func<TItem, string>` | Additional CSS class(es) for the text of the `HxCheckbox`. |
| ItemTextSelector | `Func<TItem, string>` | Selects the text to display from the item. When not set, `ToString()` is used. |
| ItemValueSelector | `Func<TItem, TValue>` | Selects the value from the item. Not required when TValue is the same as TItem. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Outline | `bool?` | Indicates whether to use Bootstrap "outline" buttons. for `CheckboxListRenderMode.ToggleButtons` and `CheckboxListRenderMode.ButtonGroup`. |
| RenderMode | `CheckboxListRenderMode` | Checkbox render mode. |
| Settings | `CheckboxListSettings` | Set of settings to be applied to the component instance (overrides `HxInputDate.Defaults`, overridden by individual parameters). |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `List<TValue>` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<List<TValue>>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<List<TValue>>` | A callback that updates the bound value. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `CheckboxListSettings` | Application-wide defaults for `HxCheckboxList` and derived components. |

## Available demo samples

- HxCheckboxList_Demo.razor
- HxCheckboxList_Demo_Inline.razor
- HxCheckboxList_Demo_RenderModes.razor

