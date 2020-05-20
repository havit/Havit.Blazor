using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxButton : ComponentBase
	{
		[Parameter]
		public string Text { get; set; }

		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[Parameter]
		public string Skin { get; set; } // TODO

		[Parameter]
		public bool Enabled { get; set; } // TODO

		[Parameter]
		public EventCallback<MouseEventArgs> OnClick { get; set; }

		private async Task ButtonClicked(MouseEventArgs mouseEventArgs)
		{
			await OnClick.InvokeAsync(mouseEventArgs);
		}
	}
}
