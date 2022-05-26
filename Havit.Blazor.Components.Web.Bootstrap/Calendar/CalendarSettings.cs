namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxCalendar"/> and derived components.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/CalendarSettings">https://havit.blazor.eu/types/CalendarSettings</see>
	/// </summary>
	public record CalendarSettings
	{
		/// <summary>
		/// Minimal value to choose from calendar.
		/// </summary>
		public DateTime? MinDate { get; set; }

		/// <summary>
		/// Maximal value to choose from calendar.
		/// </summary>
		public DateTime? MaxDate { get; set; }

		/// <summary>
		/// Allows customization of the dates in dropdown calendars.
		/// </summary>
		public CalendarDateCustomizationProviderDelegate DateCustomizationProvider { get; set; }
	}
}
