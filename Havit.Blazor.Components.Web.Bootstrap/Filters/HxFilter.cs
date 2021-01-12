using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public sealed class HxFilter : ComponentBase, IHxFilter, IHxChipGenerator, IDisposable
	{
		[CascadingParameter(Name = HxFilterView.FilterRegistrationCascadingValueName)] public CollectionRegistration<IHxFilter> FiltersRegistration { get; set; }

		[CascadingParameter(Name = HxFilterView.ChipGeneratorRegistrationCascadingValueName)] public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		[Parameter] public string Label { get; set; }

		[Parameter] public RenderFragment LabelTemplate { get; set; }

		[Parameter] public RenderFragment FilterTemplate { get; set; }

		[Parameter] public RenderFragment ChipTemplate { get; set; } // nebo jinak, uvidíme, jak s remove callbackem		

		RenderedEventHandler IRenderNotificationComponent.Rendered { get; set; }

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
			return RenderFragmentBuilder.CreateFrom(Label, LabelTemplate);
		}

		public RenderFragment GetFilterTemplate()
		{
			return FilterTemplate;
		}

		public IEnumerable<ChipItem> GetChips()
		{
			if (ChipTemplate != null)
			{
				yield return new ChipItem
				{
					ChipTemplate = this.ChipTemplate,
					RemoveCallback = null // TODO
				};
			}
		}

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);

			((IRenderNotificationComponent)this).Rendered?.Invoke(this, firstRender);
		}
	}
}
