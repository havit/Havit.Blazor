namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Application-wide defaults and localization resource marker for <see cref="HxInputDateRange{TValue}"/>.
/// </summary>
public sealed class HxInputDateRange
{
	/// <summary>
	/// Application-wide defaults shared by all supported value types of <see cref="HxInputDateRange{TValue}"/>.
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
}
