using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxSearchBox
	{
		[Parameter]
		// TODO: Když se komponenta jmenuje SearchBox, nepoužijeme jen Text? nebo SearchText?
		public string QueryText { get; set; }

		[Parameter]
		public EventCallback<string> OnQueryTextChanged { get; set; }

		protected async Task QueryTextChanged()
		{
			await OnQueryTextChanged.InvokeAsync(QueryText);
		}
	}
}
