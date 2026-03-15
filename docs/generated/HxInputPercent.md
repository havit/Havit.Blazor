# HxInputPercent

Numeric input in percentages with value normalization (0% = 0, 100% = 1.0).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| BindEvent | `BindEvent` | Input event used to bind the Value. Default is OnChange. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| Decimals | `int?` | Gets or sets the number of decimal digits. Can be used only for floating point types, for integer types throws exception (for values other than 0). When not set, 2 decimal digits are used. |
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
| InputMode | `InputMode?` | Hint to browsers as to the type of virtual keyboard configuration to use when editing. If not set (neither with ` nor , i.e. null`), the ` will be used for equal to 0`. |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| ParsingErrorMessage | `string` | Gets or sets the error message used when displaying an a parsing error. Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property. |
| Placeholder | `string` | Placeholder for the input. |
| SelectOnFocus | `bool?` | Determines whether all the content within the input field is automatically selected when it receives focus. |
| Settings | `InputNumberSettings` | Set of settings to be applied to the component instance (overrides , overridden by individual parameters). |
| SmartKeyboard | `bool?` | When enabled, the input may provide an optimized keyboard experience for numeric entry. Currently, this means whenever a minus key is pressed, the sign of the number is toggled. Default is `true`. |
| SmartPaste | `bool?` | When enabled, pasted values are normalized to contain only valid numeric characters. Default is `true`. |
| Type | `InputType?` | Allows switching between textual and numeric input types. Only `InputType.Text` (default) and `InputType.Number` are supported. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the input number. |

## Available demo samples

- HxInputPercent_Demo_Basic.razor
- HxInputPercent_Demo_CustomInputGroupEnd.razor
- HxInputPercent_Demo_NoInputGroupEnd.razor

