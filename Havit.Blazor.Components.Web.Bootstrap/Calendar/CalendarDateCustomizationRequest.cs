namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Customization request for a specific date in the calendar (<see cref="HxCalendar"/>, <see cref="HxInputDate{TValue}"/>, <see cref="HxInputDateRange"/>).
/// </summary>
public record CalendarDateCustomizationRequest
{
	/// <summary>
	/// Who is requesting the customization and where it will be applied.
	/// Allows distinguishing between From and To inputs in <see cref="HxInputDateRange"/>.
	/// </summary>
	public CalendarDateCustomizationTarget Target { get; init; }

	/// <summary>
	/// The date to be customized.
	/// </summary>
	public DateTime Date { get; init; }
}
