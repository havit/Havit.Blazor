# HxCalendar

Basic calendar building block. Used for `HxInputDate` and `HxInputDateRange` implementation.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| DateCustomizationProvider | `CalendarDateCustomizationProviderDelegate` | Allows customization of the dates in calendar. |
| DisplayMonth | `DateTime` | Month to display. |
| DisplayMonthChanged | `EventCallback<DateTime>` | Raised when the month selection changes. |
| KeyboardNavigation | `bool` | Indicates whether the keyboard navigation is enabled. When disabled, the calendar renders `tabindex="-1"` on interactive elements. Default is `true` (tabindex attribute is not rendered). |
| MaxDate | `DateTime?` | Last date selectable from the calendar. Default is `31.12.2099` (configurable from `Defaults`). |
| MinDate | `DateTime?` | First date selectable from the calendar. Default is `1.1.1900` (configurable from `Defaults`). |
| Settings | `CalendarSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| TimeProvider | `TimeProvider` | TimeProvider is resolved in the following order: 1. TimeProvider from this parameter 2. Settings TimeProvider (configurable from `Settings`) 3. Defaults TimeProvider (configurable from `Defaults`) |
| Value | `DateTime?` | Date selected. |
| ValueChanged | `EventCallback<Nullable<DateTime>>` | Raised when the selected date changes. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| RefreshAsync() | `Task` | Refreshes the calendar. Useful when the customization needs to be updated. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `CalendarSettings` | Application-wide defaults for the `HxCalendar`. |

## Available demo samples

- HxCalendar_Demo.razor

