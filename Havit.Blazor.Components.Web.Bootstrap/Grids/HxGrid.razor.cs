using Havit.Collections;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid to display tabular data from data source. Includes support for client-side and server-side paging &amp; sorting (or virtualized scrolling as needed).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxGrid">https://havit.blazor.eu/components/HxGrid</see>
/// </summary>
/// <typeparam name="TItem">Type of row data item.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public partial class HxGrid<TItem> : ComponentBase, IDisposable
{
	/// <summary>
	/// Represents a constant name for ColumnsRegistration cascading value.
	/// </summary>
	public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

	/// <summary>
	/// Specifies the grid settings. Overrides default settings in <see cref="HxGrid.Defaults"/> and can be further overridden by individual parameters.
	/// </summary>
	[Parameter] public GridSettings Settings { get; set; }

	/// <summary>
	/// Provides the current settings of the grid. Override in derived classes to return a specific settings type or additional configurations.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual GridSettings GetSettings() => Settings;

	/// <summary>
	/// Data provider delegate for the grid. The data provider is responsible for fetching items to be rendered in the grid.
	/// It must always return an instance of <see cref="GridDataProviderResult{TItem}"/> and cannot return null.
	/// </summary>
	/// <remarks>
	/// The delegate is invoked to fetch data based on the current grid state, including pagination and sorting parameters.
	/// </remarks>
	[Parameter, EditorRequired] public GridDataProviderDelegate<TItem> DataProvider { get; set; }

	/// <summary>
	/// Enables or disables single item selection by row click. Can be used alongside multi-selection.
	/// Defaults to <c>true</c>.
	/// </summary>
	[Parameter] public bool SelectionEnabled { get; set; } = true;

	/// <summary>
	/// Enables or disables multi-item selection using checkboxes in the first column.
	/// Can be used with single selection.
	/// Defaults to <c>false</c>.
	/// </summary>
	[Parameter] public bool MultiSelectionEnabled { get; set; } = false;

	/// <summary>
	/// Grid columns.
	/// </summary>
	[Parameter, EditorRequired] public RenderFragment Columns { get; set; }

	/// <summary>
	/// Defines a template for the initial data loading phase.
	/// This template is not used when loading data for sorting or paging operations.
	/// </summary>
	[Parameter] public RenderFragment LoadingDataTemplate { get; set; }

	/// <summary>
	/// Template for rendering when the data source is empty but not null.
	/// </summary>
	[Parameter] public RenderFragment EmptyDataTemplate { get; set; }

	/// <summary>
	/// Template for rendering custom pagination.
	/// </summary>
	[Parameter] public RenderFragment<GridPaginationTemplateContext> PaginationTemplate { get; set; }

	/// <summary>
	/// Template for the "load more" button (or other UI element).
	/// </summary>
	[Parameter] public RenderFragment<GridLoadMoreTemplateContext> LoadMoreTemplate { get; set; }

	/// <summary>
	/// Represents the currently selected data item in the grid for data binding and state synchronization. Changes trigger <see cref="SelectedDataItemChanged"/>.
	/// </summary>
	[Parameter] public TItem SelectedDataItem { get; set; }

	/// <summary>
	/// Event that fires when the <see cref="SelectedDataItem"/> property changes. This event is intended for data binding and state synchronization.
	/// </summary>
	[Parameter] public EventCallback<TItem> SelectedDataItemChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="SelectedDataItemChanged"/> event asynchronously. This method can be overridden in derived components to intercept the event and provide custom logic before or after the event is triggered.
	/// </summary>
	protected virtual Task InvokeSelectedDataItemChangedAsync(TItem selectedDataItem) => SelectedDataItemChanged.InvokeAsync(selectedDataItem);

	/// <summary>
	/// Represents the collection of currently selected data items in the grid, primarily for data binding and state management in multi-selection scenarios.
	/// </summary>
	[Parameter] public HashSet<TItem> SelectedDataItems { get; set; }

	/// <summary>
	/// Event that fires when the collection of selected data items changes. This is particularly relevant in multi-selection scenarios. It is intended for data binding and state synchronization.
	/// </summary>
	[Parameter] public EventCallback<HashSet<TItem>> SelectedDataItemsChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="SelectedDataItemsChanged"/> event. This method can be overridden in derived components to implement custom logic before or after the event is triggered.
	/// </summary>
	protected virtual Task InvokeSelectedDataItemsChangedAsync(HashSet<TItem> selectedDataItems) => SelectedDataItemsChanged.InvokeAsync(selectedDataItems);

	/// <summary>
	/// Gets or sets a value indicating whether the current selection (either <see cref="SelectedDataItem"/> for single selection
	/// or <see cref="SelectedDataItems"/> for multiple selection) should be preserved during data operations, such as paging, sorting, filtering,
	/// or manual invocation of <see cref="RefreshDataAsync"/>.<br />
	/// Default value is <c>false</c> (can be set by using <c>HxGrid.Defaults</c>).
	/// </summary>
	/// <remarks>
	/// This setting ensures that the selection remains intact during operations that refresh or modify the displayed data in the grid.
	/// Note that preserving the selection requires that the underlying data items can still be matched in the updated dataset (e.g., by <c>item1.Equals(item2)</c>).
	/// </remarks>
	[Parameter] public bool? PreserveSelection { get; set; }
	protected bool PreserveSelectionEffective => PreserveSelection ?? GetSettings()?.PreserveSelection ?? GetDefaults().PreserveSelection ?? throw new InvalidOperationException(nameof(PreserveSelection) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// The strategy for how data items are displayed and loaded into the grid. Supported modes include pagination, load more, and infinite scroll.
	/// </summary>
	[Parameter] public GridContentNavigationMode? ContentNavigationMode { get; set; }
	protected GridContentNavigationMode ContentNavigationModeEffective => ContentNavigationMode ?? GetSettings()?.ContentNavigationMode ?? GetDefaults().ContentNavigationMode ?? throw new InvalidOperationException(nameof(ContentNavigationMode) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// The number of items to display per page. Applicable for grid modes such as pagination and load more. Set to 0 to disable paging.
	/// </summary>
	[Parameter] public int? PageSize { get; set; }
	protected int PageSizeEffective => PageSize ?? GetSettings()?.PageSize ?? GetDefaults().PageSize ?? throw new InvalidOperationException(nameof(PageSize) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Determines whether the grid footer is rendered when the grid's data source is empty. The default value is <c>false</c>.
	/// </summary>
	[Parameter] public bool? ShowFooterWhenEmptyData { get; set; }
	protected bool ShowFooterWhenEmptyDataEffective => ShowFooterWhenEmptyData ?? GetSettings()?.ShowFooterWhenEmptyData ?? GetDefaults().ShowFooterWhenEmptyData ?? throw new InvalidOperationException(nameof(ShowFooterWhenEmptyData) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Pager settings.
	/// </summary>
	[Parameter] public PagerSettings PagerSettings { get; set; }
	protected PagerSettings PagerSettingsEffective => PagerSettings ?? GetSettings()?.PagerSettings ?? GetDefaults().PagerSettings;

	/// <summary>
	/// The text for the "Load more" button, used in the <see cref="GridContentNavigationMode.LoadMore"/> navigation mode. The default text is obtained from localization resources.
	/// </summary>
	[Parameter] public string LoadMoreButtonText { get; set; }

	/// <summary>
	/// Configuration for the "Load more" button, including appearance and behavior settings. Relevant in grid modes that use a "Load more" button for data navigation.
	/// </summary>
	[Parameter] public ButtonSettings LoadMoreButtonSettings { get; set; }
	protected ButtonSettings LoadMoreButtonSettingsEffective => LoadMoreButtonSettings ?? GetSettings()?.LoadMoreButtonSettings ?? GetDefaults().LoadMoreButtonSettings ?? throw new InvalidOperationException(nameof(LoadMoreButtonSettings) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Gets or sets the current state of the grid, including pagination and sorting information. This state can be used to restore the grid to a specific configuration or to synchronize it with external state management systems.
	/// </summary>
	[Parameter] public GridUserState CurrentUserState { get; set; } = new GridUserState();

	/// <summary>
	/// Event that fires when the <see cref="CurrentUserState"/> property changes. This event can be used to react to changes in the grid's state, such as sorting or pagination adjustments.
	/// </summary>
	[Parameter] public EventCallback<GridUserState> CurrentUserStateChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="CurrentUserStateChanged"/> event. This method can be overridden in derived components to add custom logic before or after the state change event is triggered.
	/// </summary>
	protected virtual Task InvokeCurrentUserStateChangedAsync(GridUserState newGridUserState) => CurrentUserStateChanged.InvokeAsync(newGridUserState);

	/// <summary>
	/// Delay in milliseconds before the progress indicator is displayed. The default value is <c>300 ms</c>.
	/// </summary>
	[Parameter] public int? ProgressIndicatorDelay { get; set; }
	protected int ProgressIndicatorDelayEffective => ProgressIndicatorDelay ?? GetSettings()?.ProgressIndicatorDelay ?? GetDefaults().ProgressIndicatorDelay ?? throw new InvalidOperationException(nameof(ProgressIndicatorDelay) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Gets or sets a value indicating whether the grid is currently processing data, such as loading or refreshing items.
	/// When set to <c>null</c>, the progress state is automatically managed based on the data provider's activity.
	/// </summary>
	[Parameter] public bool? InProgress { get; set; }

	/// <summary>
	/// Custom CSS class for the <c>div</c> element that wraps the main <c>table</c> element. Excludes the <see cref="HxPager"/> which is not wrapped in this <c>div</c> element.
	/// </summary>
	[Parameter] public string TableContainerCssClass { get; set; }
	protected string TableContainerCssClassEffective => TableContainerCssClass ?? GetSettings()?.TableContainerCssClass ?? GetDefaults().TableContainerCssClass;

	/// <summary>
	/// Custom CSS class for the main <c>table</c> element of the grid. This class allows for styling and customization of the grid's appearance.
	/// </summary>
	[Parameter] public string TableCssClass { get; set; }
	protected string TableCssClassEffective => TableCssClass ?? GetSettings()?.TableCssClass ?? GetDefaults().TableCssClass;

	/// <summary>
	/// Custom CSS class for the <c>thead</c> element of the grid. This class allows for styling and customization of the grid's appearance.
	/// </summary>
	[Parameter] public string TableHeaderCssClass { get; set; }
	protected string TableHeaderCssClassEffective => TableHeaderCssClass ?? GetSettings()?.TableHeaderCssClass ?? GetDefaults().TableHeaderCssClass;

	/// <summary>
	/// Custom CSS class for the header <c>tr</c> element in the grid. Enables specific styling for the header row separate from the rest of the grid.
	/// </summary>
	[Parameter] public string HeaderRowCssClass { get; set; }
	protected string HeaderRowCssClassEffective => HeaderRowCssClass ?? GetSettings()?.HeaderRowCssClass ?? GetDefaults().HeaderRowCssClass;

	/// <summary>
	/// Custom CSS class for the data <c>tr</c> elements in the grid. This class is applied to each row of data, providing a way to customize the styling of data rows.
	/// </summary>
	[Parameter] public string ItemRowCssClass { get; set; }
	protected string ItemRowCssClassEffective => ItemRowCssClass ?? GetSettings()?.ItemRowCssClass ?? GetDefaults().ItemRowCssClass;

	/// <summary>
	/// Height of each item row, used in calculations for infinite scrolling (<see cref="GridContentNavigationMode.InfiniteScroll"/>).
	/// The default value (41px) corresponds to the typical row height in the Bootstrap 5 default theme.
	/// The row height is not applied for other navigation modes, use CSS for that.
	/// </summary>
	[Parameter] public float? ItemRowHeight { get; set; }
	protected float ItemRowHeightEffective => ItemRowHeight ?? GetSettings()?.ItemRowHeight ?? GetDefaults().ItemRowHeight ?? throw new InvalidOperationException(nameof(ItemRowHeight) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Function that defines a custom CSS class for each data <c>tr</c> element based on the item it represents.
	/// This allows for conditional styling of rows based on their data.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemRowCssClassSelector { get; set; }

	/// <summary>
	/// Optionally defines a value for @key on each rendered row. Typically this should be used to specify a
	/// unique identifier, such as a primary key value, for each data item.
	///
	/// This allows the grid to preserve the association between row elements and data items based on their
	/// unique identifiers, even when the TGridItem instances are replaced by new copies (for
	/// example, after a new query against the underlying data store).
	///
	/// If not set, the @key will be the TItem instance itself.
	/// </summary>
	/// <remarks>Inspired by QuickGrid</remarks>
	[Parameter] public Func<TItem, object> ItemKeySelector { get; set; } = x => x;

	/// <summary>
	/// A custom CSS class for the footer <c>tr</c> element in the grid. This allows styling of the grid footer independently of other grid elements.
	/// </summary>
	[Parameter] public string FooterRowCssClass { get; set; }
	protected string FooterRowCssClassEffective => FooterRowCssClass ?? GetSettings()?.FooterRowCssClass ?? GetDefaults().FooterRowCssClass;

	/// <summary>
	/// The number of placeholder rows to be rendered in the grid. Placeholders are used when loading data or when <see cref="LoadingDataTemplate" />
	/// is not set. Set to 0 to disable placeholders. Default value is 5.
	/// </summary>
	[Parameter] public int? PlaceholdersRowCount { get; set; }
	protected int PlaceholdersRowCountEffective => PlaceholdersRowCount ?? GetSettings()?.PlaceholdersRowCount ?? GetDefaults().PlaceholdersRowCount ?? throw new InvalidOperationException(nameof(PlaceholdersRowCount) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Defines the number of additional items to be rendered before and after the visible region in an infinite scrolling scenario.
	/// This helps to reduce the frequency of rendering during scrolling, though higher values increase the number of elements present in the page.
	/// Default is 3.
	/// </summary>
	[Parameter] public int? OverscanCount { get; set; }
	protected int OverscanCountEffective => OverscanCount ?? GetSettings()?.OverscanCount ?? GetDefaults().OverscanCount ?? throw new InvalidOperationException(nameof(OverscanCount) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Determines if the grid should be scrollable horizontally across different breakpoints.
	/// When set to true, the <c>table-responsive</c> class is added to the table.
	/// Default is false.
	/// </summary>
	[Parameter] public bool? Responsive { get; set; }
	protected bool ResponsiveEffective => Responsive ?? GetSettings()?.Responsive ?? GetDefaults().Responsive ?? throw new InvalidOperationException(nameof(Responsive) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Enables or disables the hover state on table rows within a <c>&lt;tbody&gt;</c>.
	/// When not set, the table is hoverable by default if selection is enabled. This property customizes the hover behavior of the grid rows.
	/// </summary>
	[Parameter] public bool? Hover { get; set; }
	protected bool? HoverEffective => Hover ?? GetSettings()?.Hover ?? GetDefaults().Hover;

	/// <summary>
	/// Adds zebra-striping to any table row within the <c>&lt;tbody&gt;</c> for better readability.
	/// Rows will have alternating background colors.
	/// Default is false.
	/// </summary>
	[Parameter] public bool? Striped { get; set; }
	protected bool StripedEffective => Striped ?? GetSettings()?.Striped ?? GetDefaults().Striped ?? throw new InvalidOperationException(nameof(Striped) + " default for " + nameof(HxGrid) + " has to be set.");

	/// <summary>
	/// Icon to indicate the ascending sort direction in the column header. This icon is displayed when a column is sorted in ascending order.
	/// </summary>
	[Parameter] public IconBase SortAscendingIcon { get; set; }
	protected IconBase SortAscendingIconEffective => SortAscendingIcon ?? GetSettings()?.SortAscendingIcon ?? GetDefaults().SortAscendingIcon;

	/// <summary>
	/// Icon to indicate the descending sort direction in the column header. This icon is shown when a column is sorted in descending order.
	/// </summary>
	[Parameter] public IconBase SortDescendingIcon { get; set; }
	protected IconBase SortDescendingIconEffective => SortDescendingIcon ?? GetSettings()?.SortDescendingIcon ?? GetDefaults().SortDescendingIcon;

	/// <summary>
	/// Defines a function that returns additional attributes for a specific <c>tr</c> element based on the item it represents.
	/// This allows for custom behavior or event handling on a per-row basis.
	/// </summary>
	/// <remarks>
	/// If both <see cref="ItemRowAdditionalAttributesSelector"/> and <see cref="ItemRowAdditionalAttributes"/> are specified,
	/// both dictionaries are combined into one.
	/// Note that there is no prevention of duplicate keys, which may result in a <see cref="System.ArgumentException"/>.
	/// </remarks>
	[Parameter] public Func<TItem, Dictionary<string, object>> ItemRowAdditionalAttributesSelector { get; set; }

	/// <summary>
	/// Provides a dictionary of additional attributes to apply to all body <c>tr</c> elements in the grid.
	/// These attributes can be used to customize the appearance or behavior of rows.
	/// </summary>
	/// <remarks>
	/// If both <see cref="ItemRowAdditionalAttributesSelector"/> and <see cref="ItemRowAdditionalAttributes"/> are specified,
	/// both dictionaries are combined into one.
	/// Note that there is no prevention of duplicate keys, which may result in a <see cref="System.ArgumentException"/>.
	/// </remarks>
	[Parameter] public Dictionary<string, object> ItemRowAdditionalAttributes { get; set; }

	/// <summary>
	/// Provides a dictionary of additional attributes to apply to the header <c>tr</c> element of the grid.
	/// This allows for custom styling or behavior of the header row.
	/// </summary>
	[Parameter] public Dictionary<string, object> HeaderRowAdditionalAttributes { get; set; }

	/// <summary>
	/// Provides a dictionary of additional attributes to apply to the footer <c>tr</c> element of the grid.
	/// This allows for custom styling or behavior of the footer row.
	/// </summary>
	[Parameter] public Dictionary<string, object> FooterRowAdditionalAttributes { get; set; }

	/// <summary>
	/// Determines the effective additional attributes for a given data row, combining both the global and per-item attributes.
	/// </summary>
	/// <param name="item">The data item for the current row.</param>
	/// <returns>A dictionary of additional attributes to apply to the row.</returns>
	/// <exception cref="System.ArgumentException">Thrown when there are duplicate keys in the combined dictionaries.</exception>
	private Dictionary<string, object> ItemRowAdditionalAttributesSelectorEffective(TItem item)
	{
		if (ItemRowAdditionalAttributesSelector == null)
		{
			return ItemRowAdditionalAttributes;
		}
		else if (ItemRowAdditionalAttributes == null)
		{
			return ItemRowAdditionalAttributesSelector(item);
		}
		else
		{
			return ItemRowAdditionalAttributes.Concat(ItemRowAdditionalAttributesSelector(item)).ToDictionary(x => x.Key, x => x.Value);
		}
	}

	/// <summary>
	/// Retrieves the default settings for the grid. This method can be overridden in derived classes
	/// to provide different default settings or to use a derived settings class.
	/// </summary>
	protected virtual GridSettings GetDefaults() => HxGrid.Defaults;

	[Inject] protected IStringLocalizer<HxGrid> HxGridLocalizer { get; set; }

	private List<IHxGridColumn<TItem>> _columnsList;
	private HashSet<string> _columnIds;

	private CollectionRegistration<IHxGridColumn<TItem>> _columnsListRegistration;
	private bool _isDisposed = false;

	private bool _paginationDecreasePageIndexAfterRender = false;
	private List<TItem> _paginationDataItemsToRender;
	private CancellationTokenSource _paginationRefreshDataCancellationTokenSource;
	private List<GridInternalStateSortingItem<TItem>> _currentSorting = null;
	private bool _postponeCurrentSortingDeserialization = false;

	private bool _firstRenderCompleted = false;
	private GridUserState _previousUserState;
	private int _previousPageSizeEffective;
	private int _previousLoadMoreAdditionalItemsCount;

	private Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize<TItem> _infiniteScrollVirtualizeComponent;

	private int? _totalCount;
	private bool _dataProviderInProgress;
	private System.Timers.Timer _dataProviderInProgressDelayTimer;
	private bool _dataProviderInProgressAfterDelay;
	private bool _virtualizeDataProviderInProgressFromExplicitRefreshRequest;

	/// <summary>
	/// Constructor.
	/// </summary>
	public HxGrid()
	{
		_columnsList = new List<IHxGridColumn<TItem>>();
		_columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItem>>(_columnsList, async () => await InvokeAsync(StateHasChanged), () => _isDisposed, HandleColumnAdded, HandleColumnRemoved);
	}

	/// <inheritdoc />
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Contract.Requires<InvalidOperationException>(DataProvider != null, $"Property {nameof(DataProvider)} on {GetType()} must have a value.");
		Contract.Requires<InvalidOperationException>(CurrentUserState != null, $"Property {nameof(CurrentUserState)} on {GetType()} must have a value.");
		if ((ContentNavigationModeEffective == GridContentNavigationMode.InfiniteScroll) && MultiSelectionEnabled)
		{
			Contract.Requires<InvalidOperationException>(PreserveSelectionEffective, $"{nameof(PreserveSelection)} must be enabled on {nameof(HxGrid)} when using {nameof(GridContentNavigationMode.InfiniteScroll)} with {nameof(MultiSelectionEnabled)}.");
		}

		if (_previousUserState != CurrentUserState)
		{
			_currentSorting = DeserializeCurrentUserStateSorting(CurrentUserState.Sorting);
		}

		if (_firstRenderCompleted) /* after first render previousUserState cannot be null */
		{
			bool shouldRefreshData = false;

			if (_previousUserState != CurrentUserState)
			{
				// await: This adds one more render before OnParameterSetAsync is finished.
				// We consider it safe because we already have some data.
				// But for a moment (before data is refreshed (= before OnParametersSetAsync is finished), the component is rendered with a new user state and with old data).
				_previousUserState = CurrentUserState;
				shouldRefreshData = true;
			}

			if (_previousPageSizeEffective != PageSizeEffective)
			{
				_previousPageSizeEffective = PageSizeEffective;
				shouldRefreshData = true;
			}

			if (shouldRefreshData)
			{
				await RefreshDataCoreAsync();
			}
		}
		_previousUserState = CurrentUserState;
		_previousPageSizeEffective = PageSizeEffective;
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		// when no sorting is set, use default
		if (firstRender && (_currentSorting == null))
		{
			GridInternalStateSortingItem<TItem>[] defaultSorting = GetDefaultSorting();
			if (defaultSorting != null)
			{
				await SetCurrentSortingWithEventCallback(defaultSorting);
			}
		}

		if (firstRender && ((ContentNavigationModeEffective == GridContentNavigationMode.Pagination) || (ContentNavigationModeEffective == GridContentNavigationMode.LoadMore) || (ContentNavigationModeEffective == GridContentNavigationMode.PaginationAndLoadMore)))
		{
			// except InfiniteScroll (Virtualize will initiate the load on its own), we want to load data on first render
			await RefreshDataCoreAsync();
		}

		// when rendering page with no data, navigate one page back
		if (_paginationDecreasePageIndexAfterRender)
		{
			_paginationDecreasePageIndexAfterRender = false;
			int newPageIndex = ((_totalCount == null) /* hopefully not even possible */ || (_totalCount.Value == 0))
				? 0
				: (int)Math.Ceiling((decimal)_totalCount.Value / PageSizeEffective) - 1;
			if (await SetCurrentPageIndexWithEventCallback(newPageIndex))
			{
				await RefreshDataCoreAsync();
			}
		}

		_firstRenderCompleted = true;
	}

	/// <summary>
	/// Returns columns to render.
	/// </summary>
	protected List<IHxGridColumn<TItem>> GetColumnsToRender()
	{
		return _columnsList.Where(column => column.IsVisible()).OrderBy(column => column.GetOrder()).ToList();
	}

	/// <summary>
	/// Returns CSS class for the <c>&lt;table&gt;</c> element.
	/// </summary>
	/// <remarks>
	/// overridden in 176.BT2 project to allow setting background-color for grids with selected items.
	/// </remarks>
	/// <param name="rendersData">Indicates whether the grid renders data (<c>false</c> when the grid has no items to render or the data have not been loaded yet).</param>
	protected virtual string GetTableElementCssClass(bool rendersData)
	{
		bool hoverable = rendersData && (HoverEffective ?? (SelectionEnabled || MultiSelectionEnabled));
		return CssClassHelper.Combine("hx-grid table",
			hoverable ? "table-hover" : null,
			StripedEffective ? "table-striped" : null,
			TableCssClassEffective,
			ContentNavigationModeEffective == GridContentNavigationMode.InfiniteScroll ? "hx-grid-infinite-scroll" : null);
	}

	/// <summary>
	/// Returns default sorting if set.
	/// </summary>
	private GridInternalStateSortingItem<TItem>[] GetDefaultSorting()
	{
		var columnsSortings = GetColumnsToRender()
			.Select(item => new { Column = item, DefaultSortingOrder = item.GetDefaultSortingOrder() })
			.Where(item => item.DefaultSortingOrder != null)
			.OrderBy(item => item.DefaultSortingOrder.Value)
			.Select(item => item.Column)
			.ToArray();

		if (columnsSortings.Any())
		{
			return columnsSortings.Select(item => new GridInternalStateSortingItem<TItem>
			{
				Column = item,
				ReverseDirection = false
			}).ToArray();
		}

		return null;
	}

	/// <summary>
	/// Returns grid header cell context.
	/// </summary>
	protected virtual GridHeaderCellContext CreateGridHeaderCellContext()
	{
		return new GridHeaderCellContext { TotalCount = _totalCount };
	}

	/// <summary>
	/// Returns grid footer cell context.
	/// </summary>
	protected virtual GridFooterCellContext CreateGridFooterCellContext()
	{
		return new GridFooterCellContext { TotalCount = _totalCount };
	}

	private async Task SetSelectedDataItemWithEventCallback(TItem newSelectedDataItem)
	{
		if (!EqualityComparer<TItem>.Default.Equals(SelectedDataItem, newSelectedDataItem))
		{
			SelectedDataItem = newSelectedDataItem;
			await InvokeSelectedDataItemChangedAsync(newSelectedDataItem);
		}
	}

	private async Task SetSelectedDataItemsWithEventCallback(HashSet<TItem> selectedDataItems)
	{
		SelectedDataItems = selectedDataItems;
		await InvokeSelectedDataItemsChangedAsync(SelectedDataItems);
	}

	private async Task<bool> SetCurrentSortingWithEventCallback(IReadOnlyList<GridInternalStateSortingItem<TItem>> newSorting)
	{
		_postponeCurrentSortingDeserialization = false;
		_currentSorting = newSorting.ToList();
		CurrentUserState = CurrentUserState with { Sorting = SerializeToCurrentUserStateSorting(_currentSorting) };
		_previousUserState = CurrentUserState; // suppress another RefreshDataAsync call in OnParametersSetAsync
		await InvokeCurrentUserStateChangedAsync(CurrentUserState);
		return true;
	}

	private async Task<bool> SetCurrentPageIndexWithEventCallback(int newPageIndex)
	{
		if ((CurrentUserState.PageIndex != newPageIndex) || (CurrentUserState.LoadMoreAdditionalItemsCount != 0))
		{
			CurrentUserState = CurrentUserState with
			{
				PageIndex = newPageIndex,
				LoadMoreAdditionalItemsCount = 0 // When navigating by Pager in LoadMore mode, navigate directly to the page (do not load additional items).
			};
			_previousUserState = CurrentUserState; // suppress another RefreshDataAsync call in OnParametersSetAsync
			await InvokeCurrentUserStateChangedAsync(CurrentUserState);
			return true;
		}
		return false;
	}

	private async Task IncreaseCurrentLoadMoreAdditionalItemsCountWithEventCallback(int additionalItemsCount)
	{
		Contract.Requires(additionalItemsCount > 0);

		CurrentUserState = CurrentUserState with { LoadMoreAdditionalItemsCount = CurrentUserState.LoadMoreAdditionalItemsCount + additionalItemsCount };
		_previousUserState = CurrentUserState; // suppress another RefreshDataAsync call in OnParametersSetAsync
		await InvokeCurrentUserStateChangedAsync(CurrentUserState);
	}

	private async Task HandleSelectOrMultiSelectDataItemClick(TItem clickedDataItem)
	{
		Contract.Requires(SelectionEnabled || MultiSelectionEnabled);

		if (SelectionEnabled)
		{
			await SetSelectedDataItemWithEventCallback(clickedDataItem);
		}
		else // MultiSelectionEnabled
		{
			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? [];
			if (selectedDataItems.Add(clickedDataItem) // when the item was added
				|| selectedDataItems.Remove(clickedDataItem)) // or removed... But because of || item removal is performed only when the item was not added!
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}
	}

	private async Task HandleSortingClick(IHxGridColumn<TItem> newSortColumn)
	{
		GridInternalStateSortingItem<TItem>[] newSorting = GridInternalStateSortingItemHelper.ApplyColumnToSorting(_currentSorting, newSortColumn);

		if (await SetCurrentSortingWithEventCallback(newSorting))
		{
			await RefreshDataCoreAsync();
		}
	}

	public Task PagerCurrentPageIndexChanged(int newPageIndex) => HandlePagerCurrentPageIndexChanged(newPageIndex);

	private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
	{
		if (await SetCurrentPageIndexWithEventCallback(newPageIndex))
		{
			await RefreshDataCoreAsync();
		}
	}

	private async Task HandleLoadMoreClick()
	{
		await LoadMoreAsync();
	}

	private void HandleColumnAdded(IHxGridColumn<TItem> column)
	{
		string columnId = column.GetId();
		if (!String.IsNullOrEmpty(columnId))
		{
			_columnIds ??= new HashSet<string>();
			if (!_columnIds.Add(columnId))
			{
				throw new InvalidOperationException($"[{GetType().Name}] There is already registered another column with the '{columnId}' identifier.");
			}
		}

		if (_postponeCurrentSortingDeserialization)
		{
			_currentSorting = DeserializeCurrentUserStateSorting(CurrentUserState.Sorting);
		}
	}

	private void HandleColumnRemoved(IHxGridColumn<TItem> column)
	{
		if ((_columnIds != null) && (!String.IsNullOrEmpty(column.GetId())))
		{
			_columnIds.Remove(column.GetId());
		}
	}

	/// <summary>
	/// Requests a data refresh from the <see cref="DataProvider"/>.
	/// Useful for updating the grid when external data may have changed.
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
	public async Task RefreshDataAsync()
	{
		if (_firstRenderCompleted)
		{
			await RefreshDataCoreAsync();
		}
		else
		{
			// first render is not completed yet, default sorting not resolved yet, will load data in OnAfterRenderAsync
		}
	}

	/// <summary>
	/// Instructs the component to load data from its <see cref="DataProvider"/>.
	/// Used in internal methods to implement the data-loading flow.
	/// </summary>
	protected async Task RefreshDataCoreAsync()
	{
		switch (ContentNavigationModeEffective)
		{
			case GridContentNavigationMode.Pagination:
			case GridContentNavigationMode.LoadMore:
			case GridContentNavigationMode.PaginationAndLoadMore:
				await RefreshPaginationOrLoadMoreDataCoreAsync(forceReloadAllPaginationOrLoadMoreData: true);
				break;

			case GridContentNavigationMode.InfiniteScroll:
				if (_infiniteScrollVirtualizeComponent != null)
				{
					// Display the InProgress indicator when refreshing data from the RefreshDataAsync method.
					// Do not display the InProgress indicator when loading data due to scrolling (Virtualize displays placeholder items).
					_virtualizeDataProviderInProgressFromExplicitRefreshRequest = true;

					await _infiniteScrollVirtualizeComponent.RefreshDataAsync();

					// We are aware of the race condition that may occur when _virtualizeDataProviderInProgressFromExplicitRefreshRequest
					// is set to false while another refresh is requested in the meantime.
					// We believe that this race condition is rare and not worth implementing a more robust solution for.
					// The only consequence is a visual issue where the progress indicator may not be displayed when it should be.
					// This can be improved with a counter any later, when it turns out to be a problem.
					_virtualizeDataProviderInProgressFromExplicitRefreshRequest = false;
				}
				break;

			default:
				throw new InvalidOperationException(ContentNavigationModeEffective.ToString());
		}
	}

	internal async Task LoadMoreAsync()
	{
		Contract.Requires<InvalidOperationException>((ContentNavigationMode == GridContentNavigationMode.LoadMore) || (ContentNavigationModeEffective == GridContentNavigationMode.PaginationAndLoadMore), $"{nameof(LoadMoreAsync)} method can be used only with {nameof(ContentNavigationMode)}.{nameof(GridContentNavigationMode.LoadMore)} or {nameof(ContentNavigationMode)}.{nameof(GridContentNavigationMode.PaginationAndLoadMore)}.");

		await IncreaseCurrentLoadMoreAdditionalItemsCountWithEventCallback(PageSizeEffective);
		await RefreshPaginationOrLoadMoreDataCoreAsync();
	}

	private async ValueTask RefreshPaginationOrLoadMoreDataCoreAsync(bool forceReloadAllPaginationOrLoadMoreData = false)
	{
		Contract.Requires((ContentNavigationModeEffective == GridContentNavigationMode.Pagination) || (ContentNavigationModeEffective == GridContentNavigationMode.LoadMore) || (ContentNavigationModeEffective == GridContentNavigationMode.PaginationAndLoadMore));

#pragma warning disable VSTHRD103 // Call async methods when in an async method
		// TODO Consider CancelAsync method for net8.0+
		_paginationRefreshDataCancellationTokenSource?.Cancel();
#pragma warning restore VSTHRD103 // Call async methods when in an async method
		// do not dispose the CTS here, there might be still some tasks running and they might throw ObjectDisposedException when using the CancellationToken
		_paginationRefreshDataCancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = _paginationRefreshDataCancellationTokenSource.Token;

		GridUserState currentUserState = CurrentUserState;

		// note: PageSize can be null!
		// loading scenarios:
		// 1) initial load -> load everything (no paging) or load page 0 (special case of #5)
		// 2) next page load -> load page X
		// 3) additional items load -> load Y items
		// 4) sorting change load -> load page X + additional Y items
		// 5) state reset (GridUserState changed) -> load page X + additional Y items
		GridDataProviderRequest<TItem> request;
		bool loadingAdditionalItemsOnly;
		if (PageSizeEffective == 0)
		{
			loadingAdditionalItemsOnly = false;
			request = new GridDataProviderRequest<TItem>
			{
				StartIndex = 0,
				Count = null,
				Sorting = GridInternalStateSortingItemHelper.ToSortingItems(_currentSorting),
				CancellationToken = cancellationToken
			};
		}
		else
		{
			loadingAdditionalItemsOnly = !forceReloadAllPaginationOrLoadMoreData
				&& (currentUserState.LoadMoreAdditionalItemsCount > 0)
				&& (_paginationDataItemsToRender?.Count > 0);

			request = new GridDataProviderRequest<TItem>
			{
				StartIndex = loadingAdditionalItemsOnly
					? ((currentUserState.PageIndex + 1) * PageSizeEffective) + _previousLoadMoreAdditionalItemsCount // loading "a few" load more items
					: (currentUserState.PageIndex * PageSizeEffective), // loading whole page and additional items (no load more scenario or state reset)
				Count = loadingAdditionalItemsOnly
					? currentUserState.LoadMoreAdditionalItemsCount - _previousLoadMoreAdditionalItemsCount // loading "a few" load more items
					: PageSizeEffective + currentUserState.LoadMoreAdditionalItemsCount, // loading whole page and additional items (no load more scenario or state reset)
				Sorting = GridInternalStateSortingItemHelper.ToSortingItems(_currentSorting),
				CancellationToken = cancellationToken
			};
		}

		GridDataProviderResult<TItem> result;
		try
		{
			result = await InvokeDataProviderInternal(request);
		}
		catch (OperationCanceledException) // gRPC stack does not set the operationFailedException.CancellationToken, do not check in when-clause
		{
			// NOOP, we are the one who canceled the token
			return;
		}

		// do not use result from cancelled request (for the case a developer does not use the cancellation token)
		if (!cancellationToken.IsCancellationRequested)
		{
			#region Verify paged data information
			if (result.Data != null)
			{
				int dataCount = result.Data.Count();

				if ((request.Count != null) && (dataCount > request.Count))
				{
					throw new InvalidOperationException($"[{GetType().Name}] {nameof(DataProvider)} returned more data items then it was requested.");
				}

				if ((request.Count != null) && (result.TotalCount == null))
				{
					throw new InvalidOperationException($"[{GetType().Name}] {nameof(DataProvider)} did not set {nameof(GridDataProviderResult<TItem>.TotalCount)}.");
				}

				if (result.TotalCount != null && (dataCount > result.TotalCount.Value))
				{
					throw new InvalidOperationException($"[{GetType().Name}] Invalid {nameof(DataProvider)} response. {nameof(GridDataProviderResult<TItem>.TotalCount)} value smaller than the number of returned data items.");
				}
			}
			#endregion

			if (!loadingAdditionalItemsOnly)
			{
				_paginationDataItemsToRender = result.Data?.ToList();

				if (!PreserveSelectionEffective)
				{
					if (!EqualityComparer<TItem>.Default.Equals(SelectedDataItem, default))
					{
						if ((_paginationDataItemsToRender == null) || !_paginationDataItemsToRender.Contains(SelectedDataItem))
						{
							await SetSelectedDataItemWithEventCallback(default);
						}
					}

					if (SelectedDataItems?.Count > 0)
					{
						HashSet<TItem> selectedDataItems = _paginationDataItemsToRender?.Intersect(SelectedDataItems).ToHashSet() ?? new HashSet<TItem>();
						await SetSelectedDataItemsWithEventCallback(selectedDataItems);
					}
				}
			}
			else
			{
				_paginationDataItemsToRender.AddRange(result.Data?.ToList());
			}
			_previousLoadMoreAdditionalItemsCount = currentUserState.LoadMoreAdditionalItemsCount;
			// hide InProgress & show data
			StateHasChanged();
		}
	}

	private async ValueTask<Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult<TItem>> VirtualizeItemsProvider(Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderRequest request)
	{
		GridDataProviderRequest<TItem> gridDataProviderRequest = new GridDataProviderRequest<TItem>
		{
			StartIndex = request.StartIndex,
			Count = request.Count,
			Sorting = GridInternalStateSortingItemHelper.ToSortingItems(_currentSorting),
			CancellationToken = request.CancellationToken
		};

		GridDataProviderResult<TItem> gridDataProviderResponse = await InvokeDataProviderInternal(gridDataProviderRequest);

		if (!request.CancellationToken.IsCancellationRequested)
		{
			// hide InProgress
			StateHasChanged();
		}

		return new Microsoft.AspNetCore.Components.Web.Virtualization.ItemsProviderResult<TItem>(gridDataProviderResponse.Data, gridDataProviderResponse.TotalCount ?? 0);
	}

	private async Task<GridDataProviderResult<TItem>> InvokeDataProviderInternal(GridDataProviderRequest<TItem> request)
	{
		StartDataProviderInProgress();

		GridDataProviderResult<TItem> result = await DataProvider.Invoke(request);
		Contract.Requires<ArgumentException>(result != null, "The " + nameof(DataProvider) + " should never return null. Instance of " + nameof(GridDataProviderResult<TItem>) + " has to be returned.");

		// do not use result from cancelled request (for the case a developer does not use the cancellation token)
		if (!request.CancellationToken.IsCancellationRequested)
		{
			StopDataProviderInProgress();
			_totalCount = result.TotalCount ?? result.Data?.Count() ?? 0;
		}

		return result;
	}

	private void StartDataProviderInProgress()
	{
		if (!_dataProviderInProgress)
		{
			_dataProviderInProgress = true;
			if (ProgressIndicatorDelayEffective == 0)
			{
				_dataProviderInProgressAfterDelay = true;
				StateHasChanged();
			}
			else
			{
				_dataProviderInProgressDelayTimer = new System.Timers.Timer(ProgressIndicatorDelayEffective);
				_dataProviderInProgressDelayTimer.AutoReset = false; // run once
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
				_dataProviderInProgressDelayTimer.Elapsed += async (sender, e) => await HandleTimerElapsedAsync();
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
				_dataProviderInProgressDelayTimer.Start();
			}

			async Task HandleTimerElapsedAsync()
			{
				if (_dataProviderInProgress)
				{
					_dataProviderInProgressAfterDelay = true;
					await InvokeAsync(StateHasChanged);
				}
				if (_dataProviderInProgressDelayTimer != null)
				{
					_dataProviderInProgressDelayTimer.Dispose();
					_dataProviderInProgressDelayTimer = null;
				}
			}
		}
	}

	private void StopDataProviderInProgress()
	{
		_dataProviderInProgress = false; // Multithreading: we can safely clean dataProviderInProgress only when received data from non-cancelled task
		_dataProviderInProgressAfterDelay = false;
		// no need to call StateHasChanged, this method is called from InvokeDataProviderInternal where the rendering is expected to happen
	}

	#region MultiSelect events
	private async Task HandleMultiSelectSelectDataItemClicked(TItem selectedDataItem)
	{
		Contract.Requires(MultiSelectionEnabled);

		var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItem>();
		if (selectedDataItems.Add(selectedDataItem))
		{
			await SetSelectedDataItemsWithEventCallback(selectedDataItems);
		}
	}

	private async Task HandleMultiSelectUnselectDataItemClicked(TItem selectedDataItem)
	{
		Contract.Requires(MultiSelectionEnabled);

		var selectedDataItems = SelectedDataItems?.ToHashSet() ?? [];
		if (selectedDataItems.Remove(selectedDataItem))
		{
			await SetSelectedDataItemsWithEventCallback(selectedDataItems);
		}
	}

	private async Task HandleMultiSelectSelectAllClicked()
	{
		Contract.Requires(MultiSelectionEnabled, nameof(MultiSelectionEnabled));

		if (_paginationDataItemsToRender is null)
		{
			await SetSelectedDataItemsWithEventCallback([]);
		}
		else
		{
			if (PreserveSelectionEffective)
			{
				var selectedDataItems = SelectedDataItems?.ToHashSet() ?? [];
				int originalCount = selectedDataItems.Count;
				selectedDataItems.UnionWith(_paginationDataItemsToRender);
				if (selectedDataItems.Count != originalCount)
				{
					await SetSelectedDataItemsWithEventCallback(selectedDataItems);
				}
			}
			else
			{
				await SetSelectedDataItemsWithEventCallback(new HashSet<TItem>(_paginationDataItemsToRender));
			}
		}
	}

	private async Task HandleMultiSelectSelectNoneClicked()
	{
		Contract.Requires(MultiSelectionEnabled);

		if (PreserveSelectionEffective)
		{
			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? [];
			int originalCount = selectedDataItems.Count;
			selectedDataItems.ExceptWith(_paginationDataItemsToRender);
			if (selectedDataItems.Count != originalCount)
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}
		else
		{
			await SetSelectedDataItemsWithEventCallback([]);
		}
	}
	#endregion

	private List<GridUserStateSortingItem> SerializeToCurrentUserStateSorting(IEnumerable<GridInternalStateSortingItem<TItem>> sorting)
	{
		return sorting?.Select(item => new GridUserStateSortingItem
		{
			ColumnId = item.Column.GetId(),
			ReverseDirection = item.ReverseDirection
		}).ToList();
	}

	private List<GridInternalStateSortingItem<TItem>> DeserializeCurrentUserStateSorting(IEnumerable<GridUserStateSortingItem> gridSortingStateItems)
	{
		if ((gridSortingStateItems == null) || !gridSortingStateItems.Any())
		{
			return null;
		}

		// deserializing expression brings a security vulnerability
		// to make it safe we allow only those expressions which are present on any of the columns

		_postponeCurrentSortingDeserialization = false;

		var result = new List<GridInternalStateSortingItem<TItem>>();
		foreach (var gridSortingStateItem in gridSortingStateItems)
		{
			// try to find the column for the sorting state item
			var sortingColumn = _columnsList.FirstOrDefault(item => gridSortingStateItem.ColumnId == item.GetId());

			if (sortingColumn != null)
			{
				result.Add(new GridInternalStateSortingItem<TItem>
				{
					Column = sortingColumn,
					ReverseDirection = gridSortingStateItem.ReverseDirection
				});
			}
			else
			{
				// otherwise if any of the the columns with the sorting is not yet register so postpone the "deserialization" to the later time (HandleColumnAdded).
				_postponeCurrentSortingDeserialization = true;
				return null;
			}
		}

		return result;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_paginationRefreshDataCancellationTokenSource?.Cancel();
			// do not dispose the CTS here, there might be still some tasks running and they might throw ObjectDisposedException when using the CancellationToken
			_paginationRefreshDataCancellationTokenSource = null;

			_dataProviderInProgressDelayTimer?.Dispose();
			_dataProviderInProgressDelayTimer = null;

			_isDisposed = true;
		}
	}

	internal static SortDirection GetSortIconDisplayDirection(bool isCurrentSorting, List<GridInternalStateSortingItem<TItem>> currentSorting, SortingItem<TItem>[] columnSorting)
	{
		if (!isCurrentSorting)
		{
			// column is NOT the primary sort column and click will cause ascending sorting (icon hover effect)
			return columnSorting[0].SortDirection;
		}
		else
		{
			// column is the primary sort column and the icon shows the current sorting direction (status icon)
			return currentSorting[0].ReverseDirection
				? columnSorting[0].SortDirection.Reverse()
				: columnSorting[0].SortDirection;
		}
	}
}
