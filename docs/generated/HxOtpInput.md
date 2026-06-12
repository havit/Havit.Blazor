# HxOtpInput

Bootstrap OTP input component (new in Bootstrap 6). Connected input fields for one-time passwords, PIN codes, and verification codes with automatic focus advancement, backspace navigation, paste support, and browser autofill (`autocomplete="one-time-code"`). The `Value` is bound as a `String` (the concatenated code).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| ChipTemplate | `RenderFragment` | The chip template. |
| Connected | `bool?` | When `true`, the inputs are visually connected into a single cohesive field (Bootstrap `input-group` styles). When used together with `GroupSize`, each group of inputs is wrapped in a nested `input-group`. The default is `false`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| GroupSize | `int?` | When set, the inputs are split into groups of this size with a visual `Separator` between the groups (e.g. `123-456`). The separator is purely visual, it does not affect the `Value`. The default is `null` (no groups). |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Length | `int?` | The number of inputs (digits) to render. The default is `6`. |
| Mask | `bool?` | When `true`, the inputs render as `type="password"` fields to hide the entered values. The default is `false`. |
| Separator | `string` | The separator text rendered between the groups of inputs (`GroupSize`). The default is `–` (en dash). |
| Settings | `OtpInputSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `string` | Value of the input. This should be used with two-way binding. |
| ValueChanged | `EventCallback<string>` | A callback that updates the bound value. |
| ValueExpression | `Expression<Func<string>>` | An expression that identifies the bound value. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnCompleted | `EventCallback<string>` | Raised when all inputs are filled (Bootstrap `complete.bs.otp-input` event). Receives the complete code as a parameter. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the component (the first empty input, or the last input when all inputs are filled). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `OtpInputSettings` | Application-wide defaults for the `HxOtpInput` component. |

## Available demo samples

- HxOtpInput_Demo.razor
- HxOtpInput_Demo_Connected.razor
- HxOtpInput_Demo_Disabled.razor
- HxOtpInput_Demo_OnCompleted.razor
- HxOtpInput_Demo_Pin.razor
- HxOtpInput_Demo_Separator.razor
- HxOtpInput_Demo_Validation.razor

