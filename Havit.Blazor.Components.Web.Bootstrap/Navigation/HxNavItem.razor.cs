using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.0/components/navs-tabs/">Bootstrap nav-item</see> component.
	/// </summary>
	public partial class HxNavItem
	{
		/// <summary>
		/// Icon (optional).
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Basic text representing the nav-item.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Navigation target.
		/// </summary>
		[Parameter] public string Href { get; set; }

		/// <summary>
		/// Collapsible child-content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private string id = "hx" + Guid.NewGuid().ToString("N");
	}
}
