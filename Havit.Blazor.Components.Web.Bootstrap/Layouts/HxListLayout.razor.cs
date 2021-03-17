using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Layouts;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxListLayout<TFilterModel>
	{
		[Parameter] public string Title { get; set; }

		[Parameter] public RenderFragment TitleTemplate { get; set; }

		[Parameter] public RenderFragment NamedViewsTemplate { get; set; }

		[Parameter] public RenderFragment SearchTemplate { get; set; } // TODO SearchTemplate

		[Parameter] public RenderFragment<TFilterModel> FilterTemplate { get; set; }

		[Parameter] public TFilterModel FilterModel { get; set; }
		[Parameter] public EventCallback<TFilterModel> FilterModelChanged { get; set; }

		[Parameter] public bool FilterDrawerOpen { get; set; }

		[Parameter] public RenderFragment DataTemplate { get; set; }

		[Parameter] public RenderFragment DetailTemplate { get; set; }

		[Parameter] public RenderFragment CommandsTemplate { get; set; }

		[Inject] protected IStringLocalizer<HxListLayout> Localizer { get; set; }

		private ChipItem[] chips;
		private string filterFormId = "hx" + Guid.NewGuid().ToString("N");
		private HxFilterForm<TFilterModel> filterForm;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>((FilterTemplate is null) || (FilterModel is not null), $"{nameof(HxListLayout)} requires {nameof(FilterModel)} to be set if {nameof(FilterTemplate)}  is used.");
		}

		private void HandleChipUpdated(ChipItem[] chips)
		{
			this.chips = chips;
		}

		private async Task HandleChipRemoveClick(ChipItem chipItemToRemove)
		{
			await filterForm.RemoveChipAsync(chipItemToRemove);
		}

		private async Task HandleFilterFormModelChanged(TFilterModel newFilterModel)
		{
			FilterModel = newFilterModel;
			await FilterModelChanged.InvokeAsync(newFilterModel);
			FilterDrawerOpen = false;
		}
	}
}
