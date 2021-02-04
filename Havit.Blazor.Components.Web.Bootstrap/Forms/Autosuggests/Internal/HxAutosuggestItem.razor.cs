using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestItem
	{
		[Parameter] public string Text { get; set; }

		[Parameter] public EventCallback OnClick { get; set; }

		private async Task HandleItemClick()
		{
			await OnClick.InvokeAsync();
		}
	}
}
