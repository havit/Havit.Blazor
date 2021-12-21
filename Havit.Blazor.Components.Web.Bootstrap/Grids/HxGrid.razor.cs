using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Collections;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid to display tabular data from data source. Includes support for client-side and server-side paging &amp; sorting (or virtualized scrolling as needed).
	/// </summary>
	/// <typeparam name="TItem">Type of row data item.</typeparam>
	public partial class HxGrid<TItem> : ComponentBase, IDisposable
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxGrid{TItem}"/>.
		/// </summary>
		public static GridSettings Defaults { get; } = new GridSettings();

		/// <summary>
		/// ColumnsRegistration cascading value name.
		/// </summary>
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		/// <summary>
		/// Data provider for items to render as a table.
		/// </summary>
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
		[Parameter] public RenderFragment Columns { get; set; }

		/// <summary>
		/// Context menu template.
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

		/// <summary>
		/// Page size.		
		/// </summary>
		[Parameter] public int? PageSize { get; set; } = null;

		/// <summary>
		/// Indicates whether to render footer when data are empty.
		/// </summary>
		[Parameter] public bool? ShowFooterWhenEmptyData { get; set; } = false;

		/// <summary>
		/// Current grid state (page, sorting).
		/// </summary>
		[Parameter] public GridUserState<TItem> CurrentUserState { get; set; } = new GridUserState<TItem>(0, null);

		/// <summary>
		/// Event fires when grid state is changed.
		/// </summary>
		[Parameter] public EventCallback<GridUserState<TItem>> CurrentUserStateChanged { get; set; }
		/// <summary>
		/// Triggers the <see cref="CurrentUserStateChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeCurrentUserStateChangedAsync(GridUserState<TItem> newGridUserState) => CurrentUserStateChanged.InvokeAsync(newGridUserState);

		/// <summary>
		/// Indicates whether the grid should be displayed as "in progress".
		/// When <c>null</c> (default) value is used, grid is "in progress" when retrieving data by data provider.
		/// </summary>
		[Parameter] public bool? InProgress { get; set; }

		/// <summary>
		/// Custom CSS class to render with <c>div</c> element wrapping the main <c>table</c> (<see cref="HxPager"/> is not wrapped in this <c>div</c> element).
		/// </summary>
		[Parameter] public string TableContainerCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with main <c>table</c> element.
		/// </summary>
		[Parameter] public string TableCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with header <c>tr</c> element.
		/// </summary>
		[Parameter] public string HeaderRowCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with data <c>tr</c> element.
		/// </summary>
		[Parameter] public string ItemRowCssClass { get; set; }

		/// <summary>
		/// Height of the item row used for infinite scroll calculations.
		/// Default value is <c>41px</c> (row-height of regular table-row within Bootstrap 5 default theme).
		/// </summary>
		[Parameter] public float? ItemRowHeight { get; set; }

		/// <summary>
		/// Returns custom CSS class to render with data <c>tr</c> element.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemRowCssClassSelector { get; set; }

		/// <summary>
		/// Custom CSS class to render with footer <c>tr</c> element.
		/// </summary>
		[Parameter] public string FooterRowCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with pager wrapping <c>div</c> element.
		/// </summary>
		[Parameter] public string PagerContainerCssClass { get; set; }

		[Inject] private IStringLocalizer<HxGrid> HxGridLocalizer { get; set; } // private: non-generic HxGrid grid is internal, so the property cannot have wider accessor (protected)

		/// <summary>
		/// Number of rows with placeholders to render.
		/// When value is zero, placeholders are not used.
		/// When <see cref="LoadingDataTemplate" /> is set, placeholder are not used.
		/// </summary>
		[Parameter] public int? PlaceholdersRowCount { get; set; }

		/// <summary>
		/// Infinite scroll:
		/// Gets or sets a value that determines how many additional items will be rendered
		/// before and after the visible region. This help to reduce the frequency of rendering
		/// during scrolling. However, higher values mean that more elements will be present
		/// in the page.<br/>
		/// Default is <c>50</c>.
		/// </summary>
		[Parameter] public int? OverscanCount { get; set; }

		protected int PageSizeEffective => PageSize ?? GetDefaults().PageSize;
		protected int PlaceholdersRowCountEffective => PlaceholdersRowCount ?? GetDefaults().PlaceholdersRowCount;
		protected GridContentNavigationMode ContentNavigationModeEffective => this.ContentNavigationMode ?? GetDefaults().ContentNavigationMode;

		/// <summary>
		/// Return <see cref="HxGrid{TItem}"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual GridSettings GetDefaults() => HxGrid<TItem>.Defaults;

		private List<IHxGridColumn<TItem>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItem>> columnsListRegistration;
		private bool isDisposed = false;

		private bool paginationDecreasePageIndexAfterRender = false;
		private List<TItem> paginationDataItemsToRender;
		private CancellationTokenSource paginationRefreshDataCancellationTokenSource;

		private Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize<TItem> infiniteScrollVirtualizeComponent;

		private int? totalCount;
		private bool dataProviderInProgress;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxGrid()
		{
			columnsList = new List<IHxGridColumn<TItem>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItem>>(columnsList, this.StateHasChanged, () => isDisposed);
		}

		/// <inheritdoc />
		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			Contract.Requires<InvalidOperationException>(DataProvider != null, $"Property {nameof(DataProvider)} on {GetType()} must have a value.");
			Contract.Requires<InvalidOperationException>(!MultiSelectionEnabled || (ContentNavigationModeEffective != GridContentNavigationMode.InfiniteScroll), $"Cannot use multi selection with infinite scroll on {GetType()}.");
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
				await RefreshPaginationDataCoreAsync(renderOnSuccess: true);
			}

			// when rendering page with no data, navigate one page back
			if (paginationDecreasePageIndexAfterRender)
			{
				paginationDecreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentUserState.PageIndex - 1);
				await RefreshPaginationDataCoreAsync(true);
			}
		}

		/// <summary>
		/// Returns columns to render.
		/// Main goal of the method is to add ContextMenuGridColumn to the user defined columns.
		/// </summary>
		protected List<IHxGridColumn<TItem>> GetColumnsToRender()
		{
			return columnsList.Where(column => column.IsVisible()).OrderBy(column => column.GetOrder()).ToList();
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
			CurrentUserState = new GridUserState<TItem>(CurrentUserState.PageIndex, newSorting);
			await InvokeCurrentUserStateChangedAsync(CurrentUserState);
			return true;
		}

		private async Task<bool> SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentUserState.PageIndex != newPageIndex)
			{
				CurrentUserState = new GridUserState<TItem>(newPageIndex, CurrentUserState.Sorting);
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
					// We don't auto-render after this operation because in the typical use case, the
					// host component calls this from one of its lifecycle methods, and will naturally
					// re-render afterwards anyway. It's not desirable to re-render twice.
					await RefreshPaginationDataCoreAsync(renderOnSuccess: false);
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

		private async ValueTask RefreshPaginationDataCoreAsync(bool renderOnSuccess = false)
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

				if (renderOnSuccess)
				{
					StateHasChanged();
				}
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

			// do not use result from cancelled request (for the case a developer does not use the cancellation token)
			if (!request.CancellationToken.IsCancellationRequested)
			{
				dataProviderInProgress = false; // Multithreading: we can safelly clean dataProviderInProgress only wnen received data from non-cancelled task
				totalCount = result.TotalCount ?? result.Data.Count();
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
		public virtual void Dispose()
		{
			paginationRefreshDataCancellationTokenSource?.Cancel();
			paginationRefreshDataCancellationTokenSource?.Dispose();
			paginationRefreshDataCancellationTokenSource = null;

			isDisposed = true;
		}
	}
}
