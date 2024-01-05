namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Customization result for a specific date in the calendar (<see cref="HxCalendar"/>, <see cref="HxInputDate{TValue}"/>, <see cref="HxInputDateRange"/>).
/// </summary>
public class CalendarDateCustomizationResult
{
	/// <summary>
	/// Indicates whether the date is enabled.<br />
	/// The default value is <c>true</c>.
	/// </summary>
	public bool Enabled { get; init; } = true;

	/// <summary>
	/// Specifies an additional CSS class for the date.
	/// </summary>
	public string CssClass { get; init; }
}
