using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxBreadcrumb
	{
		/// <summary>
		/// Child content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }
		/// <summary>
		/// Breadcrumb divider. Default is <c>/</c>. Enter either a character (such as <c>></c>) or use an embedded SVG icon. Disable the divider with the value <c>none</c>.
		/// </summary>
		[Parameter] public string Divider { get; set; }
	}
}
