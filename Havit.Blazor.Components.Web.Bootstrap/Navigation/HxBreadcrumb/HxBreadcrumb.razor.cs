using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Indicate the current page’s location within a navigational hierarchy.
	/// </summary>
	public partial class HxBreadcrumb
	{
		/// <summary>
		/// Child content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }
		/// <summary>
		/// Breadcrumb divider. Default is <c>/</c>. Enter either a character (such as <c>></c>) or use an embedded SVG icon. Disable the divider with <c>null</c>.
		/// </summary>
		[Parameter] public string Divider { get; set; } = "/";

		/// <summary>
		/// Indicates whether the Divider is an image or a text.
		/// </summary>
		/// <returns></returns>
		private bool IsDividerImage()
		{
			Regex regex = new Regex(@"url\(");
			return regex.IsMatch(Divider);
		}
	}
}
