using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNamedViewList<TFilterModel>
	{
		[Parameter] public IEnumerable<NamedView<TFilterModel>> NamedViews { get; set; }

		[Parameter] public TFilterModel FilterModel { get; set; }

		[Parameter] public EventCallback<TFilterModel> FilterModelChanged { get; set; }

		[Parameter] public EventCallback<NamedView<TFilterModel>> OnNamedViewSelected { get; set; }

		protected async Task HandleNamedViewClick(NamedView<TFilterModel> namedView)
		{
			TFilterModel newFilter = namedView.Filter();
			if (newFilter != null)
			{
				FilterModel = newFilter; // POZOR, filter has to be clonned
				await FilterModelChanged.InvokeAsync(newFilter);
			}

			await OnNamedViewSelected.InvokeAsync(namedView);
		}
	}
}
