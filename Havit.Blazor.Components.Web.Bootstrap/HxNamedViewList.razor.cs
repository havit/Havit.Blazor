using Havit.Blazor.Components.Web.Bootstrap.NamedViews;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNamedViewList
	{
		[Parameter]
		// TODO: Pojmenování?
		// TODO: IEnumerable?
		public IEnumerable<NamedView> Data { get; set; }

		[Parameter]
		public NamedView SelectedNamedView { get; set; }

		[Parameter]
		// TODO: Pojmenování?
		public EventCallback<NamedView> OnNamedViewChanged { get; set; }

		//TODO: Click nebo Clicked?
		protected async Task NamedViewClick(NamedView namedView)
		{
			await OnNamedViewChanged.InvokeAsync(namedView);
		}
	}
}
