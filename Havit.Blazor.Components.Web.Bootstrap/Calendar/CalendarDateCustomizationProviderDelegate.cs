namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Allows customization of specific date in calendar (<see cref="HxCalendar"/>, <see cref="HxInputDate{TValue}"/>, <see cref="HxInputDateRange"/>).
	/// </summary>
	public delegate CalendarDateCustomizationResult CalendarDateCustomizationProviderDelegate(CalendarDateCustomizationRequest request);
}
