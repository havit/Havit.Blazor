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
		/// <summary>
		/// Triggers the <see cref="FilterModelChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeFilterModelChangedAsync(TFilterModel newFilterModel) => FilterModelChanged.InvokeAsync(newFilterModel);

		[Parameter] public EventCallback<NamedView<TFilterModel>> OnNamedViewSelected { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnNamedViewSelected"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnNamedViewSelectedAsync(NamedView<TFilterModel> namedViewSelected) => OnNamedViewSelected.InvokeAsync(namedViewSelected);

		protected async Task HandleNamedViewClick(NamedView<TFilterModel> namedView)
		{
			TFilterModel newFilter = namedView.Filter();
			if (newFilter != null)
			{
				FilterModel = newFilter; // BEWARE, filter has to be clonned
				await InvokeFilterModelChangedAsync(newFilter);
			}

			await InvokeOnNamedViewSelectedAsync(namedView);
		}
	}
}
