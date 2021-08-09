using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item for the <see cref="HxSidebar"/>.
	/// </summary>
	public partial class HxSidebarItem : ComponentBase
	{
		/// <summary>
		/// Item text.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Item icon (optional).
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Item navigation target.
		/// </summary>
		[Parameter] public string Href { get; set; }

		/// <summary>
		/// URL matching behavior for the underlying <see cref="NavLink"/>.
		/// Default is <c>null</c> (none).
		/// </summary>
		[Parameter] public NavLinkMatch? Match { get; set; }

		/// <summary>
		/// Any additional CSS class to add.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Sub-items (not intended to be used for any other purpose).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private string id = "hx" + Guid.NewGuid().ToString("N");
	}
}
