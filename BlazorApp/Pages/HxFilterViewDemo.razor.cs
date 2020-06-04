using Havit.Blazor.Components.Web.Bootstrap.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
	public partial class HxFilterViewDemo
	{
		public CustomFilter Filter { get; set; } = new CustomFilter();

		public class CustomFilter
		{
			public DateRange MyDateRange { get; set; }
			public string MyText { get; set; }
		}

		private void SetRandomFilter()
		{
			Filter = new CustomFilter() { MyText = Guid.NewGuid().ToString() };
		}
	}
}
