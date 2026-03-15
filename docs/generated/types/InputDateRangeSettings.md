# InputDateRangeSettings

Settings for `HxInputDateRange`.

## Properties

| Name | Type | Description |
|------|------|-------------|
| CalendarDateCustomizationProvider | `CalendarDateCustomizationProviderDelegate` | Allows customization of the dates in the dropdown calendars. |
| CalendarIcon | `IconBase` | Optional icon to display within the input. |
| FromPlaceholder | `string` | Placeholder for the start-date input. |
| InputSize | `InputSize?` | Input size. |
| MaxDate | `DateTime?` | The last date selectable from the dropdown calendar. |
| MinDate | `DateTime?` | The first date selectable from the dropdown calendar. |
| PredefinedDateRanges | `IEnumerableInputDateRangePredefinedRangesItem>` | The predefined date ranges to be displayed. |
| RequireDateOrder | `bool?` | When enabled, validates that the "from" date is less than or equal to the "to" date. The default is `true`. |
| ShowClearButton | `bool?` |  |
| ShowPredefinedDateRanges | `bool?` | When enabled, shows predefined day ranges (from `HxInputDateRange.PredefinedDateRanges`, e.g., Today). |
| TimeProvider | `TimeProvider` | The TimeProvider used to get DateTime.Today. |
| ToPlaceholder | `string` | Placeholder for the end-date input. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. |

