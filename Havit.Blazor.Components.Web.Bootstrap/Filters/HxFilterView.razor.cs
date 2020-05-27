using Havit.Blazor.Components.Web.Bootstrap.Chips;
using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	// TODO: Je dobře pojmenovat to filter view, když jde vlastně o editaci? FiltrLayout?
	public partial class HxFilterView : IDisposable
	{
		public const string FilterRegistrationCascadingValueName = "FiltersRegistration";
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		[CascadingParameter(Name = "FilterDrawerOpen")] // TODO: Konstanta
		public bool IsExpanded { get; set; }

		[Parameter]
		public EventCallback ApplyFilterRequested { get; set; }

		[Parameter]
		public RenderFragment Criteria { get; set; }

		private List<IHxFilter> filters;
		private CollectionRegistration<IHxFilter> filtersRegistration;

		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;
		
		private bool isDisposed = false;

		public HxFilterView()
		{
			filters = new List<IHxFilter>();
			filtersRegistration = new CollectionRegistration<IHxFilter>(filters, this.StateHasChanged, () => isDisposed);

			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators, this.StateHasChanged, () => isDisposed);
		}

		protected async Task ApplyFilterClick()
		{
			await ApplyFilterRequested.InvokeAsync(null);
		}

		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
