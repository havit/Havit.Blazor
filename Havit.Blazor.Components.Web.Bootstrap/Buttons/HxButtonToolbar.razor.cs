using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap <see href="https://getbootstrap.com/docs/5.1/components/button-group/#button-toolbar">Button toolbar</see> component.
	/// </summary>
	public partial class HxButtonToolbar
	{
		/// <summary>
		/// Toolbar's content
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// An explicit label should be set, as most assistive technologies will otherwise not announce them, despite the presence of the correct role attribute. 
		/// </summary>
		[Parameter] public string AriaLabel { get; set; }

		/// <summary>
		/// CSS class(es) to add to the HTML element with the <c>.btn-toolbar</c> class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }
	}
}
