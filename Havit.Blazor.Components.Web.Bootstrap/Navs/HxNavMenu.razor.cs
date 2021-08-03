using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNavMenu : ComponentBase
	{





		[Parameter] public string BrandName { get; set; }

		[Parameter] public string BrandNameShort { get; set; }

		[Parameter] public RenderFragment BrandTemplate { get; set; }

		[Parameter] public RenderFragment ItemsTemplate { get; set; }

		[Parameter] public IconBase DesktopOpenCollapseIcon { get; set; } = BootstrapIcon.ChevronBarRight;

		[Parameter] public IconBase DesktopCloseCollapseIcon { get; set; } = BootstrapIcon.ChevronBarLeft;

		[Parameter] public IconBase LoginIcon { get; set; } = BootstrapIcon.Person;

		[Parameter] public RenderFragment LoginDisplayTemplate { get; set; }

		private bool CollapsedMobile { get; set; } = false;

		private bool CollapsedDesktop { get; set; } = false;

		private string GetCollapsedMobileCssClass() => CollapsedMobile ? "collapse" : null;

		private string GetDesktopCollapsedCssClass() => CollapsedDesktop ? "desktop-collapse" : null;

		private void ToggleMobileNavMenu()
		{
			CollapsedMobile = !CollapsedMobile;
			CollapsedDesktop = false;
		}

		private void ToggleDesktopNavMenu()
		{
			CollapsedDesktop = !CollapsedDesktop;
			CollapsedMobile = false;
		}
	}
}
