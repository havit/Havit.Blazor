using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class FilterContext<TModel> : ComponentBase
	{
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		[Parameter] public TModel Model { get; set; }
		[Parameter] public EventCallback<TModel> ModelChanged { get; set; }

		[Parameter] public EventCallback<ChipItem[]> OnChipsUpdated { get; set; }

		[Parameter] public RenderFragment<TModel> ChildContent { get; set; }
		[CascadingParameter] public EditContext EditContext { get; set; }

		private TModel modelInEdit;
		private TModel modelInEditStash;
		private TModel previousModel;
		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;
		private bool isDisposed = false;
		private ChipItem chipToRemoveAfterRender;

		public FilterContext()
		{
			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators, null, () => isDisposed/*, HandleChipGeneratorsChanged, HandleChipGeneratorsChanged*/);
		}

		//private void HandleChipGeneratorsChanged(IHxChipGenerator chipGenerator)
		//{
		//	//_ = InvokeAsync(async () => await OnChipsUpdated.InvokeAsync());
		//}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			if (!EqualityComparer<TModel>.Default.Equals(previousModel, Model))
			{
				modelInEdit = (TModel)((ICloneable)Model).Clone(); // TODO: strategie klonování
				previousModel = Model;
			}

			if (EditContext != null)
			{
				throw new InvalidOperationException($"{nameof(FilterContext<object>)} cannot be nested in {nameof(EditForm)}.");
			}
		}

		public async Task UpdateModelAsync()
		{
			Model = modelInEdit;
			await ModelChanged.InvokeAsync(modelInEdit);
			var chips = await GetChipsAsync();
			await OnChipsUpdated.InvokeAsync(chips);

			if (!EqualityComparer<TModel>.Default.Equals(modelInEditStash, default))
			{
				modelInEdit = modelInEditStash;
				modelInEditStash = default;
			}
			else
			{
				modelInEdit = (TModel)((ICloneable)Model).Clone(); // TODO: strategie klonování
			}
			previousModel = Model;
			StateHasChanged(); // voláno i z onafterrender
		}

		// TODO: Extrakt společně s modelem??? Nevím...
		private async Task<ChipItem[]> GetChipsAsync()
		{
			List<ChipItem> result = new List<ChipItem>();
		
			foreach (IHxChipGenerator chipGenerator in chipGenerators)
			{
				result.AddRange(await chipGenerator.GetChipsAsync());
			}

			return result.ToArray();
		}

		public void RemoveChip(ChipItem chipToRemove)
		{
			modelInEditStash = modelInEdit;
			modelInEdit = (TModel)((ICloneable)Model).Clone(); // TODO: strategie klonování
			chipToRemoveAfterRender = chipToRemove;
			StateHasChanged();
		}

		protected override bool ShouldRender()
		{
			return base.ShouldRender();
		}



		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (chipToRemoveAfterRender != null)
			{
				foreach (IHxChipGenerator chipGenerator in this.chipGenerators)
				{
					if (await chipGenerator.TryRemoveChipAsync(chipToRemoveAfterRender))
					{
						break;
					}
				}
				chipToRemoveAfterRender = null;
				//StateHasChanged();
				await UpdateModelAsync();
			}			
		}

		public void Dispose()
		{
			isDisposed = true;
		}

	}
}
