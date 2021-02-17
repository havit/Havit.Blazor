using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppTest.Pages
{
	public partial class InputsTest
	{
		private FormModel model { get; set; } = new FormModel();

		public class FormModel : ICloneable
		{
			[Required]
			[MaxLength(50)]
			public string Text { get; set; }

			public int IntegerValue { get; set; }

			public int? NullableIntegerValue { get; set; }

			public decimal DecimalValue { get; set; }

			public decimal? NullableDecimalValue { get; set; }

			public DateTime DateValue { get; set; } = DateTime.Today;

			public DateTime? NullableDateValue { get; set; }

			[Required]
			public string CultureInfoName { get; set; }

			[Required]
			public CultureInfo CultureInfo { get; set; }

			public bool BoolValue { get; set; }

			public Havit.Blazor.Components.Web.Bootstrap.DateTimeRange DateRange { get; set; }

			object ICloneable.Clone() => MemberwiseClone();
		}
	}
}
