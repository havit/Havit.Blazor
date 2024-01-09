namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Indicates the date for which the calendar is to be customized.
/// Allows distinguishing between the From and To inputs in <see cref="HxInputDateRange"/>.
/// </summary>
public class CalendarDateCustomizationTarget
{
	/// <summary>
	/// Customization is for the standalone <see cref="HxCalendar" />.
	/// </summary>
	public static CalendarDateCustomizationTarget Calendar { get; } = new CalendarDateCustomizationTarget();

	/// <summary>
	/// Customization is for the dropdown calendar in <see cref="HxInputDate{TValue}" />.
	/// </summary>
	public static CalendarDateCustomizationTarget InputDate { get; } = new CalendarDateCustomizationTarget();

	/// <summary>
	/// Customization is for the From dropdown calendar in <see cref="HxInputDateRange" />.
	/// </summary>
	public static CalendarDateCustomizationTarget InputDateRangeFrom { get; } = new CalendarDateCustomizationTarget();

	/// <summary>
	/// Customization is for the To dropdown calendar in <see cref="HxInputDateRange" />.
	/// </summary>
	public static CalendarDateCustomizationTarget InputDateRangeTo { get; } = new CalendarDateCustomizationTarget();
}
