using Microsoft.AspNetCore.Components;
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
		public EventCallback OnItemClick { get; set; }

		public async Task OnClick()
		{
			await OnItemClick.InvokeAsync(null);
		}
	}
}
