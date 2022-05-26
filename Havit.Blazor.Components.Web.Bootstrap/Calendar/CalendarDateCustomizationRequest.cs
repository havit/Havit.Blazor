﻿namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Customization request for specific date in calendar (<see cref="HxCalendar"/>, <see cref="HxInputDate{TValue}"/>, <see cref="HxInputDateRange"/>).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/CalendarDateCustomizationRequest">https://havit.blazor.eu/types/CalendarDateCustomizationRequest</see>
	/// </summary>
	public record CalendarDateCustomizationRequest
	{
		/// <summary>
		/// Who is asking for a customization, where the customization will be applied.
		/// Allows distinquishing From and To inputs in <see cref="HxInputDateRange"/>.
		/// </summary>
		public CalendarDateCustomizationTarget Target { get; init; }

		/// <summary>
		/// Date to be customized.
		/// </summary>
		public DateTime Date { get; init; }
	}
}