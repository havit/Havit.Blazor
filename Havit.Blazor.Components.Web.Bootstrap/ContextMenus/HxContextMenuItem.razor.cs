using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxContextMenuItem : ComponentBase
	{
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string Title { get; set; }
		[Parameter] public IconBase Icon { get; set; }
		[Parameter] public string ConfirmationQuestion { get; set; }
		[Parameter] public EventCallback OnItemClick { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		public async Task HandleClick()
		{
			if (!String.IsNullOrEmpty(ConfirmationQuestion))
			{
				if (!await JSRuntime.InvokeAsync<bool>("confirm", ConfirmationQuestion)) // TODO: HxMessageBox/HxModal spíš než JS-confirm
				{
					return; // No Action
				}
			}
			await OnItemClick.InvokeAsync(null);
		}
	}
}
