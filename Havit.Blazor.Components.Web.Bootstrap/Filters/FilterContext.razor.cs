using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Rename to FilterForm
	// TODO: Měl by dědit z EditForm? Nebo zapouzdřit?
	public partial class FilterContext<TModel> : ComponentBase
	{
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		[Parameter] public TModel Model { get; set; }
		[Parameter] public EventCallback<TModel> ModelChanged { get; set; }

		[Parameter] public EventCallback<ChipItem[]> OnChipsUpdated { get; set; }

		[Parameter] public RenderFragment<TModel> ChildContent { get; set; }
		[CascadingParameter] public EditContext EditContext { get; set; }
		
		[Inject] public ILogger<FilterContext<TModel>> Logger { get; set; }

		private TModel modelInEdit;
		private TModel modelInEditStash;
		private TModel previousModel;
		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;
		private bool isDisposed = false;
		private ChipItem chipToRemoveAfterRender;
		private bool notifyChipsUpdatedAfterRender;

		public FilterContext()
		{
			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators, null, () => isDisposed);
		}

		protected override void OnParametersSet()
		{
			Logger.LogDebug("OnParametersSet");
			base.OnParametersSet();
			if (!EqualityComparer<TModel>.Default.Equals(previousModel, Model))
			{
				Logger.LogDebug("OnParametersSet - Cloning model");

				// we are going to let user edit a clone of the model
				modelInEdit = CloneModel(Model);
				previousModel = Model;

				// TO show chips from initial model value
				notifyChipsUpdatedAfterRender = true;
			}

			if (EditContext != null)
			{
				throw new InvalidOperationException($"{nameof(FilterContext<object>)} cannot be nested in {nameof(EditForm)}.");
			}
		}

		public async Task UpdateModelAsync()
		{
			Logger.LogDebug("UpdateModelAsync");

			// TODO: EditForm OnValidSubmit
			Logger.LogDebug("UpdateModelAsync - ModelChanged");

			await NotifyChipsUpdated();
			Model = modelInEdit; // TODO: ModelInEdit - zveřejnit?
			previousModel = Model; // to suppress cloning Model in OnParametersSet, must be before ModelChanged is invoked!
			await ModelChanged.InvokeAsync(modelInEdit);

			Logger.LogDebug("UpdateModelAsync - NotifyChipsUpdated");
			//await NotifyChipsUpdated();

			// if there is a stashed modelInEdit (from RemoveChipt), let's get it back			
			if (!EqualityComparer<TModel>.Default.Equals(modelInEditStash, default))
			{
				Logger.LogDebug("UpdateModelAsync - restore model from stash");
				modelInEdit = modelInEditStash;
				modelInEditStash = default;
			}
			else
			{
				Logger.LogDebug("UpdateModelAsync - cloning model");
				modelInEdit = CloneModel(Model);
			}
				StateHasChanged();
		}

		private async Task NotifyChipsUpdated()
		{
			Logger.LogDebug("NotifyChipsUpdated");
			var chips = await GetChipsAsync();
			await OnChipsUpdated.InvokeAsync(chips);
		}

		private async Task<ChipItem[]> GetChipsAsync()
		{
			Logger.LogDebug("GetChipsAsync");
			List<ChipItem> result = new List<ChipItem>();
			
			foreach (IHxChipGenerator chipGenerator in chipGenerators.ToArray())
			{
				result.AddRange(await chipGenerator.GetChipsAsync());
			}

			Logger.LogDebug("GetChipsAsync - returning result");
			return result.ToArray();
		}

		/// <summary>
		/// Tries to remove chip.
		/// Execution is postponed to OnAfterRender, so this method cannot have a return value.
		/// </summary>
		public void RemoveChip(ChipItem chipToRemove)
		{
			Logger.LogDebug("RemoveChip");
			// Stashes the current modelInEdit. We will modify the modelInEdit on the next line. This stashing allows us to get the modelInEdit back. And this returns back users valid inputs.
//			modelInEditStash = modelInEdit;
			// starts to edit the Model (the clone, to be prcise)
			modelInEdit = CloneModel(Model);
			// notify we need to remove chip in OnAfterRender
			chipToRemoveAfterRender = chipToRemove;
			StateHasChanged();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			Logger.LogDebug("OnAfterRenderAsync");
			await base.OnAfterRenderAsync(firstRender);

			// if we chould remove chip (we have a Model clone from it should be removed)
			if (chipToRemoveAfterRender != null)
			{
				// Tind the chip generator which is responsible for the chip and removes it.
				// It changes the value in the model.
				Logger.LogDebug("OnAfterRenderAsync - removing chip");
				foreach (IHxChipGenerator chipGenerator in this.chipGenerators.ToArray())
				{
					if (await chipGenerator.TryRemoveChipAsync(chipToRemoveAfterRender))
					{
						break; // shortcut for performance
					}
				}

				chipToRemoveAfterRender = null;
				
				// Notify the model was changed.
				Logger.LogDebug("OnAfterRenderAsync - updating model");
				await UpdateModelAsync();
			}

			if (notifyChipsUpdatedAfterRender)
			{
				notifyChipsUpdatedAfterRender = false;
				Logger.LogDebug("OnAfterRenderAsync - NotifyChipsUpdated");
				await NotifyChipsUpdated();
			}
		}

		internal static TModel CloneModel(TModel modelToClone)
		{
			return (TModel)((ICloneable)modelToClone).Clone(); // TODO: strategie klonování
		}

		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
