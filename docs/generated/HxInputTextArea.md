# HxInputTextArea

Textarea. To set a custom height, do not use the rows attribute. Instead, set an explicit height (either inline or via custom CSS).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| BindEvent | `BindEvent` | Gets or sets the behavior when the model is updated from the input. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupEndTemplate | `RenderFragment` | Input group at the end of the input. |
| InputGroupEndText | `string` | Input group at the end of the input. |
| InputGroupStartTemplate | `RenderFragment` | Input group at the beginning of the input. |
| InputGroupStartText | `string` | Input group at the beginning of the input. |
| InputMode | `InputMode?` | Hint to browsers as to the type of virtual keyboard configuration to use when editing. The default is `null` (not set). |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| MaxLength | `int?` | The maximum number of characters (UTF-16 code units) that the user can enter. If the parameter value isn't specified, the `MaxLengthAttribute` of the `Value` is checked and used. If not specified either, the user can enter an unlimited number of characters. |
| Placeholder | `string` | Placeholder for the input. |
| SelectOnFocus | `bool?` | Determines whether all the text within the input field is automatically selected when it receives focus. |
| Settings | `InputTextSettings` | Set of settings to be applied to the component instance (overrides `HxInputText.Defaults`, overridden by individual parameters). |
| Spellcheck | `bool?` | Defines whether the input may be checked for spelling errors. |
| Type | `InputType` | Input type. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `string` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<string>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<string>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the component. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputTextSettings` | Application-wide defaults for the `HxInputFile` and derived components. Full documentation and demos: https://havit.blazor.eu/components/HxInputText. |

## Available demo samples

- HxInputTextArea_Demo.razor

