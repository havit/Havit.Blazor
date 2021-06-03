using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Displays bootstrap icon. See https://icons.getbootstrap.com/.
	/// </summary>
	internal class HxBootstrapIcon : ComponentBase
	{
		/// <summary>
		/// Icon to display.
		/// </summary>
		[Parameter] public BootstrapIcon Icon { get; set; }

		/// <summary>
		/// CSS Class to combine with basic icon CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call

			// https://icons.getbootstrap.com/

			builder.OpenElement(0, "i");
			builder.AddAttribute(1, "class", CssClassHelper.Combine("hx-icon", "bi-" + Icon.Name, CssClass));

			builder.CloseElement(); // i
		}
	}
}
