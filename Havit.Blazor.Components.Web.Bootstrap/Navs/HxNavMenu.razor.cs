using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNavMenu : ComponentBase
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public RenderFragment LoginDisplayContent { get; set; }

		[Parameter] public string BrandName { get; set; }

		[Parameter] public string BrandNameShort { get; set; }

		private bool CollapsedMobile { get; set; } = true;
		private bool CollapsedDesktop { get; set; } = false;

		private string GetCollapsedMobileCssClass() => CollapsedMobile ? "collapse" : null;

		private void ToggleMobileNavMenu()
		{
			CollapsedMobile = !CollapsedMobile;
			CollapsedDesktop = false;
		}
		private void ToggleDesktopNavMenu()
		{
			CollapsedDesktop = !CollapsedDesktop;
			CollapsedMobile = true;
		}
	}
}
