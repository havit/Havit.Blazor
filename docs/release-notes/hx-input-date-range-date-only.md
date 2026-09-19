# HxInputDateRange: native DateOnly ranges

`HxInputDateRange<TValue>` now supports `Havit.DateOnlyRange` as well as `Havit.DateTimeRange`, using Havit.Core 2.0.41. Date-only model properties bind directly, including through thin wrappers forwarding `ValueExpression`. Validation messages, field notifications, and field CSS use the original parent model field.

Both endpoints remain nullable, and an empty range is `default`. Calendar selection, typing, clear buttons, predefined ranges, and optional date-order validation share the existing picker implementation. Internal date-only conversions do not apply UTC or time-zone adjustments.

## Source migration

- Existing `<HxInputDateRange @bind-Value="model.Range" />` markup continues to infer its value type.
- Explicit C# references, subclasses, and render-tree calls must use `HxInputDateRange<DateTimeRange>` or `HxInputDateRange<DateOnlyRange>`. This includes the fields used by `@ref`.
- Supply `TValue` when no value/binding can infer the type.
- For date-only values, use `InputDateRangeSettings<DateOnlyRange>` and `InputDateRangePredefinedRangesItem<DateOnlyRange>`. The non-generic settings/item classes remain DateTime specializations. Collections exposed by settings and the component now use the generic item type; code reading them into non-generic item collections may need adjustment.
- `HxInputDateRange.Defaults` retains DateTime defaults. `HxInputDateRange.DateOnlyDefaults` configures independent DateOnly defaults.
- Calendar limits, display months, and customization callbacks retain their existing DateTime types, including in settings, consistently with `HxInputDate` and `HxCalendar`. Limits control calendar selection; manually typed dates still require application validation where applicable.
- Replace application-side range conversion properties with direct DateOnlyRange binding. Wrappers can forward all three binding parameters without converting expressions or adding a separate validation-message component.

This is a source- and binary-breaking component API change; consumers must rebuild and apply the reference changes above.

Related: [#1807](https://github.com/havit/Havit.Blazor/issues/1807).
