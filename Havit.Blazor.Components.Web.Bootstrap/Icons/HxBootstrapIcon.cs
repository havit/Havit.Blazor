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
		[Parameter]	public string CssClass { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call

			// https://icons.getbootstrap.com/
			// <svg class="bi" width="32" height="32" fill="currentColor">
			//   <use xlink:href="bootstrap-icons.svg#toggles"/>
			// </svg>

			builder.OpenElement(0, "svg");
			builder.AddAttribute(1, "class", CssClassHelper.Combine("bi", CssClass));
			builder.AddAttribute(2, "width", "1em");
			builder.AddAttribute(3, "height", "1em");
			builder.AddAttribute(4, "fill", "currentColor");
			builder.AddMarkupContent(5, $"<use xlink:href=\"{GetIconHref()}\"></use>"); // does not work as OpenElement + AddAttribute
			builder.CloseElement(); // svg
		}

		private string GetIconHref()
		{
			return "/_content/Havit.Blazor.Components.Web.Bootstrap/bootstrap-icons.svg#" + Icon.Name;
		}
	}
}
