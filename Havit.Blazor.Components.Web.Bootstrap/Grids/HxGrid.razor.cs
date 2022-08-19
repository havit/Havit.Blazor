using System.Threading;
using Havit.Diagnostics.Contracts;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid to display tabular data from data source. Includes support for client-side and server-side paging &amp; sorting (or virtualized scrolling as needed).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxGrid">https://havit.blazor.eu/components/HxGrid</see>
	/// </summary>
	/// <typeparam name="TItem">Type of row data item.</typeparam>
#if NET6_0_OR_GREATER
	[CascadingTypeParameter(nameof(TItem))]
#endif
	public partial class HxGrid<TItem> : ComponentBase, IDisposable
	{
		/// <summary>
		/// ColumnsRegistration cascading value name.
		/// </summary>
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="HxGrid.Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public GridSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual GridSettings GetSettings() => this.Settings;

		/// <summary>
		/// Data provider for items to render.<br />
		/// The provider should always return instance of <see cref="GridDataProviderResult{TItem}"/>, <c>null</c> is not allowed.
		/// </summary>
#if NET6_0_OR_GREATER

		[EditorRequired]
#endif
		[Parameter] public GridDataProviderDelegate<TItem> DataProvider { get; set; }

		/// <summary>
		/// Indicates whether single data item selection is enabled. 
		/// Selection is performed by click on the item row.
		/// Can be combined with multiselection.
		/// Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool SelectionEnabled { get; set; } = true;

		/// <summary>
		/// Indicates whether multi data items selection is enabled. 
		/// Selection is performed by checkboxes in the first column.
		/// Can be combined with (single) selection.
		/// Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool MultiSelectionEnabled { get; set; } = false;

		/// <summary>
		/// Columns template.
		/// </summary>
#if NET6_0_OR_GREATER
		[EditorRequired]
#endif
		[Parameter] public RenderFragment Columns { get; set; }

		/// <summary>
		/// Context menu template (positioned as last column).<br/>
		/// NOTE: This parameter will be most likely removed in vNext, use <see cref="HxContextMenuGridColumn{TItem}"/> in <see cref="Columns"/> instead.
		/// </summary>
		[Parameter] public RenderFragment<TItem> ContextMenu { get; set; }

		/// <summary>
		/// Template to render when "first" data are loading.
		/// This template is not used when loading data for sorting or paging operations.
		/// </summary>
		[Parameter] public RenderFragment LoadingDataTemplate { get; set; }

		/// <summary>
		/// Template to render when there is empty Data (but not <c>null</c>).
		/// </summary>
		[Parameter] public RenderFragment EmptyDataTemplate { get; set; }

		/// <summary>
		/// Selected data item.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public TItem SelectedDataItem { get; set; }

		/// <summary>
		/// Event fires when selected data item changes.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public EventCallback<TItem> SelectedDataItemChanged { get; set; }
		/// <summary>
		/// Triggers the <see cref="SelectedDataItemChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeSelectedDataItemChangedAsync(TItem selectedDataItem) => SelectedDataItemChanged.InvokeAsync(selectedDataItem);

		/// <summary>
		/// Selected data items.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public HashSet<TItem> SelectedDataItems { get; set; }

		/// <summary>
		/// Event fires when selected data items changes.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public EventCallback<HashSet<TItem>> SelectedDataItemsChanged { get; set; }
		/// <summary>
		/// Triggers the <see cref="SelectedDataItemsChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeSelectedDataItemsChangedAsync(HashSet<TItem> selectedDataItems) => SelectedDataItemsChanged.InvokeAsync(selectedDataItems);

		/// <summary>
		/// Strategy how data are displayed in the grid (and loaded to the grid).
		/// </summary>
		[Parameter] public GridContentNavigationMode? ContentNavigationMode { get; set; }
		protected GridContentNavigationMode ContentNavigationModeEffective => this.ContentNavigationMode ?? this.GetSettings()?.ContentNavigationMode ?? GetDefaults().ContentNavigationMode ?? throw new InvalidOperationException(nameof(ContentNavigationMode) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Page size for <see cref="GridContentNavigationMode.Pagination"/>. Set <c>0</c> to disable paging.
		/// </summary>
		[Parameter] public int? PageSize { get; set; }
		protected int PageSizeEffective => this.PageSize ?? this.GetSettings()?.PageSize ?? GetDefaults().PageSize ?? throw new InvalidOperationException(nameof(PageSize) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Indicates whether to render footer when data are empty.
		/// Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool? ShowFooterWhenEmptyData { get; set; }
		protected bool ShowFooterWhenEmptyDataEffective => this.ShowFooterWhenEmptyData ?? this.GetSettings()?.ShowFooterWhenEmptyData ?? GetDefaults().ShowFooterWhenEmptyData ?? throw new InvalidOperationException(nameof(ShowFooterWhenEmptyData) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Pager settings.
		/// </summary>
		[Parameter] public PagerSettings PagerSettings { get; set; }
		protected PagerSettings PagerSettingsEffective => this.PagerSettings ?? this.GetSettings()?.PagerSettings ?? GetDefaults().PagerSettings;

		/// <summary>
		/// Current grid state (page, sorting).
		/// </summary>
		[Parameter] public GridUserState<TItem> CurrentUserState { get; set; } = new GridUserState<TItem>();

		/// <summary>
		/// Event fires when grid state is changed.
		/// </summary>
		[Parameter] public EventCallback<GridUserState<TItem>> CurrentUserStateChanged { get; set; }
		/// <summary>
		/// Triggers the <see cref="CurrentUserStateChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeCurrentUserStateChangedAsync(GridUserState<TItem> newGridUserState) => CurrentUserStateChanged.InvokeAsync(newGridUserState);

		/// <summary>
		/// Indicates when the grid should be displayed as "in progress".
		/// When not set (<c>null</c>), grid progress is automatically tracked when retrieving data by data provider.
		/// </summary>
		[Parameter] public bool? InProgress { get; set; }

		/// <summary>
		/// Custom CSS class to render with <c>div</c> element wrapping the main <c>table</c> (<see cref="HxPager"/> is not wrapped in this <c>div</c> element).
		/// </summary>
		[Parameter] public string TableContainerCssClass { get; set; }
		protected string TableContainerCssClassEffective => this.TableContainerCssClass ?? this.GetSettings()?.TableContainerCssClass ?? GetDefaults().TableContainerCssClass;

		/// <summary>
		/// Custom CSS class to render with main <c>table</c> element.
		/// </summary>
		[Parameter] public string TableCssClass { get; set; }
		protected string TableCssClassEffective => this.TableCssClass ?? this.GetSettings()?.TableCssClass ?? GetDefaults().TableCssClass;

		/// <summary>
		/// Custom CSS class to render with header <c>tr</c> element.
		/// </summary>
		[Parameter] public string HeaderRowCssClass { get; set; }
		protected string HeaderRowCssClassEffective => this.HeaderRowCssClass ?? this.GetSettings()?.HeaderRowCssClass ?? GetDefaults().HeaderRowCssClass;

		/// <summary>
		/// Custom CSS class to render with data <c>tr</c> element.
		/// </summary>
		[Parameter] public string ItemRowCssClass { get; set; }
		protected string ItemRowCssClassEffective => this.ItemRowCssClass ?? this.GetSettings()?.ItemRowCssClass ?? GetDefaults().ItemRowCssClass;

		/// <summary>
		/// Height of the item row used for infinite scroll calculations.
		/// Default value is <c>41px</c> (row-height of regular table-row within Bootstrap 5 default theme).
		/// </summary>
		[Parameter] public float? ItemRowHeight { get; set; }
		protected float ItemRowHeightEffective => this.ItemRowHeight ?? this.GetSettings()?.ItemRowHeight ?? GetDefaults().ItemRowHeight ?? throw new InvalidOperationException(nameof(ItemRowHeight) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Returns custom CSS class to render with data <c>tr</c> element.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemRowCssClassSelector { get; set; }

		/// <summary>
		/// Custom CSS class to render with footer <c>tr</c> element.
		/// </summary>
		[Parameter] public string FooterRowCssClass { get; set; }
		protected string FooterRowCssClassEffective => this.FooterRowCssClass ?? this.GetSettings()?.FooterRowCssClass ?? GetDefaults().FooterRowCssClass;

		/// <summary>
		/// Custom CSS class to add to the pager.
		/// </summary>
		[Parameter] public string PagerCssClass { get; set; }
		protected string PagerCssClassEffective => this.PagerCssClass ?? this.GetSettings()?.PagerCssClass ?? GetDefaults().PagerCssClass;

		/// <summary>
		/// Number of rows with placeholders to render.
		/// When value is zero, placeholders are not used.
		/// When <see cref="LoadingDataTemplate" /> is set, placeholder are not used.
		/// Default is <c>5</c>.
		/// </summary>
		[Parameter] public int? PlaceholdersRowCount { get; set; }
		protected int PlaceholdersRowCountEffective => this.PlaceholdersRowCount ?? this.GetSettings()?.PlaceholdersRowCount ?? GetDefaults().PlaceholdersRowCount ?? throw new InvalidOperationException(nameof(PlaceholdersRowCount) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Infinite scroll:
		/// Gets or sets a value that determines how many additional items will be rendered
		/// before and after the visible region. This help to reduce the frequency of rendering
		/// during scrolling. However, higher values mean that more elements will be present
		/// in the page.<br/>
		/// Default is <c>50</c>.
		/// </summary>
		[Parameter] public int? OverscanCount { get; set; }
		protected int OverscanCountEffective => this.OverscanCount ?? this.GetSettings()?.OverscanCount ?? GetDefaults().OverscanCount ?? throw new InvalidOperationException(nameof(OverscanCount) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Allows the table to be scrolled horizontally with ease accross any breakpoint (adds the <c>table-responsive</c> class to the table).<br/>
		/// Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool? Responsive { get; set; }
		protected bool ResponsiveEffective => this.Responsive ?? this.GetSettings()?.Responsive ?? GetDefaults().Responsive ?? throw new InvalidOperationException(nameof(Responsive) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Enables hover state on table rows within a <c>&lt;tbody&gt;</c> (sets the <c>table-hover</c> class on the table).<br />
		/// If not set (default) the table is hoverable when selection is enabled.
		/// </summary>
		[Parameter] public bool? Hover { get; set; }
		protected bool? HoverEffective => this.Hover ?? this.GetSettings()?.Hover ?? GetDefaults().Hover;

		/// <summary>
		/// Adds zebra-striping to any table row within the <c>&lt;tbody&gt;</c> (alternating rows).<br />
		/// Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool? Striped { get; set; }
		protected bool StripedEffective => this.Striped ?? this.GetSettings()?.Striped ?? GetDefaults().Striped ?? throw new InvalidOperationException(nameof(Striped) + " default for " + nameof(HxGrid) + " has to be set.");

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual GridSettings GetDefaults() => HxGrid.Defaults;

		[Inject] protected IStringLocalizer<HxGrid> HxGridLocalizer { get; set; }

		private List<IHxGridColumn<TItem>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItem>> columnsListRegistration;
		private bool isDisposed = false;

		private bool paginationDecreasePageIndexAfterRender = false;
		private List<TItem> paginationDataItemsToRender;
		private CancellationTokenSource paginationRefreshDataCancellationTokenSource;
		private GridUserState<TItem> previousUserState;
		private bool firstRenderCompleted = false;

		private Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize<TItem> infiniteScrollVirtualizeComponent;

		private int? totalCount;
		private bool dataProviderInProgress;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxGrid()
		{
			columnsList = new List<IHxGridColumn<TItem>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItem>>(columnsList, async () => await InvokeAsync(this.StateHasChanged), () => isDisposed);
		}

		/// <inheritdoc />
		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			Contract.Requires<InvalidOperationException>(DataProvider != null, $"Property {nameof(DataProvider)} on {GetType()} must have a value.");
			Contract.Requires<InvalidOperationException>(CurrentUserState != null, $"Property {nameof(CurrentUserState)} on {GetType()} must have a value.");
			Contract.Requires<InvalidOperationException>(!MultiSelectionEnabled || (ContentNavigationModeEffective != GridContentNavigationMode.InfiniteScroll), $"Cannot use multi selection with infinite scroll on {GetType()}.");

			if (firstRenderCompleted && (previousUserState != CurrentUserState)) /* after first render previousUserState cannot be null */
			{
				// await: This adds one more render before OnParameterSetAsync is finished.
				// We consider it safe because we already have some data.
				// But for a moment (before data is refreshed (= before OnParametersSetAsync is finished), the component is rendered with a new user state and with old data).
				previousUserState = CurrentUserState;
				await RefreshDataAsync();
			}
			previousUserState = CurrentUserState;
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			// when no sorting is set, use default
			if (firstRender && (CurrentUserState.Sorting == null))
			{
				SortingItem<TItem>[] defaultSorting = GetDefaultSorting();
				if (defaultSorting != null)
				{
					await SetCurrentSortingWithEventCallback(defaultSorting);
				}
			}

			if (firstRender && (ContentNavigationModeEffective == GridContentNavigationMode.Pagination))
			{
				await RefreshPaginationDataCoreAsync();
			}

			// when rendering page with no data, navigate one page back
			if (paginationDecreasePageIndexAfterRender)
			{
				paginationDecreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentUserState.PageIndex - 1);
				await RefreshPaginationDataCoreAsync();
			}

			firstRenderCompleted = true;
		}

		/// <summary>
		/// Returns columns to render.
		/// </summary>
		protected List<IHxGridColumn<TItem>> GetColumnsToRender()
		{
			return columnsList.Where(column => column.IsVisible()).OrderBy(column => column.GetOrder()).ToList();
		}

		/// <summary>
		/// Returns CSS class for the <c>&lt;table&gt;</c> element.
		/// </summary>
		/// <remarks>
		/// Overriden in 176.BT2 project to allow setting background-color for grids with selected items.
		/// </remarks>
		/// <param name="rendersData">Indicates whether the grid renders data (<c>false</c> when the grid has no items to render or the data have not been loaded yet).</param>
		protected virtual string GetTableElementCssClass(bool rendersData)
		{
			bool hoverable = rendersData && (this.HoverEffective ?? (this.SelectionEnabled || this.MultiSelectionEnabled));
			return CssClassHelper.Combine("hx-grid table",
				hoverable ? "table-hover" : null,
				this.StripedEffective ? "table-striped" : null,
				this.TableCssClassEffective);
		}

		/// <summary>
		/// Returns default sorting if set.
		/// </summary>
		private SortingItem<TItem>[] GetDefaultSorting()
		{
			var columnsSortings = GetColumnsToRender().SelectMany(item => item.GetSorting()).ToArray();

			if (columnsSortings.Any())
			{
				var defaultSorting = columnsSortings
					.Where(item => item.SortDefaultOrder != null)
					.OrderBy(item => item.SortDefaultOrder.Value)
					.ToArray();


				return defaultSorting;
			}

			return null;
		}

		/// <summary>
		/// Returns grid header cell context.
		/// </summary>
		protected virtual GridHeaderCellContext CreateGridHeaderCellContext()
		{
			return new GridHeaderCellContext { TotalCount = totalCount };
		}

		/// <summary>
		/// Returns grid footer cell context.
		/// </summary>
		protected virtual GridFooterCellContext CreateGridFooterCellContext()
		{
			return new GridFooterCellContext { TotalCount = totalCount };
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

		private async Task<bool> SetCurrentSortingWithEventCallback(IReadOnlyList<SortingItem<TItem>> newSorting)
		{
			CurrentUserState = CurrentUserState with { Sorting = newSorting };
			previousUserState = CurrentUserState; // suppress another RefreshDataAsync call in OnParametersSetAsync
			await InvokeCurrentUserStateChangedAsync(CurrentUserState);
			return true;
		}

		private async Task<bool> SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentUserState.PageIndex != newPageIndex)
			{
				CurrentUserState = CurrentUserState with { PageIndex = newPageIndex };
				previousUserState = CurrentUserState; // suppress another RefreshDataAsync call in OnParametersSetAsync
				await InvokeCurrentUserStateChangedAsync(CurrentUserState);
				return true;
			}
			return false;
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
				var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItem>();
				if (selectedDataItems.Add(clickedDataItem) // when the item was added
					|| selectedDataItems.Remove(clickedDataItem)) // or removed... But because of || item removal is performed only when the item was not added!
				{
					await SetSelectedDataItemsWithEventCallback(selectedDataItems);
				}
			}
		}

		private async Task HandleSortingClick(IEnumerable<SortingItem<TItem>> sorting)
		{
			if (await SetCurrentSortingWithEventCallback(CurrentUserState.Sorting?.ApplySorting(sorting.ToArray()) ?? sorting.ToList().AsReadOnly())) // when current sorting is null, use new sorting
			{
				await RefreshDataAsync();
			}
		}

		private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
		{
			if (await SetCurrentPageIndexWithEventCallback(newPageIndex))
			{
				await RefreshDataAsync();
			}
		}

		/// <summary>
		/// Instructs the component to re-request data from its <see cref="DataProvider"/>.
		/// This is useful if external data may have changed.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
		public async Task RefreshDataAsync()
		{
			switch (ContentNavigationModeEffective)
			{
				case GridContentNavigationMode.Pagination:
					await RefreshPaginationDataCoreAsync();
					break;

				case GridContentNavigationMode.InfiniteScroll:
					if (infiniteScrollVirtualizeComponent != null)
					{
						await infiniteScrollVirtualizeComponent.RefreshDataAsync();
					}
					// when infiniteScrollVirtualizeComponent, it will be rendered and data loaded so no action here is required
					break;

				default: throw new InvalidOperationException(ContentNavigationModeEffective.ToString());
			}
		}

		private async ValueTask RefreshPaginationDataCoreAsync()
		{
			Contract.Requires(ContentNavigationModeEffective == GridContentNavigationMode.Pagination);

			paginationRefreshDataCancellationTokenSource?.Cancel();
			paginationRefreshDataCancellationTokenSource?.Dispose();
			paginationRefreshDataCancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = paginationRefreshDataCancellationTokenSource.Token;

			int? pageSizeEffective = PageSizeEffective;
			GridDataProviderRequest<TItem> request = new GridDataProviderRequest<TItem>
			{
				StartIndex = (pageSizeEffective ?? 0) * CurrentUserState.PageIndex,
				Count = pageSizeEffective,
				Sorting = CurrentUserState.Sorting,
				CancellationToken = cancellationToken
			};

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
				if ((result.Data != null) && (pageSizeEffective > 0))
				{
					int dataCount = result.Data.Count();

					if (dataCount > pageSizeEffective.Value)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} returned more data items then is the size od the page.");
					}

					if (result.TotalCount == null)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} did not set ${nameof(GridDataProviderResult<TItem>.TotalCount)}.");
					}
					else if (dataCount > result.TotalCount.Value)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} set ${nameof(GridDataProviderResult<TItem>.TotalCount)} property byt the value is smaller than the number of data items.");
					}
				}
				#endregion

				paginationDataItemsToRender = result.Data?.ToList();

				if (!EqualityComparer<TItem>.Default.Equals(SelectedDataItem, default))
				{
					if ((paginationDataItemsToRender == null) || !paginationDataItemsToRender.Contains(SelectedDataItem))
					{
						await SetSelectedDataItemWithEventCallback(default);
					}
				}

				if (SelectedDataItems?.Count > 0)
				{
					HashSet<TItem> selectedDataItems = paginationDataItemsToRender?.Intersect(SelectedDataItems).ToHashSet() ?? new HashSet<TItem>();
					await SetSelectedDataItemsWithEventCallback(selectedDataItems);
				}

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
				Sorting = CurrentUserState.Sorting,
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
			// Multithreading: we can safelly set dataProviderInProgress, always dataProvider is going to retrieve data we are it is in in a progress.
			if (!dataProviderInProgress)
			{
				dataProviderInProgress = true;
				StateHasChanged();
			}

			GridDataProviderResult<TItem> result = await DataProvider.Invoke(request);
			Contract.Requires<ArgumentException>(result != null, "The " + nameof(DataProvider) + " should never return null. Instance of " + nameof(GridDataProviderResult<TItem>) + " has to be returned.");

			// do not use result from cancelled request (for the case a developer does not use the cancellation token)
			if (!request.CancellationToken.IsCancellationRequested)
			{
				dataProviderInProgress = false; // Multithreading: we can safelly clean dataProviderInProgress only wnen received data from non-cancelled task
				totalCount = result.TotalCount ?? result.Data?.Count() ?? 0;
			}

			return result;
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
			Contract.Requires(ContentNavigationModeEffective == GridContentNavigationMode.Pagination, "ContentNavigationModeEffective == GridContentNavigationMode.Pagination");

			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItem>();
			if (selectedDataItems.Remove(selectedDataItem))
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}

		private async Task HandleMultiSelectSelectAllClicked()
		{
			Contract.Requires(MultiSelectionEnabled, nameof(MultiSelectionEnabled));
			Contract.Requires(ContentNavigationModeEffective == GridContentNavigationMode.Pagination, "ContentNavigationModeEffective == GridContentNavigationMode.Pagination");

			await SetSelectedDataItemsWithEventCallback(new HashSet<TItem>(paginationDataItemsToRender));
		}

		private async Task HandleMultiSelectSelectNoneClicked()
		{
			Contract.Requires(MultiSelectionEnabled);
			Contract.Requires(ContentNavigationModeEffective == GridContentNavigationMode.Pagination, "ContentNavigationModeEffective == GridContentNavigationMode.Pagination");

			await SetSelectedDataItemsWithEventCallback(new HashSet<TItem>());
		}
		#endregion

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				paginationRefreshDataCancellationTokenSource?.Cancel();
				paginationRefreshDataCancellationTokenSource?.Dispose();
				paginationRefreshDataCancellationTokenSource = null;

				isDisposed = true;
			}
		}
	}
}
