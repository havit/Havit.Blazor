using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Input with sizing support.
	/// </summary>
	public interface IInputWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		InputSize InputSize { get; set; }

		/// <summary>
		/// Returns css class to render component with desired size.
		/// </summary>
		public string GetInputSizeCssClass()
		{
			return InputSize switch
			{
				InputSize.Regular => null,
				InputSize.Small => "form-control-sm",
				InputSize.Large => "form-control-lg",
				_ => throw new InvalidOperationException(InputSize.ToString())
			};
		}
	}
}
