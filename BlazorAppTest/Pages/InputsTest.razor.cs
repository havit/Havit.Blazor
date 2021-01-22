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
		protected FormModel Model { get; } = new FormModel();

		public class FormModel
		{
			[Required]
			[MaxLength(50)]
			public string Text { get; set; }

			public int IntegerValue { get; set; }

			public int? NullableIntegerValue { get; set; }

			public decimal DecimalValue { get; set; }

			public decimal? NullableDecimalValue { get; set; }

			public DateTime DateValue { get; set; }

			public DateTime? NullableDateValue { get; set; }

			[Required]
			public string CultureInfoName { get; set; }

			[Required]
			public CultureInfo CultureInfo { get; set; }

			public bool BoolValue { get; set; }
		}
	}
}
