using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{	
	public sealed class HxFilter : ComponentBase, IHxFilter, IHxChipGenerator, IDisposable
	{
		[CascadingParameter(Name = HxFilterView<object>.FilterRegistrationCascadingValueName)]
		public CollectionRegistration<IHxFilter> FiltersRegistration { get; set; }

		[CascadingParameter(Name = HxFilterView<object>.ChipGeneratorRegistrationCascadingValueName)]
		public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		[Parameter]
		public string Label { get; set; }

		[Parameter]
		public RenderFragment LabelTemplate { get; set; }

		[Parameter]
		public RenderFragment FilterTemplate { get; set; }

		[Parameter]
		public RenderFragment ChipTemplate { get; set; } // nebo jinak, uvidíme, jak s remove callbackem		

		protected override void OnInitialized()
		{
			base.OnInitialized();

			FiltersRegistration.Register(this);
			ChipGeneratorsRegistration.Register(this);
		}

		public void Dispose()
		{
			FiltersRegistration.Unregister(this);			
			ChipGeneratorsRegistration.Unregister(this);
		}

		public RenderFragment GetLabelTemplate()
		{
			return (RenderTreeBuilder builder) =>
			{
				if (!String.IsNullOrEmpty(Label))
				{
					builder.AddContent(0, Label);
				}

				if (LabelTemplate != null)
				{
					builder.AddContent(1, LabelTemplate);
				}
			};
		}

		public RenderFragment GetFilterTemplate()
		{
			return FilterTemplate;
		}

		public IEnumerable<Chip> GetChips()
		{
			if (ChipTemplate != null)
			{
				yield return new Chip
				{
					ChipTemplate = this.ChipTemplate,
					RemoveCallback = null // TODO
				};
			}
		}
	}
}
