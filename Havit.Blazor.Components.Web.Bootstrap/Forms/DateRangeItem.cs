using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date range item for <see cref="HxInputDateRange" />.
	/// </summary>
	public class DateRangeItem
	{
		/// <summary>
		/// Custom label.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Date range.
		/// </summary>
		public DateTimeRange DateRange { get; set; }
	}
}
