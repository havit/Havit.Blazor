using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Dropdowns;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Custom dropdown content for <see cref="HxDropdown"/>.
	/// </summary>
	public partial class HxDropdownContent
	{
		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }

		protected string GetCssClass() =>
			CssClassHelper.Combine(
				"dropdown-menu",
				((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
				this.CssClass
				);
	}
}
