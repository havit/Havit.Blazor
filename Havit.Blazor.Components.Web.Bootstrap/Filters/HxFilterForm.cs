using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Edit form derived from HxModelEditForm with support for chip generators.
	/// </summary>
	public class HxFilterForm<TModel> : HxModelEditForm<TModel>
	{
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		[Parameter] public EventCallback<ChipItem[]> OnChipsUpdated { get; set; }

		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;
		private bool isDisposed = false;
		private ChipItem chipToRemoveAfterRender;
		private bool notifyChipsUpdatedAfterRender;

		public HxFilterForm()
		{
			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators, null, () => isDisposed);
		}

		protected override void ModelSet()
		{
			base.ModelSet();
			notifyChipsUpdatedAfterRender = true;
		}

		public override async Task UpdateModelAsync()
		{
			await NotifyChipsUpdated();
			await base.UpdateModelAsync();
		}

		private async Task NotifyChipsUpdated()
		{
			var chips = await GetChipsAsync();
			await OnChipsUpdated.InvokeAsync(chips);
		}

		private async Task<ChipItem[]> GetChipsAsync()
		{
			List<ChipItem> result = new List<ChipItem>();

			foreach (IHxChipGenerator chipGenerator in chipGenerators.ToArray())
			{
				result.AddRange(await chipGenerator.GetChipsAsync());
			}

			return result.ToArray();
		}

		/// <summary>
		/// Tries to remove chip.
		/// Execution is postponed to OnAfterRender, so this method cannot have a return value.
		/// </summary>
		public void RemoveChip(ChipItem chipToRemove)
		{
			// starts to edit the Model (the clone, to be prcise)
			ModelInEdit = CloneModel(Model);
			// notify we need to remove chip in OnAfterRender
			chipToRemoveAfterRender = chipToRemove;
			StateHasChanged();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			// if we chould remove chip (we have a Model clone from it should be removed)
			if (chipToRemoveAfterRender != null)
			{
				// Find the chip generator which is responsible for the chip and removes it.
				// It changes the value in the model.
				foreach (IHxChipGenerator chipGenerator in this.chipGenerators.ToArray())
				{
					if (await chipGenerator.TryRemoveChipAsync(chipToRemoveAfterRender))
					{
						break; // shortcut for performance
					}
				}

				chipToRemoveAfterRender = null;

				// Notify the model was changed.
				await UpdateModelAsync();
			}

			if (notifyChipsUpdatedAfterRender)
			{
				notifyChipsUpdatedAfterRender = false;
				await NotifyChipsUpdated();
			}
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<CascadingValue<CollectionRegistration<IHxChipGenerator>>>(0);
			builder.AddAttribute(1, nameof(CascadingValue<CollectionRegistration<IHxChipGenerator>>.Name), ChipGeneratorRegistrationCascadingValueName);
			builder.AddAttribute(2, nameof(CascadingValue<CollectionRegistration<IHxChipGenerator>>.Value), chipGeneratorsRegistration);
			builder.AddAttribute(3, nameof(CascadingValue<CollectionRegistration<IHxChipGenerator>>.IsFixed), true);
			builder.AddAttribute(4, nameof(CascadingValue<CollectionRegistration<IHxChipGenerator>>.ChildContent), (RenderFragment)base.BuildRenderTree);
			builder.CloseComponent();
		}

		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
