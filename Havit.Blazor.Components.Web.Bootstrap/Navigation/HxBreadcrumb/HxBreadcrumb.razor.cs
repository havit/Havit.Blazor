using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/breadcrumb/">Bootstrap 5 Breadcrumb</see> component.<br />
	/// Indicates the current page’s location within a navigational hierarchy.
	/// </summary>
	public partial class HxBreadcrumb
	{
		/// <summary>
		/// Child content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Breadcrumb divider. Default is <c>/</c>.
		/// Enter either a character (such as <c>></c>) or use an embedded SVG icon.
		/// Disable the divider with <c>null</c>.
		/// </summary>
		[Parameter] public string Divider { get; set; } = "/";

		/// <summary>
		/// Indicates whether the Divider is an image or a text.
		/// </summary>
		private bool IsDividerImage()
		{
			return Divider.Contains("url(");
		}
	}
}
