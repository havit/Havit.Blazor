using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNamedViewList<TFilterType>
	{
		[Parameter]
		// TODO: Pojmenování?
		// TODO: IEnumerable?
		public IEnumerable<NamedView<TFilterType>> NamedViews { get; set; }

		[Parameter]
		public TFilterType Filter { get; set; }
		
		[Parameter]
		public EventCallback<TFilterType> FilterChanged { get; set; }

		[Parameter]
		// TODO: Pojmenování?
		public EventCallback<NamedView<TFilterType>> SelectedNamedViewChanged { get; set; }

		protected async Task HandleNamedViewClick(NamedView<TFilterType> namedView)
		{
			TFilterType newFilter = namedView.Filter();
			if (newFilter != null)
			{
				Filter = newFilter; // POZOR, filtr je nutno klonovat, jinak budeme měnit instanci, která je použita pro filtr (ev. musíme z filtru vždy vracet novou instanci!!! to je možný předpoklad)
				await FilterChanged.InvokeAsync(newFilter);
			}

			await SelectedNamedViewChanged.InvokeAsync(namedView);
		}
	}
}
