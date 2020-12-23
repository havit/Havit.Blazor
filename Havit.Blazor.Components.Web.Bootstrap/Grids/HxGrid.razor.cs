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

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Údržba SelectedItem a SelectedItems: Kdy by mělo dojít k údržbě? V každém renderu? I při stránkování na straně serveru? 
	// TODO: Co když se tedy při stránkování ztrácí záznamy vybrané na předchozí stránce?
	// TODO: A naopak: Abychom při stránkování poznali stejné záznamy, musíme být schopni je porovnat. 
	// TODO: Jenže to klade nároky na implementace IEquatable<>, což asi těžko bude někdo implementovat hromadně.	

	/// <summary>
	/// Grid to display tabular data from data source.
	/// </summary>
	/// <typeparam name="TItemType">Type of row data item.</typeparam>
	public partial class HxGrid<TItemType> : IDisposable
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
		/// Grid view selection mode. Default is "Select".
		/// </summary>
		[Parameter] public GridSelectionMode SelectionMode { get; set; }

		/// <summary>
		/// Columns template.
		/// </summary>
		[Parameter] public RenderFragment Columns { get; set; }

		/// <summary>
		/// Context menu template.
		/// </summary>
		[Parameter] public RenderFragment<TItemType> ContextMenu { get; set; }

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
		/// Default: Auto-sorting is enabled when all sortings on all columns have <c>SortKeySelector</c> />.
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
		

		private List<IHxGridColumn<TItemType>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration;
		private bool decreasePageIndexAfterRender = false;
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
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>((Data == null) || (DataProvider == null), $"{GetType()} can only accept one item source from its parameters. Do not supply both '{nameof(Data)}' and '{nameof(DataProvider)}'.");			

			dataProvider = DataProvider ?? EnumerableDataProvider;
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
				await RefreshDataToRenderCore(true);
			}

			// when rendering page with no data, navigate one page back
			if (decreasePageIndexAfterRender)
			{
				decreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentUserState.PageIndex - 1);
				await RefreshDataToRenderCore(true);
			}
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
			Contract.Requires(SelectionMode == GridSelectionMode.Select);

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
				await RefreshDataToRenderCore();
			}
		}

		private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
		{
			if (await SetCurrentPageIndexWithEventCallback(newPageIndex))
			{
				await RefreshDataToRenderCore();
			}
		}

		private async ValueTask RefreshDataToRenderCore(bool renderOnSuccess = false)
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
			bool autoSortEffective = AutoSort ?? columnsList.SelectMany(column => column.GetSorting()).All(sorting => sorting.SortKeySelector != null);
			if (autoSortEffective)
			{
				Contract.Assert(request.Sorting.All(item => item.SortKeySelector != null), "All sorting items must have set SortKeySelector property.");

				if ((request.Sorting == null) || (request.Sorting.Count == 0))
				{
					// no sorting applied
				}
				else
				{
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
				DataItemsTotalCount = Data.Count()
			});
		}

		#region MultiSelect events
		private async Task HandleMultiSelectSelectDataItemClicked(TItemType selectedDataItem)
		{
			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItemType>();
			if (selectedDataItems.Add(selectedDataItem))
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}

		private async Task HandleMultiSelectUnselectDataItemClicked(TItemType selectedDataItem)
		{
			var selectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItemType>();
			if (selectedDataItems.Remove(selectedDataItem))
			{
				await SetSelectedDataItemsWithEventCallback(selectedDataItems);
			}
		}

		private async Task HandleMultiSelectSelectAllClicked()
		{
			await SetSelectedDataItemsWithEventCallback(new HashSet<TItemType>(dataItemsToRender));
		}

		private async Task HandleMultiSelectSelectNoneClicked()
		{
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
