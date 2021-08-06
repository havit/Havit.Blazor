using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap Dropdown menu which opens when triggered.
	/// </summary>
	public partial class HxDropdownMenu
	{
		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
