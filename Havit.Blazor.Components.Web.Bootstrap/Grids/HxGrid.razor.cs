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
		/// Indicates total number of items for server-side paging.
		/// </summary>
		[Parameter] public int? TotalItemsCount { get; set; }

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

		/// <summary>
		/// Event fires when data reload is required. It is when
		/// <list type="bullet">
		/// <item>Current sorting is changed (includes initial set of default sorting).</item>
		/// <item>Current page index is changes.</item>
		/// </list>
		/// </summary>
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

			// when no sorting is set, use default
			if (firstRender && (CurrentSorting == null))
			{
				SortingItem<TItemType>[] defaultSorting = GetDefaultSorting();
				if (defaultSorting != null)
				{
					await SetCurrentSortingWithEventCallback(defaultSorting);
				}
			}

			if (firstRender && (this.Items == null))
			{
				await DataReloadRequired.InvokeAsync(GetCurrentUserState());
			}

			// when rendering page with no data, navigate one page back
			if (decreasePageIndexAfterRender)
			{
				decreasePageIndexAfterRender = false;
				await SetCurrentPageIndexWithEventCallback(CurrentPageIndex - 1);
			}

			// No StateHasChanged required - all reactions are performed by parameter changes which already causes re-rendering.
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

				Contract.Assert(defaultSorting.Length > 0, "There should be specified default sorting.");

				return defaultSorting;
			}

			return null;
		}

		/// <summary>
		/// Applies paging on the source data when possible.
		/// </summary>
		protected virtual IEnumerable<TItemType> ApplyPaging(IEnumerable<TItemType> source)
		{
			return ((PageSize > 0) && (TotalItemsCount == null))
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

		private async Task SetCurrentSortingWithEventCallback(SortingItem<TItemType>[] newSorting)
		{
			CurrentSorting = newSorting;
			await CurrentSortingChanged.InvokeAsync(newSorting);

			if (!AutoSort)
			{
				await DataReloadRequired.InvokeAsync(GetCurrentUserState());
			}
		}

		private async Task SetCurrentPageIndexWithEventCallback(int newPageIndex)
		{
			if (CurrentPageIndex != newPageIndex)
			{
				CurrentPageIndex = newPageIndex;
				await CurrentPageIndexChanged.InvokeAsync(newPageIndex);

				if (TotalItemsCount != null)
				{
					await DataReloadRequired.InvokeAsync(GetCurrentUserState());
				}
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
			await SetCurrentSortingWithEventCallback(CurrentSorting?.ApplySorting(sorting.ToArray()) ?? sorting.ToArray()); // when current sorting is null, use new sorting
		}

		private async Task HandlePagerCurrentPageIndexChanged(int newPageIndex)
		{
			await SetCurrentPageIndexWithEventCallback(newPageIndex);
		}

		private GridUserState<TItemType> GetCurrentUserState()
		{
			return new GridUserState<TItemType>(this.CurrentPageIndex, this.CurrentSorting);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
