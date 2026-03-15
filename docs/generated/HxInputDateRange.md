# HxInputDateRange

Date range picker. Form input component for entering a start date and an end date.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| CalendarDateCustomizationProvider | `CalendarDateCustomizationProviderDelegate` | Allows customization of the dates in the dropdown calendars. The default customization is configurable with `Defaults`. |
| CalendarIcon | `IconBase` |  |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DateOrderErrorMessage | `string` | Gets or sets the error message used when the "from" date is greater than the "to" date (used with `RequireDateOrder`). Used with `String.Format(...)`, `{0}` is replaced by the Label property, `{1}` is replaced by the name of the bounded property. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| FromCalendarDisplayMonth | `DateTime` | The month to display in the from calendar when no start date is selected. |
| FromParsingErrorMessage | `string` | Gets or sets the error message used when displaying a "from" parsing error. Used with `String.Format(...)`, `{0}` is replaced by the Label property, `{1}` is replaced by the name of the bounded property. |
| FromPlaceholder | `string` | Placeholder for the start-date input. If not set, localized default is used ("From" + localizations). |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| MaxDate | `DateTime?` | The last date selectable from the dropdown calendar. The default is `31.12.2099` (configurable from `Defaults`). |
| MinDate | `DateTime?` | The first date selectable from the dropdown calendar. The default is `1.1.1900` (configurable from `Defaults`). |
| PredefinedDateRanges | `IEnumerableInputDateRangePredefinedRangesItem>` | Predefined dates to be displayed. |
| RequireDateOrder | `bool?` | When enabled, validates that the "from" date is less than or equal to the "to" date. The default is `true` (configurable from `Defaults`). |
| Settings | `InputDateRangeSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowClearButton | `bool?` | Indicates whether the Clear button in the dropdown calendar should be visible. The default is `true` (configurable in `HxInputDate.Defaults`). |
| ShowPredefinedDateRanges | `bool?` | When enabled (default is `true`), shows predefined days (from `PredefinedDateRanges`, e.g. Today). |
| TimeProvider | `TimeProvider` | TimeProvider is resolved in the following order: 1. TimeProvider from this parameter 2. Settings TimeProvider (configurable from `Settings`) 3. Defaults TimeProvider (configurable from `Defaults`) |
| ToCalendarDisplayMonth | `DateTime` | The month to display in the to calendar when no end date or start date is selected. It will default to `FromCalendarDisplayMonth`. |
| ToParsingErrorMessage | `string` | Gets or sets the error message used when displaying a "to" parsing error. Used with `String.Format(...)`, `{0}` is replaced by the Label property, `{1}` is replaced by the name of the bounded property. |
| ToPlaceholder | `string` | Placeholder for the end-date input. If not set, localized default is used ("End" + localizations). |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `DateTimeRange` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<DateTimeRange>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<DateTimeRange>` | A callback that updates the bound value. |

## Properties

| Name | Type | Description |
|------|------|-------------|
| FromPlaceholderEffective | `string` |  |
| ToPlaceholderEffective | `string` |  |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` |  |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputDateRangeSettings` | Application-wide defaults for the `HxInputDateRange` component. |

## Available demo samples

- HxInputDateRange_Demo.razor
- HxInputDateRange_Demo_CalendarDateCustomization.razor
- HxInputDateRange_Demo_CalendarIcon.razor
- HxInputDateRange_Demo_ClearButton.razor
- HxInputDateRange_Demo_CustomRanges.razor
- HxInputDateRange_Demo_MinDateMaxDate.razor

