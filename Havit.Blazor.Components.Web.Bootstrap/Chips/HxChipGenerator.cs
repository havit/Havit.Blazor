namespace Havit.Blazor.Components.Web.Bootstrap;

public class HxChipGenerator : ComponentBase, IHxChipGenerator, IAsyncDisposable
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

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async Task DisposeAsyncCore()
	{
		if (ChipGeneratorsRegistration != null)
		{
			await ChipGeneratorsRegistration.UnregisterAsync(this);
		}
	}
}
