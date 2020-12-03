using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
		/// Items to render as a table.
		/// </summary>
		[Parameter] public IEnumerable<TItemType> Items { get; set; }

		/// <summary>
		/// Grid view selection mode. Default is "Select".
		/// </summary>
		[Parameter] public GridViewSelectionMode SelectionMode { get; set; }

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
		/// Indicates total number of items for server-side paging.
		/// </summary>
		[Parameter] public int? TotalItemsCount { get; set; }

		/// <summary>
		/// Enable/disable in-memory auto-sorting the data in <see cref="Items"/> property.
		/// Default: Auto-sorting is enabled when all sortings on all columns have <see cref="SortingItem{TItemType}.SortKeySelector"/>.
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

		/// <summary>
		/// Event fires when data reload is required. It is when
		/// <list type="bullet">
		/// <item>Current sorting is changed (includes initial set of default sorting).</item>
		/// <item>Current page index is changes.</item>
		/// </list>
		/// </summary>
		/// <remarks>
		/// It sounds like it is enought to have just <see cref="CurrentUserState"/> but there are reason why it is not true:
		/// <list type="number">
		/// <item>Consider usage of the grid - <see cref="CurrentUserState"/> and <see cref="CurrentUserStateChanged"/> are used by data-binding and no other event can be attached.</item>
		/// <item>When there is no default sort <see cref="CurrentUserStateChanged"/> is not fired so there would not be simple place to attach initial data load.</item>
		/// </list>
		/// </remarks>
		[Parameter] public EventCallback<GridUserState<TItemType>> DataReloadRequired { get; set; }

		private List<IHxGridColumn<TItemType>> columnsList;
		private CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration;
		private bool decreasePageIndexAfterRender = false;
		private bool isDisposed = false;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxGrid()
		{
			columnsList = new List<IHxGridColumn<TItemType>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItemType>>(columnsList, this.StateHasChanged, () => isDisposed);
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

			if (firstRender && (this.Items == null))
			{
				await DataReloadRequired.InvokeAsync(CurrentUserState);
			}

			// when rendering page with no data, navigate one page back
			if (decreasePageIndexAfterRender)
			{
				decreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentUserState.PageIndex - 1);
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

		/// <summary>
		/// Returns effective value of autosort.
		/// When AutoSort is not set return true when all sortings in all columns has SortKeySelector.
		/// </summary>
		private bool GetAutoSortEffective()
		{
			return AutoSort ?? columnsList.SelectMany(column => column.GetSorting()).All(sorting => sorting.SortKeySelector != null);
		}

		/// <summary>
		/// Applies paging on the source data when possible.
		/// </summary>
		protected virtual IEnumerable<TItemType> ApplyPaging(IEnumerable<TItemType> source)
		{
			return ((PageSize > 0) && (TotalItemsCount == null))
				? source.Skip(PageSize * CurrentUserState.PageIndex).Take(PageSize)
				: source;
		}

		/// <summary>
		/// Applies sorting on the source data when possible.
		/// </summary>
		protected virtual IEnumerable<TItemType> ApplySorting(IEnumerable<TItemType> source)
		{
			if ((CurrentUserState.Sorting == null) || (CurrentUserState.Sorting.Count == 0) || !GetAutoSortEffective())
			{
				// no sorting applied
				return source;
			}

			Contract.Assert(CurrentUserState.Sorting.All(item => item.SortKeySelector != null), "All sorting items must have set SortKeySelector property.");

			IOrderedEnumerable<TItemType> result = (CurrentUserState.Sorting[0].SortDirection == SortDirection.Ascending)
				? source.OrderBy(CurrentUserState.Sorting[0].SortKeySelector.Compile())
				: source.OrderByDescending(CurrentUserState.Sorting[0].SortKeySelector.Compile());

			for (int i = 1; i < CurrentUserState.Sorting.Count; i++)
			{
				result = (CurrentUserState.Sorting[i].SortDirection == SortDirection.Ascending)
					? result.ThenBy(CurrentUserState.Sorting[i].SortKeySelector.Compile())
					: result.ThenByDescending(CurrentUserState.Sorting[i].SortKeySelector.Compile());
			}

			return result;
		}

		private async Task SetCurrentSortingWithEventCallback(IReadOnlyList<SortingItem<TItemType>> newSorting)
		{
			CurrentUserState = new GridUserState<TItemType>(CurrentUserState.PageIndex, newSorting);

			await CurrentUserStateChanged.InvokeAsync(CurrentUserState);

			if (!GetAutoSortEffective())
			{
				await DataReloadRequired.InvokeAsync(CurrentUserState);
			}
		}

		private async Task SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentUserState.PageIndex != newPageIndex)
			{
				CurrentUserState = new GridUserState<TItemType>(newPageIndex, CurrentUserState.Sorting);
				await CurrentUserStateChanged.InvokeAsync(CurrentUserState);

				if (TotalItemsCount != null)
				{
					await DataReloadRequired.InvokeAsync(CurrentUserState);
				}
			}
		}

		private async Task HandleSelectDataItemClick(TItemType newSelectedDataItem)
		{
			Contract.Requires(SelectionMode == GridViewSelectionMode.Select);

			if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, newSelectedDataItem))
			{
				SelectedDataItem = newSelectedDataItem;
				await SelectedDataItemChanged.InvokeAsync(newSelectedDataItem);
			}
		}

		private async Task HandleMultiSelectItemSelectionToggled(TItemType item)
		{
			Contract.Requires(SelectionMode == GridViewSelectionMode.MultiSelect);

			SelectedDataItems = SelectedDataItems?.ToHashSet() ?? new HashSet<TItemType>();
			if (!SelectedDataItems.Add(item))
			{
				SelectedDataItems.Remove(item);
			}
			await SelectedDataItemsChanged.InvokeAsync(SelectedDataItems);
		}

		private async Task HandleSortingClick(IEnumerable<SortingItem<TItemType>> sorting)
		{
			await SetCurrentSortingWithEventCallback(CurrentUserState.Sorting?.ApplySorting(sorting.ToArray()) ?? sorting.ToList().AsReadOnly()); // when current sorting is null, use new sorting
		}

		private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
		{
			await SetCurrentPageIndexWithEventCallback(newPageIndex);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
