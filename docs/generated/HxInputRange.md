# HxInputRange

Allows the user to select a number in a specified range using a slider.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Max **[REQUIRED]** | `TValue` | Maximum value. |
| Min **[REQUIRED]** | `TValue` | Minimum value. |
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| BindEvent | `BindEvent?` | Instructs whether the `Value` is going to be updated `oninput` (immediately), or `onchange` (usually `onmouseup`). Default is `BindEvent.OnChange`. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Settings | `InputRangeSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Step | `TValue` | By default, `HxInputRange` snaps to integer values. To change this, you can specify a step value. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` | Focuses the input range. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputRangeSettings` | Application-wide defaults for the `HxInputRange`. |

## Available demo samples

- HxInputRange_Demo.razor
- HxInputRange_Demo_BindEvent.razor
- HxInputRange_Demo_Disabled.razor
- HxInputRange_Demo_Steps.razor

