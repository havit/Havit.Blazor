using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Provides a unified layout for data presentation components and associated filtering controls.<br/>
/// This component orchestrates the interaction between filter controls and the data presentation component.
/// The data list is typically implemented using a <see cref="HxGrid{TItem}"/> component. Filters are displayed
/// in a <see cref="HxOffcanvas"/> component, while filter values are shown as <see cref="HxChipList"/>.
/// Additionally, it supports predefined named views for quick switching between different filter configurations
/// and other features such as a title, search box, and commands.
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxListLayout">https://havit.blazor.eu/components/HxListLayout</see>
/// </summary>
public partial class HxListLayout<TFilterModel>
{
	/// <summary>
	/// Returns the <see cref="HxMessageBox"/> defaults.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ListLayoutSettings GetDefaults() => HxListLayout.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxListLayout.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ListLayoutSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ListLayoutSettings GetSettings() => Settings;

	/// <summary>
	/// Title of the component.
	/// If <see cref="TitleFromNamedView"/> is <c>true</c> and <see cref="SelectedNamedView"/> is not <c>null</c>, the component's title displays the name of the currently selected Named View.
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Title of the component (in form of RenderFragment).
	/// If <see cref="TitleFromNamedView"/> is <c>true</c> and <see cref="SelectedNamedView"/> is not <c>null</c>, the component's title displays the name of the currently selected Named View.
	/// </summary>
	[Parameter] public RenderFragment TitleTemplate { get; set; }

	/// <summary>
	/// Represents the collection of Named Views available for selection. 
	/// Each Named View defines a pre-set filter configuration that can be applied to the data.
	/// </summary>
	/// <remarks>
	/// Named Views provide a convenient way for users to quickly apply commonly used filters to the data set.
	/// Ensure that each Named View in the collection has a unique name which accurately describes its filter criteria.
	/// </remarks>
	[Parameter] public IEnumerable<NamedView<TFilterModel>> NamedViews { get; set; }

	/// <summary>
	/// Selected named view (highlighted in the list with <c>.active</c> CSS class).
	/// </summary>
	[Parameter] public NamedView<TFilterModel> SelectedNamedView { get; set; }
	[Parameter] public EventCallback<NamedView<TFilterModel>> SelectedNamedViewChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="SelectedNamedViewChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeSelectedNamedViewChangedAsync(NamedView<TFilterModel> itemSelected) => SelectedNamedViewChanged.InvokeAsync(itemSelected);

	/// <summary>
	/// Indicates whether the name of the selected Named View (<see cref="SelectedNamedView"/>) is automatically used as title.
	/// If <c>true</c>, the component's title changes to match the name of the currently selected Named View.
	/// Useful for dynamic title updates based on user selections from predefined views.
	/// The default value is <c>true</c>.
	/// </summary>
	/// <remarks>
	/// This update occurs upon the selection of a new Named View. It allows the Title to reflect the
	/// current data filtering context provided by the Named Views, enhancing user understanding of the active filter.
	/// </remarks>
	[Parameter] public bool TitleFromNamedView { get; set; } = true;

	[Parameter] public RenderFragment SearchTemplate { get; set; }

	[Parameter] public RenderFragment<TFilterModel> FilterTemplate { get; set; }

	[Parameter] public TFilterModel FilterModel { get; set; }
	[Parameter] public EventCallback<TFilterModel> FilterModelChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="FilterModelChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeFilterModelChangedAsync(TFilterModel newFilterModel) => FilterModelChanged.InvokeAsync(newFilterModel);

	[Parameter] public RenderFragment DataTemplate { get; set; }

	[Parameter] public RenderFragment DetailTemplate { get; set; }

	[Parameter] public RenderFragment CommandsTemplate { get; set; }

	/// <summary>
	/// Settings for the wrapping <see cref="HxCard"/>.
	/// </summary>
	[Parameter] public CardSettings CardSettings { get; set; }
	protected CardSettings CardSettingsEffective => CardSettings ?? GetSettings()?.CardSettings ?? GetDefaults().CardSettings ?? throw new InvalidOperationException(nameof(CardSettings) + " default for " + nameof(HxListLayout) + " has to be set.");

	/// <summary>
	/// Settings for the <see cref="HxButton"/> opening the filtering offcanvas.
	/// </summary>
	[Parameter] public ButtonSettings FilterOpenButtonSettings { get; set; }
	protected ButtonSettings FilterOpenButtonSettingsEffective => FilterOpenButtonSettings ?? GetSettings()?.FilterOpenButtonSettings ?? GetDefaults().FilterOpenButtonSettings ?? throw new InvalidOperationException(nameof(FilterOpenButtonSettings) + " default for " + nameof(HxListLayout) + " has to be set.");

	/// <summary>
	/// Settings for the <see cref="HxButton"/> submitting the filter.
	/// </summary>
	[Parameter] public ButtonSettings FilterSubmitButtonSettings { get; set; }
	protected ButtonSettings FilterSubmitButtonSettingsEffective => FilterSubmitButtonSettings ?? GetSettings()?.FilterSubmitButtonSettings ?? GetDefaults().FilterSubmitButtonSettings ?? throw new InvalidOperationException(nameof(FilterSubmitButtonSettings) + " default for " + nameof(HxListLayout) + " has to be set.");

	/// <summary>
	/// Settings for the <see cref="HxOffcanvas"/> with the filter.
	/// </summary>
	[Parameter] public OffcanvasSettings FilterOffcanvasSettings { get; set; }
	protected OffcanvasSettings FilterOffcanvasSettingsEffective => FilterOffcanvasSettings ?? GetSettings()?.FilterOffcanvasSettings ?? GetDefaults().FilterOffcanvasSettings ?? throw new InvalidOperationException(nameof(FilterOffcanvasSettings) + " default for " + nameof(HxListLayout) + " has to be set.");

	/// <summary>
	/// Additional CSS classes for the wrapping <c>div</c>.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	[Inject] protected IStringLocalizer<HxListLayout> Localizer { get; set; }

	private ChipItem[] _chips;
	private string _filterFormId = "hx" + Guid.NewGuid().ToString("N");
	private HxFilterForm<TFilterModel> _filterForm;
	private HxOffcanvas _filterOffcanvasComponent;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		Contract.Requires<InvalidOperationException>((FilterTemplate is null) || (FilterModel is not null), $"{nameof(HxListLayout)} requires {nameof(FilterModel)} to be set if {nameof(FilterTemplate)} is used.");
	}

	private void HandleChipUpdated(ChipItem[] chips)
	{
		_chips = chips;
	}

	private async Task HandleChipRemoveClick(ChipItem chipItemToRemove)
	{
		await _filterForm.RemoveChipAsync(chipItemToRemove);
	}

	private async Task HandleFilterFormModelChanged(TFilterModel newFilterModel)
	{
		// We want the offcanvas to close before the filter model is updated (= before the data start reloading).
		await _filterOffcanvasComponent.HideAsync();

		FilterModel = newFilterModel;
		await InvokeFilterModelChangedAsync(newFilterModel);
	}

	private async Task HandleNamedViewClickAsync(NamedView<TFilterModel> namedView)
	{
		SelectedNamedView = namedView;
		await InvokeSelectedNamedViewChangedAsync(namedView);

		TFilterModel newFilterModel = namedView.CreateFilterModel();
		if (newFilterModel != null)
		{
			FilterModel = newFilterModel;
			await InvokeFilterModelChangedAsync(newFilterModel);
		}
	}

	/// <summary>
	/// Opens the filter offcanvas.
	/// </summary>
	public async Task ShowFilterOffcanvasAsync()
	{
		await _filterOffcanvasComponent.ShowAsync();
	}

	/// <summary>
	/// Closes the filter offcanvas.
	/// </summary>
	public async Task HideFilterOffcanvasAsync()
	{
		await _filterOffcanvasComponent.HideAsync();
	}
}
