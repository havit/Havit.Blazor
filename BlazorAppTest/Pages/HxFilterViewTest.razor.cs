using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppTest.Pages
{
	public partial class HxFilterViewTest
	{
		public CustomFilter Filter { get; set; } = new CustomFilter();

		public class CustomFilter
		{
			[Required]
			public string Text { get; set; }
			public DateTime? DateFrom { get; set; }
			public DateTime? DateTo { get; set; }
		}

		private void SetRandomFilter()
		{
			Filter = new CustomFilter
			{
				Text = Guid.NewGuid().ToString()
			};
		}
	}
}
