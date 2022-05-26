namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Allows customization of specific date in calendar (<see cref="HxCalendar"/>, <see cref="HxInputDate{TValue}"/>, <see cref="HxInputDateRange"/>).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/CalendarDateCustomizationProviderDelegate">https://havit.blazor.eu/types/CalendarDateCustomizationProviderDelegate</see>
	/// </summary>
	public delegate CalendarDateCustomizationResult CalendarDateCustomizationProviderDelegate(CalendarDateCustomizationRequest request);
}
