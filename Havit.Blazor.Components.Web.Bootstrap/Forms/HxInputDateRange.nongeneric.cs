namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Application-wide defaults and localization resource marker for <see cref="HxInputDateRange{TValue}"/>.
/// </summary>
public sealed class HxInputDateRange
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputDateRange"/> component.
	/// </summary>
	public static InputDateRangeSettings Defaults { get; set; }

	static HxInputDateRange()
	{
		Defaults = new InputDateRangeSettings()
		{
			MinDate = HxCalendar.DefaultMinDate,
			MaxDate = HxCalendar.DefaultMaxDate,
			ShowClearButton = true,
			ShowPredefinedDateRanges = true,
			PredefinedDateRanges = null,
			RequireDateOrder = true,
		};
	}

	/// <summary>
	/// Application-wide defaults for date-only ranges. Independent of <see cref="Defaults"/>.
	/// </summary>
	public static InputDateRangeSettings<DateOnlyRange> DateOnlyDefaults { get; set; } = new()
	{
		MinDate = HxCalendar.DefaultMinDate,
		MaxDate = HxCalendar.DefaultMaxDate,
		ShowClearButton = true,
		ShowPredefinedDateRanges = true,
		RequireDateOrder = true,
	};

	internal static InputDateRangeSettings<TValue> GetDefaults<TValue>()
	{
		if (typeof(TValue) == typeof(DateTimeRange))
		{
			return (InputDateRangeSettings<TValue>)(object)Defaults;
		}
		if (typeof(TValue) == typeof(DateOnlyRange))
		{
			return (InputDateRangeSettings<TValue>)(object)DateOnlyDefaults;
		}
		throw new InvalidOperationException($"Unsupported range type {typeof(TValue)}. Use DateTimeRange or DateOnlyRange.");
	}
}
