# InputDateSettings

Settings for `HxInputDate`.

## Properties

| Name | Type | Description |
|------|------|-------------|
| CalendarDateCustomizationProvider | `CalendarDateCustomizationProviderDelegate` | Allows customization of the dates in the dropdown calendars. |
| CalendarIcon | `IconBase` | Optional icon to display within the input. |
| InputSize | `InputSize?` | Input size. |
| LabelType | `LabelType?` | The label type. |
| MaxDate | `DateTime?` | The last date selectable from the dropdown calendar. |
| MinDate | `DateTime?` | The first date selectable from the dropdown calendar. |
| PredefinedDates | `IEnumerableInputDatePredefinedDatesItem>` | The predefined dates to be displayed. |
| ShowClearButton | `bool?` |  |
| ShowPredefinedDates | `bool?` | When enabled, shows predefined days (from `HxInputDate.PredefinedDates`, e.g., Today). |
| TimeProvider | `TimeProvider` | The TimeProvider to use, note: overrides the 'Today' in PredefinedDates. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. |

