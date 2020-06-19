using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.ContextMenus
{
	public partial class HxContextMenuItem : ComponentBase
	{
		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[Parameter]
		public string Title { get; set; }

		[Parameter]
		public string ConfirmationQuestion { get; set; }

		[Parameter]
		public EventCallback OnItemClick { get; set; }

		[Inject]
		public IJSRuntime JSRuntime { get; set; }
		
		public async Task HandleClick()
		{
			if (!String.IsNullOrEmpty(ConfirmationQuestion))
			{
				if (!await JSRuntime.InvokeAsync<bool>("confirm", ConfirmationQuestion))
				{
					return; // No Action
				}
			}
			await OnItemClick.InvokeAsync(null);
		}
	}
}
