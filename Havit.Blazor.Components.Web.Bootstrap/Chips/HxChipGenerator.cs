namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxChipGenerator : ComponentBase, IHxChipGenerator, IDisposable
	{
		[CascadingParameter(Name = HxFilterForm<object>.ChipGeneratorRegistrationCascadingValueName)] public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public Action<object> ChipRemoveAction { get; set; }

		/// <inheritdoc cref="ComponentBase.OnInitialized" />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChipGeneratorsRegistration?.Register(this);
		}

		IEnumerable<ChipItem> IHxChipGenerator.GetChips()
		{
			yield return new ChipItem
			{
				ChipTemplate = ChildContent,
				Removable = ChipRemoveAction != default,
				RemoveAction = ChipRemoveAction
			};
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ChipGeneratorsRegistration?.Unregister(this);
			}
		}
	}
}
