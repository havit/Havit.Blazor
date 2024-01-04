namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxCalendar"/> and derived components.
/// </summary>
public record CalendarSettings
{
	/// <summary>
	/// Minimum value to choose from the calendar.
	/// </summary>
	public DateTime? MinDate { get; set; }

	/// <summary>
	/// Maximum value to choose from the calendar.
	/// </summary>
	public DateTime? MaxDate { get; set; }

	/// <summary>
	/// Allows customization of the dates in the dropdown calendars.
	/// </summary>
	public CalendarDateCustomizationProviderDelegate DateCustomizationProvider { get; set; }

	/// <summary>
	/// TimeProvider to customize the today's date.
	/// </summary>
	public TimeProvider TimeProvider { get; set; }

}
