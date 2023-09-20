namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxInputDate{TValue}"/>.
/// Marker for resources for <see cref="HxInputDate{TValue}"/>.
/// </summary>
public class HxInputDate
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputDate{TValue}"/>.
	/// </summary>
	public static InputDateSettings Defaults { get; set; }

	static HxInputDate()
	{
		Defaults = new InputDateSettings()
		{
			InputSize = InputSize.Regular,
			MinDate = HxCalendar.DefaultMinDate,
			MaxDate = HxCalendar.DefaultMaxDate,
			ShowClearButton = true,
			ShowPredefinedDates = true,
			PredefinedDates = new List<InputDatePredefinedDatesItem>() { new InputDatePredefinedDatesItem() { Label = "Today", ResourceType = typeof(HxInputDate), DateSelector = (timeProvider) => timeProvider.GetLocalNow().LocalDateTime.Date } },
		};
	}
}
