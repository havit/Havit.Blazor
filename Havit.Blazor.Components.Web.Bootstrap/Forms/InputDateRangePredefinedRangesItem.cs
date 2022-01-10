using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item for <see cref="HxInputDateRange.PredefinedDateRanges" />.
	/// </summary>
	public class InputDateRangePredefinedRangesItem
	{
		/// <summary>
		/// Custom label.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Resource type for IStringLocalizer&lt;ResourceType&gt; where the localization will be searched.
		/// </summary>
		public Type ResourceType { get; set; }

		/// <summary>
		/// Date range.
		/// </summary>
		public DateTimeRange DateRange { get; set; }
	}
}
