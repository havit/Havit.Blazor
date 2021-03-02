using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxSearchBox
	{
		[Parameter] public string QueryText { get; set; } // TODO: Když se komponenta jmenuje SearchBox, nepoužijeme jen Text? nebo SearchText?

		[Parameter] public EventCallback<string> OnQueryTextChanged { get; set; }

		protected async Task QueryTextChanged()
		{
			await OnQueryTextChanged.InvokeAsync(QueryText);
		}
	}
}
