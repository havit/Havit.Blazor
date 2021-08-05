using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Forms;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Input with sizing support.
	/// </summary>
	public interface IInputWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		InputSize? InputSize { get; set; }

		IInputDefaultsWithSize GetDefaults();

		InputSize InputSizeEffective => this.InputSize ?? GetDefaults().InputSize;

		/// <summary>
		/// Returns css class to render component with desired size.
		/// </summary>
		string GetInputSizeEffectiveCssClass() => this.InputSizeEffective.AsFormControlCssClass();
	}
}
