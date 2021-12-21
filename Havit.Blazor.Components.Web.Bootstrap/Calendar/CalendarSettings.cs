using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for <see cref="HxCalendar"/>.
	/// </summary>
	public class CalendarSettings
	{
		/// <summary>
		/// Internal HFW shared default.
		/// </summary>
		internal static DateTime DefaultMinDate => new DateTime(1900, 1, 1);

		/// <summary>
		/// Internal HFW shared default.
		/// </summary>
		internal static DateTime DefaultMaxDate => new DateTime(2099, 12, 31);

		/// <summary>
		/// Minimal value to choose from calendar.
		/// </summary>
		public DateTime MinDate { get; set; } = DefaultMinDate;

		/// <summary>
		/// Maximal value to choose from calendar.
		/// </summary>
		public DateTime MaxDate { get; set; } = DefaultMaxDate;

		/// <summary>
		/// Allows customization of the dates in dropdown calendars.
		/// </summary>
		public CalendarDateCustomizationProviderDelegate DateCustomizationProvider { get; set; }
	}
}
