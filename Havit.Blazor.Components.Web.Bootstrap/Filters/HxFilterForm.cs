using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Edit form derived from HxModelEditForm with support for chip generators.
	/// </summary>
	public class HxFilterForm<TModel> : HxModelEditForm<TModel>, IDisposable
	{
		public const string ChipGeneratorRegistrationCascadingValueName = "ChipGeneratorsRegistration";

		[Parameter] public EventCallback<ChipItem[]> OnChipsUpdated { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnChipsUpdated"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnChipsUpdatedAsync(ChipItem[] newChips) => OnChipsUpdated.InvokeAsync(newChips);

		private List<IHxChipGenerator> chipGenerators;
		private CollectionRegistration<IHxChipGenerator> chipGeneratorsRegistration;
		private bool isDisposed = false;
		private bool notifyChipsUpdatedAfterRender;

		public HxFilterForm()
		{
			chipGenerators = new List<IHxChipGenerator>();
			chipGeneratorsRegistration = new CollectionRegistration<IHxChipGenerator>(chipGenerators, null, () => isDisposed);
		}

		protected override void OnModelSet()
		{
			base.OnModelSet();
			notifyChipsUpdatedAfterRender = true;
		}

		public override async Task UpdateModelAsync()
		{
			await NotifyChipsUpdatedAsync();
			await UpdateModelWithoutChipUpdateAsync();
		}

		private async Task UpdateModelWithoutChipUpdateAsync()
		{
			await base.UpdateModelAsync(); // call base class!
		}

		private async Task NotifyChipsUpdatedAsync()
		{
			var chips = GetChips();

			// Generated chips are connected to ModelInEdit.
			// When the model is changed, the chips are updated.
			// As a solution we create a new ModelInEdit so the one used for chips in not changed anymore so the chips do not update the content.
			ModelInEdit = CloneModel(ModelInEdit);
			StateHasChanged(); // also called from OnAfterRender

			await InvokeOnChipsUpdatedAsync(chips);
		}

		private ChipItem[] GetChips()
		{
			List<ChipItem> result = new List<ChipItem>();

			foreach (IHxChipGenerator chipGenerator in chipGenerators.ToArray())
			{
				result.AddRange(chipGenerator.GetChips());
			}

			return result.ToArray();
		}

		/// <summary>
		/// Tries to remove chip.
		/// Execution is postponed to OnAfterRender, so this method cannot have a return value.
		/// </summary>
		public Task RemoveChipAsync(ChipItem chipToRemove)
		{
			// starts to edit the Model (the clone, to be precise)
			TModel newModelInEdit = CloneModel(Model);
			chipToRemove.RemoveAction(newModelInEdit); // process the chip removal
			ModelInEdit = newModelInEdit; // place the model to the edit

			// propagate the model in edit to the Model and notify model changed
			// if used with await the chip is removed from UI much later
			_ = InvokeAsync(UpdateModelWithoutChipUpdateAsync);
			notifyChipsUpdatedAfterRender = true; // notify the chips update after the model is "rendered"

			return Task.CompletedTask;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (notifyChipsUpdatedAfterRender)
			{
				notifyChipsUpdatedAfterRender = false;
				await NotifyChipsUpdatedAsync();
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
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			isDisposed = true;
		}
	}
}
