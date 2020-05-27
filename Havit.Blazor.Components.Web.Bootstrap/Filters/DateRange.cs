using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public class DateRange
	{
		public DateTime? StartDate { get; }

		public DateTime? EndDate { get; }

		public DateRange()
		{
			// NOOP
		}

		public DateRange(DateTime? startDate, DateTime? endDate)
		{
			this.StartDate = startDate;
			this.EndDate = endDate;
		}

		public override string ToString()
		{
			return $"StartDate: { StartDate?.ToShortDateString() ?? "null" }, EndDate: { EndDate?.ToShortDateString() ?? "null" }";
		}
	}
}
