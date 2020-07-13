using Havit.Collections;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	// Jak pohodlně definovat Default SortExpression? Asi na sloupci. Více sloupců? V renderu? Jak s živnotním cyklem? Načíst data, render, sorting, načíst data?

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

		// TODO: OnClickBehavior nebo tak něco?
		// [Parameter] public bool AllowSelection { get; set; }

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
		/// Intended for for data binding.
		/// </summary>		
		/// TODO: Items vs. Selected*Data*Item
		[Parameter] public TItemType SelectedDataItem { get; set; }

		/// <summary>
		/// Event fires when selected data item changes.
		/// Intended for for data binding.
		/// </summary>		
		[Parameter] public EventCallback<TItemType> SelectedDataItemChanged { get; set; }

		/// <summary>
		/// Indicates whether to display footer. Default is true.
		/// </summary>
		[Parameter] public bool ShowFooter { get; set; } = true;

		/// <summary>
		/// Page size.
		/// </summary>
		[Parameter] public int PageSize { get; set; } = 0;

		/// <summary>
		/// Current page index.
		/// Intended for for data binding.
		/// </summary>
		[Parameter] public int CurrentPageIndex { get; set; }

		/// <summary>
		/// Event fires when selected page changes.
		/// Intended for for data binding.
		/// </summary>		
		[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }

		/// <summary>
		/// Indicates whether to display button to show all pages. Default is true.
		/// </summary>
		[Parameter] public bool PagerShowAllButton { get; set; } = true;

		/// <summary>
		/// Current grid sorting.
		/// </summary>
		[Parameter] public SortingItem<TItemType>[] CurrentSorting { get; set; }

		/// <summary>
		/// Event fires when sorting changes.
		/// </summary>
		[Parameter] public EventCallback<SortingItem<TItemType>[]> CurrentSortingChanged { get; set; }

		/// <summary>
		/// When true, automatically sorts the data in <see cref="Items"/> property. Default is true.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true; // TODO: Necháme to jako default? Co bude typičtější? Řazení a stránkování přes API na serveru nebo lokálně?

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
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItemType>>(columnsList, this.StateHasChanged, () => isDisposed, HandleColumnAdded);
		}

		private void HandleColumnAdded(IHxGridColumn<TItemType> column)
		{
			if (AutoSort)
			{
				Contract.Assert(column.GetSorting().All(item => item.SortExpression != null), "For AutoSort all sorting items must have SortExpression set.");
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			
			// TODO: Přesunout nastavení před render, parametry máme už dříve.
			if (decreasePageIndexAfterRender)
			{
				decreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentPageIndex - 1);
				StateHasChanged();
			}
		}

		/// <summary>
		/// Returns columns to render.
		/// Main goal of the method is to add ContextMenuGridColumn to the user defined columns.
		/// </summary>
		protected List<IHxGridColumn<TItemType>> GetColumnsToRender()
		{
			var result = new List<IHxGridColumn<TItemType>>(columnsList);
			if ((result.Count > 0) && (ContextMenu != null))
			{
				result.Add(new ContextMenuGridColumn<TItemType>(ContextMenu));
			}
			return result;
		}

		/// <summary>
		/// Applies paging on the source data when possible.
		/// </summary>
		protected virtual IEnumerable<TItemType> ApplyPaging(IEnumerable<TItemType> source)
		{
			return (PageSize > 0)
				? source.Skip(PageSize * CurrentPageIndex).Take(PageSize)
				: source;
		}

		/// <summary>
		/// Applies sorting on the source data when possible.
		/// </summary>
		protected virtual IEnumerable<TItemType> ApplySorting(IEnumerable<TItemType> source)
		{
			if ((CurrentSorting == null) || (CurrentSorting.Length == 0) || !AutoSort)
			{
				// no sorting applied
				return source;
			}

			Contract.Assert(CurrentSorting.All(item => item.SortExpression != null), "All sorting items must have set SortExpression property.");

			IOrderedEnumerable<TItemType> result = (CurrentSorting[0].SortDirection == SortDirection.Ascending)
				? source.OrderBy(CurrentSorting[0].SortExpression.Compile())
				: source.OrderByDescending(CurrentSorting[0].SortExpression.Compile());

			for (int i = 1; i < CurrentSorting.Length; i++)
			{
				result = (CurrentSorting[i].SortDirection == SortDirection.Ascending)
					? result.ThenBy(CurrentSorting[i].SortExpression.Compile())
					: result.ThenByDescending(CurrentSorting[i].SortExpression.Compile());
			}

			return result;
		}

		private async Task SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentPageIndex != newPageIndex)
			{
				CurrentPageIndex = newPageIndex;
				await CurrentPageIndexChanged.InvokeAsync(newPageIndex);
			}
		}

		private async Task HandleSelectDataItemClick(TItemType newSelectedDataItem)
		{
			if (!EqualityComparer<TItemType>.Default.Equals(SelectedDataItem, newSelectedDataItem))
			{
				SelectedDataItem = newSelectedDataItem;
				await SelectedDataItemChanged.InvokeAsync(newSelectedDataItem);
			}
		}

		private async Task HandleSortingClick(IEnumerable<SortingItem<TItemType>> sorting)
		{
			CurrentSorting = CurrentSorting?.ApplySorting(sorting.ToArray()) ?? sorting.ToArray(); // when cu
			await CurrentSortingChanged.InvokeAsync(CurrentSorting);
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
