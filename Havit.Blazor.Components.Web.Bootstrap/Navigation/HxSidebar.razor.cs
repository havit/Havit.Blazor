using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Sidebar navigation.
	/// </summary>
	public partial class HxSidebar : ComponentBase
	{
		/// <summary>
		/// Sidebar header.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Sidebar items. Use <see cref="HxSidebarItem"/>.
		/// </summary>
		[Parameter] public RenderFragment ItemsTemplate { get; set; }

		/// <summary>
		/// Icon for expanding the desktop version.
		/// </summary>
		[Parameter] public IconBase ExpandIcon { get; set; } = BootstrapIcon.ChevronBarRight;

		/// <summary>
		/// Icon for collapsing the desktop version.
		/// </summary>
		[Parameter] public IconBase CollapseIcon { get; set; } = BootstrapIcon.ChevronBarLeft;

		/// <summary>
		/// Sidebar footer (e.g. logged user, language switch, ...).
		/// </summary>
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }


		private string GetCollapsedCssClass() => Colapsed ? "collapsed" : null;
		private bool Colapsed { get; set; } = false;


		private void HandleCollapseToggleClick()
		{
			Colapsed = !Colapsed;
		}
	}
}
