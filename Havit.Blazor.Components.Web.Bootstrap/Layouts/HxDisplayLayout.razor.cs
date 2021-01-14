using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxDisplayLayout : ComponentBase
	{
		[Parameter] public RenderFragment HeaderTemplate { get; set; }
		[Parameter] public RenderFragment BodyTemplate { get; set; }
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		[Parameter] public LayoutDisplayMode DisplayMode { get; set; }

		[Inject] protected ILogger<HxDisplayLayout> Logger { get; set; }

		private bool drawerIsOpen;
		private HxModal modal;

		protected override void OnInitialized()
		{
			Logger.LogDebug("OnInitialized");
		}

		public async Task ShowAsync()
		{
			Logger.LogDebug("ShowAsync");

			if (DisplayMode == LayoutDisplayMode.Modal)
			{
				await modal.ShowAsync();
			}
			else if (DisplayMode == LayoutDisplayMode.Drawer)
			{
				drawerIsOpen = true;
				StateHasChanged();
			}
		}

		public async Task HideAsync()
		{
			Logger.LogDebug("HideAsync");

			if (DisplayMode == LayoutDisplayMode.Modal)
			{
				await modal.HideAsync();
			}
			else if (DisplayMode == LayoutDisplayMode.Drawer)
			{
				drawerIsOpen = false;
				StateHasChanged();
			}
		}
	}
}
