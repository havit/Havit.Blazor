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
	/// Grid to display tabular data from data source.
	/// </summary>
	/// <typeparam name="TItemType">Type of row data item.</typeparam>
	public partial class HxGrid<TItemType> : ComponentBase, ICascadeProgressComponent, IDisposable
	{
		/// <summary>
		/// ColumnsRegistration cascading value name.
		/// </summary>
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		/// <summary>
		/// Data provider for items to render as a table.
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
		[Parameter] public RenderFragment EmptyDataTemplate { get; set; }

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
		/// Indicates whether to render footer when data are empty.
		/// </summary>
		[Parameter] public bool ShowFooterWhenEmptyData { get; set; } = false;

		/// <summary>
		/// Current grid state (page, sorting).
		/// </summary>
		[Parameter] public GridUserState<TItemType> CurrentUserState { get; set; } = new GridUserState<TItemType>(0, null);

		/// <summary>
		/// Event fires when grid state is changed.
		/// </summary>
		[Parameter] public EventCallback<GridUserState<TItemType>> CurrentUserStateChanged { get; set; }

		/// <inheritdoc />
		[CascadingParameter] public ProgressState ProgressState { get; set; }
		
		/// <inheritdoc />
		[Parameter] public bool? InProgress { get; set; }
		
		[Inject] private IStringLocalizer<HxGrid> HxGridLocalizer { get; set; } // private: non-generic HxGrid grid is internal, so the property cannot have wider accessor (protected)

		private List<IHxGridColumn<TItemType>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration;
		private bool decreasePageIndexAfterRender = false;
		private bool isDisposed = false;
		private List<TItemType> dataItemsToRender;
		private int? dataItemsTotalCount;
		private CancellationTokenSource refreshDataCancellationTokenSource;
		private bool dataProviderInProgress;

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

			Contract.Requires<InvalidOperationException>(DataProvider != null, $"Property {nameof(DataProvider)} on {GetType()} must have a value.");
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

			await SetSelectedDataItemWithEventCallback(newSelectedDataItem);
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
			// Multithreading: we can safelly set dataProviderInProgress, always dataProvider is going to retrieve data we are it is in in a progress.
			dataProviderInProgress = true; 
			StateHasChanged();
			try
			{
				result = await DataProvider.Invoke(request);
			}
			catch (OperationCanceledException operationCanceledException) when (operationCanceledException.CancellationToken == cancellationToken)
			{
				// NOOP, we are the one who canceled the token
				return;
			}

			// do not use result from cancelled request (for the case a developer does not use the cancellation token)
			if (!cancellationToken.IsCancellationRequested)
			{
				dataProviderInProgress = false; // Multithreading: we can safelly clean dataProviderInProgress only wnen received data from non-cancelled task

				#region Verify paged data information
				if ((result.Data != null) && (this.PageSize > 0))
				{
					int dataCount = result.Data.Count();

					if (dataCount > this.PageSize)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} returned more data items then is the size od the page.");
					}

					if (result.DataItemsTotalCount == null)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} did not set ${nameof(GridDataProviderResult<TItemType>.DataItemsTotalCount)}.");
					}
					else if (dataCount > result.DataItemsTotalCount.Value)
					{
						throw new InvalidOperationException($"{nameof(DataProvider)} set ${nameof(GridDataProviderResult<TItemType>.DataItemsTotalCount)} property byt the value is smaller than the number of data items.");
					}
				}
				#endregion

				dataItemsToRender = result.Data?.ToList();
				dataItemsTotalCount = result.DataItemsTotalCount;

				if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, default))
				{
					if ((dataItemsToRender == null) || !dataItemsToRender.Contains(SelectedDataItem))
					{
						await SetSelectedDataItemWithEventCallback(default);
					}
				}

				if (SelectedDataItems?.Count > 0)
				{
					HashSet<TItemType> selectedDataItems = dataItemsToRender?.Intersect(SelectedDataItems).ToHashSet() ?? new HashSet<TItemType>();
					await SetSelectedDataItemsWithEventCallback(selectedDataItems);
				}				

				if (renderOnSuccess)
				{
					StateHasChanged();
				}
			}
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
