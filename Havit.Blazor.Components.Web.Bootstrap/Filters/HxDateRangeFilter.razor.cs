using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxDateRangeFilter : HxFilterBase<DateRange>
	{
		protected async Task StartDateChanged(ChangeEventArgs eventArgs)
		{
			if (DateTime.TryParse((string)eventArgs.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value))
			{
				Value = new DateRange(value, Value?.EndDate);
			}
			else
			{
				Value = new DateRange(null, Value?.EndDate); // TODO: ErrorHandling
			}

			await ValueChanged.InvokeAsync(Value);
		}

		protected async Task EndDateChanged(ChangeEventArgs eventArgs)
		{
			if (DateTime.TryParse((string)eventArgs.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value))
			{
				Value = new DateRange(Value?.StartDate, value);
			}
			else
			{
				Value = new DateRange(Value?.StartDate, null); // TODO: ErrorHandling
			}
			await ValueChanged.InvokeAsync(Value);
		}
	}
}
