using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxDateRangeFilter : HxFilterBase<DateRange>
	{
		protected async Task StartDateChanged(ChangeEventArgs eventArgs)
		{
			Value = new DateRange(DateTime.Parse((string)eventArgs.Value), Value?.EndDate); // TODO: ErrorHandling
			await ValueChanged.InvokeAsync(Value);
		}

		protected async Task EndDateChanged(ChangeEventArgs eventArgs)
		{
			Value = new  DateRange(Value?.StartDate, DateTime.Parse((string)eventArgs.Value)); // TODO: ErrorHandling
			await ValueChanged.InvokeAsync(Value);
		}
	}
}
