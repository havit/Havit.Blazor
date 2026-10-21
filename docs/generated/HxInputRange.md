# HxInputRange

Allows the user to select a number in a specified range using a slider. Mirrors the Bootstrap range component: a JavaScript-driven control that draws a filled track and can show an optional value `HxInputRange.Bubble` and `HxInputRange.Ticks` (tick marks).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Max **[REQUIRED]** | `TValue` | Maximum value. |
| Min **[REQUIRED]** | `TValue` | Minimum value. |
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| BindEvent | `BindEvent?` | Instructs whether the `Value` is going to be updated `oninput` (immediately), or `onchange` (usually `onmouseup`). Default is `BindEvent.OnChange`. |
| Bubble | `bool?` | When `true`, a value bubble following the thumb is shown. The default is `false`. |
| ChipTemplate | `RenderFragment` | The chip template. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| Settings | `InputRangeSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Step | `TValue` | By default, `HxInputRange` snaps to integer values. To change this, you can specify a step value. |
| Ticks | `IEnumerableInputRangeTick>` | Tick marks rendered along the track from a linked `<datalist>`. Each tick is positioned at its `InputRangeTick.Value` (even when the values are unevenly spaced) and an optional `InputRangeTick.Label` is shown beneath it. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Floating`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |

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
- HxInputRange_Demo_Bubble.razor
- HxInputRange_Demo_Disabled.razor
- HxInputRange_Demo_MinMax.razor
- HxInputRange_Demo_Steps.razor
- HxInputRange_Demo_Ticks.razor

