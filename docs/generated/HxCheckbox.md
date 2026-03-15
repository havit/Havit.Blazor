# HxCheckbox

Checkbox input. (Replaces the former `HxInputCheckbox` component which was dropped in v 4.0.0.)

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| Color | `ThemeColor?` | Bootstrap button style - theme color. The default is taken from `HxButton.Defaults` (`ThemeColor.None` if not customized). For `CheckboxRenderMode.ToggleButton`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| Inline | `bool` | Allows grouping checkboxes on the same horizontal row by rendering them inline. The default value is `false`. This only works when there is no label, no hint, and no validation message. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Outline | `bool?` | Bootstrap "outline" button style. For `CheckboxRenderMode.ToggleButton`. |
| RenderMode | `CheckboxRenderMode?` | Checkbox render mode. |
| Reverse | `bool` | Put the checkbox on the opposite side - first text, then checkbox. |
| Settings | `CheckboxSettings` | Set of settings to be applied to the component instance. |
| Text | `string` | Text to display next to the checkbox. |
| TextCssClass | `string` | CSS class to apply to the text. |
| TextTemplate | `RenderFragment` | Content to display next to the checkbox. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `bool` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<bool>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<bool>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the checkbox. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `CheckboxSettings` | Application-wide defaults for `HxCheckbox` and derived components. |

## Available demo samples

- HxCheckbox_Demo.razor
- HxCheckbox_Demo_Inline.razor
- HxCheckbox_Demo_RenderModes.razor
- HxCheckbox_Demo_Reverse.razor

