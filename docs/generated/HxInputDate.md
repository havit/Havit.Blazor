# HxInputDate

Date picker. Form input component for entering a date.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| CalendarDateCustomizationProvider | `CalendarDateCustomizationProviderDelegate` | Allows customization of the dates in the dropdown calendar. The default customization is configurable with `Defaults`. |
| CalendarDisplayMonth | `DateTime` | Default month to display in dropdown calendar when there is no Value. |
| CalendarIcon | `IconBase` | Optional icon to display within the input. Use `Defaults` to set the icon for the whole project. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupCssClass | `string` | Custom CSS class to render with the input-group span. |
| InputGroupEndTemplate | `RenderFragment` | The input-group at the end of the input. |
| InputGroupEndText | `string` | The input-group at the end of the input. |
| InputGroupStartTemplate | `RenderFragment` | The input-group at the beginning of the input. |
| InputGroupStartText | `string` | The input-group at the beginning of the input. |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| MaxDate | `DateTime?` | The last date selectable from the dropdown calendar. The default is `31.12.2099` (configurable from `Defaults`). |
| MinDate | `DateTime?` | The first date selectable from the dropdown calendar. The default is `1.1.1900` (configurable from `Defaults`). |
| ParsingErrorMessage | `string` | Gets or sets the error message used when displaying a parsing error. Used with `String.Format(...)`, `{0}` is replaced by `Label` property, `{1}` name of bounded property. |
| Placeholder | `string` | Placeholder for the input. |
| PredefinedDates | `IEnumerableInputDatePredefinedDatesItem>` | Predefined dates to be displayed. |
| Settings | `InputDateSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowClearButton | `bool?` | Indicates whether the Clear button in the dropdown calendar should be visible. The default is `true` (configurable in `Defaults`). |
| ShowPredefinedDates | `bool?` | When enabled (default is `true`), shows predefined days (from `PredefinedDates`, e.g. Today). |
| TimeProvider | `TimeProvider` | TimeProvider is resolved in the following order: 1. TimeProvider from this parameter 2. Settings TimeProvider (configurable from `Settings`) 3. Defaults TimeProvider (configurable from `Defaults`) |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` |  |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputDateSettings` | Application-wide defaults for the `HxInputDate`. |

## Available demo samples

- HxInputDate_Demo.razor
- HxInputDate_Demo_CalendarDateCustomization.razor
- HxInputDate_Demo_CalendarIcon.razor
- HxInputDate_Demo_ClearButton.razor
- HxInputDate_Demo_InputSize.razor
- HxInputDate_Demo_MinDateMaxDate.razor

