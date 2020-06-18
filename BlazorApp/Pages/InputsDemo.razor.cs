using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
	public partial class InputsDemo
	{
		protected FormModel Model { get; } = new FormModel();

		public class FormModel
		{
			[Required]
			public string Text { get; set; }

			[Required]
			public string Password { get; set; }

			public int IntegerValue { get; set; }

			public int? NullableIntegerValue { get; set; }

			public decimal DecimalValue { get; set; }

			public decimal? NullableDecimalValue { get; set; }
			
			public DateTime DateValue { get; set; }

			public DateTime? NullableDateValue { get; set; }

			public bool BoolValue { get; set; }
		}
	}
}
