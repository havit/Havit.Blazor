using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	/// <summary>
	/// Switch input.
	/// </summary>
	public class HxInputSwitch : HxInputCheckBox
	{
		/// <inheritdoc />
		private protected override string CoreCssClass => "form-check form-switch";
	}
}
