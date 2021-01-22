using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Collections;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Footer je vidět i když nemáme data. Chceme to? By default? Nastavitelné?
	// TODO: V případě zobrazení, kam umístit (vyrenderovat) empty template?
	// TODO: EmptyTemplate vs. default empty template? Jak se hezky doplnit?
	// TODO: EmptyTemplate - renderovat do tabulky nebo pod tabulku? (nyní pod tabulku, ale...)

	/// <summary>
	/// Grid to display tabular data from data source.
	/// </summary>
	/// <typeparam name="TItemType">Type of row data item.</typeparam>
	public partial class HxGrid<TItemType> : ComponentBase, IDisposable
	{
		/// <summary>
		/// ColumnsRegistration cascading value name.
		/// </summary>
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		/// <summary>
		/// Items to render as a table (mutually exclusive with DataProvider).
		/// </summary>
		[Parameter] public IEnumerable<TItemType> Data { get; set; }

		/// <summary>
		/// Data provider for items to render as a table (mutually exclusive with Data).
		/// </summary>
		[Parameter] public GridDataProviderDelegate<TItemType> DataProvider { get; set; }

		/// <summary>
		/// Indicates whether single data item selection is enabled. 
		/// Selection is performed by click on the item row.
		/// Can be combined with multiselection.
		/// Default is true.
		/// </summary>
		[Parameter] public bool SelectionEnabled { get; set; } = true;

		/// <summary>
		/// Indicates whether multi data items selection is enabled. 
		/// Selection is performed by checkboxes in the first column.
		/// Can be combined with (single) selection.
		/// Default is false.
		/// </summary>
		[Parameter] public bool MultiSelectionEnabled { get; set; } = false;

		/// <summary>
		/// Columns template.
		/// </summary>
		[Parameter] public RenderFragment Columns { get; set; }

		/// <summary>
		/// Context menu template.
		/// </summary>
		[Parameter] public RenderFragment<TItemType> ContextMenu { get; set; }

		/// <summary>
		/// Template to render when there is empty Data (but not null).
		/// </summary>
		[Parameter] public RenderFragment EmptyTemplate { get; set; }

		/// <summary>
		/// Selected data item.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public TItemType SelectedDataItem { get; set; }

		/// <summary>
		/// Event fires when selected data item changes.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public EventCallback<TItemType> SelectedDataItemChanged { get; set; }

		/// <summary>
		/// Selected data items.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public HashSet<TItemType> SelectedDataItems { get; set; }

		/// <summary>
		/// Event fires when selected data items changes.
		/// Intended for data binding.
		/// </summary>		
		[Parameter] public EventCallback<HashSet<TItemType>> SelectedDataItemsChanged { get; set; }

		/// <summary>
		/// Page size.
		/// </summary>
		[Parameter] public int PageSize { get; set; } = 0;

		/// <summary>
		/// Enable/disable in-memory auto-sorting the data in <see cref="Data"/> property.
		/// Default: Auto-sorting is enabled when all sortings on all columns have <c>SortKeySelector</c>.
		/// </summary>
		[Parameter] public bool? AutoSort { get; set; }

		/// <summary>
		/// Current grid state (page, sorting).
		/// </summary>
		[Parameter] public GridUserState<TItemType> CurrentUserState { get; set; } = new GridUserState<TItemType>(0, null);

		/// <summary>
		/// Event fires when grid state is changed.
		/// </summary>
		[Parameter] public EventCallback<GridUserState<TItemType>> CurrentUserStateChanged { get; set; }

		[Inject] private IStringLocalizer<HxGrid> HxGridLocalizer { get; set; } // private: non-generic HxGrid grid is internal, so the property cannot have wider accessor (protected)

		private List<IHxGridColumn<TItemType>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration;
		private bool decreasePageIndexAfterRender = false;
		private bool firstRenderCompleted = false;
		private bool isDisposed = false;
		private GridDataProviderDelegate<TItemType> dataProvider;
		private List<TItemType> dataItemsToRender;
		private int? dataItemsTotalCount;
		private CancellationTokenSource refreshDataCancellationTokenSource;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxGrid()
		{
			columnsList = new List<IHxGridColumn<TItemType>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItemType>>(columnsList, this.StateHasChanged, () => isDisposed);
		}

		/// <inheritdoc />
		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			Contract.Requires<InvalidOperationException>((Data == null) || (DataProvider == null), $"{GetType()} can only accept one item source from its parameters. Do not supply both '{nameof(Data)}' and '{nameof(DataProvider)}'.");

			if (DataProvider != null)
			{
				dataProvider = DataProvider;
			}
			else
			{
				dataProvider = EnumerableDataProvider;

				if (firstRenderCompleted) // we call RefreshDataCoreAsync after first render (when sorting is known), so we do not need to RefreshDataCoreAsync them in first render)
				{
					// When we have a fixed set of in-memory data, it doesn't cost anything to
					// re-query it on each cycle, so do that. This means the developer can add/remove
					// items in the collection and see the UI update without having to call RefreshDataAsync.

					// Despite the Virtualize component (the inspiration), HxGrid can be asynchronous in RefreshDataCoreAsync with EnumerableDataProvider
					// due the EventCallbacks (setting selected items etc.).
					await RefreshDataCoreAsync(renderOnSuccess: false);
				}
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			// when no sorting is set, use default
			if (firstRender && (CurrentUserState.Sorting == null))
			{
				SortingItem<TItemType>[] defaultSorting = GetDefaultSorting();
				if (defaultSorting != null)
				{
					await SetCurrentSortingWithEventCallback(defaultSorting);
				}
			}

			if (firstRender)
			{
				await RefreshDataCoreAsync(true);
			}

			// when rendering page with no data, navigate one page back
			if (decreasePageIndexAfterRender)
			{
				decreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentUserState.PageIndex - 1);
				await RefreshDataCoreAsync(true);
			}

			firstRenderCompleted = true;
		}

		/// <summary>
		/// Returns columns to render.
		/// Main goal of the method is to add ContextMenuGridColumn to the user defined columns.
		/// </summary>
		protected List<IHxGridColumn<TItemType>> GetColumnsToRender()
		{
			var result = new List<IHxGridColumn<TItemType>>(columnsList);
			return result;
		}

		/// <summary>
		/// Returns default sorting and checks the default sorting settings.
		/// Default sorting is required when at least one column has specified any sorting.
		/// Default sorting is not required when there is no sorting specified on columns.
		/// </summary>
		private SortingItem<TItemType>[] GetDefaultSorting()
		{
			var columnsSortings = GetColumnsToRender().SelectMany(item => item.GetSorting()).ToArray();

			if (columnsSortings.Any())
			{
				var defaultSorting = columnsSortings
					.Where(item => item.SortDefaultOrder != null)
					.OrderBy(item => item.SortDefaultOrder.Value)
					.ToArray();

				Contract.Assert<InvalidOperationException>(defaultSorting.Length > 0, "Default sorting has to be set.");

				return defaultSorting;
			}

			return null;
		}

		private async Task SetSelectedDataItemWithEventCallback(TItemType newSelectedDataItem)
		{
			if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, newSelectedDataItem))
			{
				SelectedDataItem = newSelectedDataItem;
				await SelectedDataItemChanged.InvokeAsync(newSelectedDataItem);
			}
		}

		private async Task SetSelectedDataItemsWithEventCallback(HashSet<TItemType> selectedDataItems)
		{
			SelectedDataItems = selectedDataItems;
			await SelectedDataItemsChanged.InvokeAsync(SelectedDataItems);
		}

		private async Task<bool> SetCurrentSortingWithEventCallback(IReadOnlyList<SortingItem<TItemType>> newSorting)
		{
			CurrentUserState = new GridUserState<TItemType>(CurrentUserState.PageIndex, newSorting);
			await CurrentUserStateChanged.InvokeAsync(CurrentUserState);
			return true;
		}

		private async Task<bool> SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentUserState.PageIndex != newPageIndex)
			{
				CurrentUserState = new GridUserState<TItemType>(newPageIndex, CurrentUserState.Sorting);
				await CurrentUserStateChanged.InvokeAsync(CurrentUserState);
				return true;
			}
			return false;
		}

		private async Task HandleSelectDataItemClick(TItemType newSelectedDataItem)
		{
			Contract.Requires(SelectionEnabled);

			if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, newSelectedDataItem))
			{
				SelectedDataItem = newSelectedDataItem;
				await SelectedDataItemChanged.InvokeAsync(newSelectedDataItem);
			}
		}

		private async Task HandleSortingClick(IEnumerable<SortingItem<TItemType>> sorting)
		{
			if (await SetCurrentSortingWithEventCallback(CurrentUserState.Sorting?.ApplySorting(sorting.ToArray()) ?? sorting.ToList().AsReadOnly())) // when current sorting is null, use new sorting
			{
				await RefreshDataCoreAsync();
			}
		}

		private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
		{
			if (await SetCurrentPageIndexWithEventCallback(newPageIndex))
			{
				await RefreshDataCoreAsync();
			}
		}

		/// <summary>
		/// Instructs the component to re-request data from its <see cref="ItemsProvider"/>.
		/// This is useful if external data may have changed. There is no need to call this
		/// when using <see cref="Items"/>.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
		public async Task RefreshDataAsync()
		{
			// We don't auto-render after this operation because in the typical use case, the
			// host component calls this from one of its lifecycle methods, and will naturally
			// re-render afterwards anyway. It's not desirable to re-render twice.
			await RefreshDataCoreAsync(renderOnSuccess: false);
		}

		private async ValueTask RefreshDataCoreAsync(bool renderOnSuccess = false)
		{
			refreshDataCancellationTokenSource?.Cancel();
			refreshDataCancellationTokenSource?.Dispose();
			refreshDataCancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = refreshDataCancellationTokenSource.Token;

			GridDataProviderRequest<TItemType> request = new GridDataProviderRequest<TItemType>
			{
				PageIndex = CurrentUserState.PageIndex,
				PageSize = PageSize,
				Sorting = CurrentUserState.Sorting,
				CancellationToken = cancellationToken
			};

			GridDataProviderResult<TItemType> result;
			try
			{
				result = await dataProvider.Invoke(request);
			}
			catch (OperationCanceledException operationCanceledException) when (operationCanceledException.CancellationToken == cancellationToken)
			{
				// NOOP, we are the one who canceled the token
				return;
			}

			if (!cancellationToken.IsCancellationRequested)
			{
				// do not use result from cancelled request (for the case that user does not use the cancellation token
				Contract.Assert<InvalidOperationException>(result.Data != null, $"DataProvider did not set value to property {nameof(GridDataProviderResult<object>.Data)}. It cannot be null.");
				Contract.Assert<InvalidOperationException>(result.DataItemsTotalCount != null, $"DataProvider did not set value to property {nameof(GridDataProviderResult<object>.DataItemsTotalCount)}. It cannot be null.");

				dataItemsToRender = result.Data.ToList();
				dataItemsTotalCount = result.DataItemsTotalCount;

				if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, default))
				{
					if (!dataItemsToRender.Contains(SelectedDataItem))
					{
						await SetSelectedDataItemWithEventCallback(default);
					}
				}

				if (SelectedDataItems?.Count > 0)
				{
					HashSet<TItemType> selectedDataItems = dataItemsToRender.Intersect(SelectedDataItems).ToHashSet();
					await SetSelectedDataItemsWithEventCallback(selectedDataItems);
				}

				if (renderOnSuccess)
				{
					StateHasChanged();
				}
			}
		}

		private ValueTask<GridDataProviderResult<TItemType>> EnumerableDataProvider(GridDataProviderRequest<TItemType> request)
		{
			IEnumerable<TItemType> resultData = Data ?? Enumerable.Empty<TItemType>();

			#region AutoSorting
			bool autoSortEffective;
			if (AutoSort.HasValue)
			{
				autoSortEffective = AutoSort.Value;
			}
			else
			{
				// Default: Auto-sorting is enabled when all sortings have SortKeySelector.
				var definedSortings = columnsList.SelectMany(column => column.GetSorting());
				autoSortEffective = definedSortings.Any() && definedSortings.All(sorting => sorting.SortKeySelector != null);
			}
			if (autoSortEffective)
			{
				Debug.Assert(request.Sorting is not null);
				Contract.Assert(request.Sorting.All(item => item.SortKeySelector != null), "All sorting items must have set SortKeySelector property.");

				IOrderedEnumerable<TItemType> orderedData = (request.Sorting[0].SortDirection == SortDirection.Ascending)
					? resultData.OrderBy(request.Sorting[0].SortKeySelector.Compile())
					: resultData.OrderByDescending(request.Sorting[0].SortKeySelector.Compile());

				for (int i = 1; i < request.Sorting.Count; i++)
				{
					orderedData = (request.Sorting[i].SortDirection == SortDirection.Ascending)
						? orderedData.ThenBy(request.Sorting[i].SortKeySelector.Compile())
						: orderedData.ThenByDescending(request.Sorting[i].SortKeySelector.Compile());
				}

				resultData = orderedData;
			}
			#endregion

			#region AutoPaging
			if (PageSize > 0)
			{
				resultData = resultData.Skip(PageSize * request.PageIndex).Take(PageSize);
			}
			#endregion

			return new ValueTask<GridDataProviderResult<TItemType>>(new GridDataProviderResult<TItemType>
			{
				Data = resultData.ToList(),
				DataItemsTotalCount = resultData.Count()
			});
		}

		#region MultiSelect events
		private async Task HandleMultiSelectSelectDataItemClicked(TItemType selectedDataItem)
		{
			Contract.Requires(MultiSelectionEnabled);

			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItemType>();
			if (selectedDataItems.Add(selectedDataItem))
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}

		private async Task HandleMultiSelectUnselectDataItemClicked(TItemType selectedDataItem)
		{
			Contract.Requires(MultiSelectionEnabled);
			
			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItemType>();
			if (selectedDataItems.Remove(selectedDataItem))
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}

		private async Task HandleMultiSelectSelectAllClicked()
		{
			Contract.Requires(MultiSelectionEnabled);
			
			await SetSelectedDataItemsWithEventCallback(new HashSet<TItemType>(dataItemsToRender));
		}

		private async Task HandleMultiSelectSelectNoneClicked()
		{
			Contract.Requires(MultiSelectionEnabled);
			
			await SetSelectedDataItemsWithEventCallback(new HashSet<TItemType>());
		}
		#endregion

		/// <inheritdoc />
		public void Dispose()
		{
			refreshDataCancellationTokenSource?.Cancel();
			refreshDataCancellationTokenSource?.Dispose();
			refreshDataCancellationTokenSource = null;

			isDisposed = true;
		}
	}
}
