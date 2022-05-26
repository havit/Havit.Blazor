﻿namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Indicates date in which calendar is to be customized.
	/// Allows distinquishing From and To inputs in <see cref="HxInputDateRange"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/CalendarDateCustomizationTarget">https://havit.blazor.eu/types/CalendarDateCustomizationTarget</see>
	/// </summary>
	public class CalendarDateCustomizationTarget
	{
		/// <summary>
		/// Customization is for standalone <see cref="HxCalendar" />.
		/// </summary>
		public static CalendarDateCustomizationTarget Calendar { get; } = new CalendarDateCustomizationTarget();

		/// <summary>
		/// Customization is for the dropdown calendar in <see cref="HxInputDate{TValue}" />.
		/// </summary>
		public static CalendarDateCustomizationTarget InputDate { get; } = new CalendarDateCustomizationTarget();

		/// <summary>
		/// Customization is for the <i>From</i> dropdown calendar in <see cref="HxInputDateRange" />.
		/// </summary>
		public static CalendarDateCustomizationTarget InputDateRangeFrom { get; } = new CalendarDateCustomizationTarget();

		/// <summary>
		/// Customization is for the <i>To</i> dropdown calendar in <see cref="HxInputDateRange" />.
		/// </summary>
		public static CalendarDateCustomizationTarget InputDateRangeTo { get; } = new CalendarDateCustomizationTarget();
	}
}
