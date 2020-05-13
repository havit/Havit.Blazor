using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Je dobře pojmenovat to filter view, když jde vlastně o editaci?
	public partial class HxFilterView<TFilterType>
	// where TFilterType : class - toto nelze zapsat
	{
		public const string FilterRegistrationCascadingValueName = "FiltersRegistration";
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		// pokud bude parametrem, pozor, že hodnotu měníme (OnParameterSet ji přepíše).
#pragma warning disable CS0414
		private bool isExpanded = false; //https://github.com/dotnet/aspnetcore/issues/20137
#pragma warning restore CS0414

		[Parameter]
		// TODO: Pojmenování? Filter? Value?
		public TFilterType Data { get; set; }

		[Parameter]
		public EventCallback<EventArgs> OnChange { get; set; }

		[Parameter]
		public RenderFragment<TFilterType> Criteria { get; set; }

		private List<IHxFilter> filters;
		private CollectionRegistration<IHxFilter> filtersRegistration;
		
		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;

		public HxFilterView()
		{
			filters = new List<IHxFilter>();
			filtersRegistration = new CollectionRegistration<IHxFilter>(filters);

			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators);
	}


	protected void ExpandFilterClick()
		{
			isExpanded = true;
		}

		protected void CollapseFilterClick()
		{
			isExpanded = false;
		}

		protected Task ApplyFilterClick()
		{
			// TODO: Notify...
			return Task.CompletedTask;
		}
	}
}
