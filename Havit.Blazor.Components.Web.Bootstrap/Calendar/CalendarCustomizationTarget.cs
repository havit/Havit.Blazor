using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Calendar display customization target.
	/// Useful for distinquishing From and To inputs in <see cref="HxInputDateRange"/>.
	/// </summary>
	public class CalendarCustomizationTarget
	{
		/// <summary>
		/// Customization is for <see cref="HxCalendar" />.
		/// </summary>
		public static CalendarCustomizationTarget Calendar { get; } = new CalendarCustomizationTarget();

		/// <summary>
		/// Customization is for a calendar in <see cref="HxInputDate{TValue}" />.
		/// </summary>
		public static CalendarCustomizationTarget InputDate { get; } = new CalendarCustomizationTarget();

		/// <summary>
		/// Customization is for a calendar for From in <see cref="HxInputDateRange" />.
		/// </summary>
		public static CalendarCustomizationTarget InputDateRangeFrom { get; } = new CalendarCustomizationTarget();

		/// <summary>
		/// Customization is for a calendar for To in <see cref="HxInputDateRange" />.
		/// </summary>
		public static CalendarCustomizationTarget InputDateRangeTo { get; } = new CalendarCustomizationTarget();
	}
}
