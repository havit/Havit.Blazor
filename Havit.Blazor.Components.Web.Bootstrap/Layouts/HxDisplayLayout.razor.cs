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
		[Parameter] public EventCallback OnClosed { get; set; }

		private HxModal modalComponent;
		private HxOffcanvas offcanvasComponent;

		public async Task ShowAsync()
		{
			if (DisplayMode == LayoutDisplayMode.Modal)
			{
				await modalComponent.ShowAsync();
			}
			else if (DisplayMode == LayoutDisplayMode.Offcanvas)
			{
				await offcanvasComponent.ShowAsync();
			}
		}

		public async Task HideAsync()
		{
			if (DisplayMode == LayoutDisplayMode.Modal)
			{
				await modalComponent.HideAsync();
			}
			else if (DisplayMode == LayoutDisplayMode.Offcanvas)
			{
				await offcanvasComponent.HideAsync();
			}
		}
	}
}
