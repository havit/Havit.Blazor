using Havit.Collections;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	// Přesunout do Havit.Blazor.Components.Web? Ale pozor na závislost na Pageru!
	// Jak pohodlně definovat Default SortExpression? Asi na sloupci. Více sloupců?
	public partial class HxGrid<TItemType>
	{
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		[Parameter] public IEnumerable<TItemType> Data { get; set; } // TODO: Items? DataSource :-)
		[Parameter] public bool AllowSelection { get; set; } // TODO: OnClickBehavior nebo tak něco?
		[Parameter] public bool AllowSorting { get; set; } // TODO
		[Parameter] public RenderFragment Columns { get; set; }
		[Parameter] public RenderFragment<TItemType> ContextMenu { get; set; }
		[Parameter] public TItemType SelectedDataItem { get; set; } // TODO
		[Parameter] public EventCallback<TItemType> SelectedDataItemChanged { get; set; }
		[Parameter] public bool ShowFooter { get; set; } = true;
		[Parameter] public int PageSize { get; set; } = 0;
		[Parameter] public int CurrentPageIndex { get; set; }
		[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }
		[Parameter] public bool PagerShowAllButton { get; set; } = true;
		[Parameter] public SortingItem<TItemType>[] CurrentSorting { get; set; } // TODO: Vypořádat se s null hodnotou pro binding
		[Parameter] public EventCallback<SortingItem<TItemType>[]> CurrentSortingChanged { get; set; } // TODO
		[Parameter] public bool AutoSort { get; set; } = true; // Necháme to jako default? Co bude typičtější? Řazení a stránkování přes API na serveru nebo lokálně?

		private List<IHxGridColumn<TItemType>> columnsList;
		protected CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration; // protected: The field 'HxGrid<TItemType>.columnsListRegistration' is never used

		public HxGrid()
		{ 
			CurrentSorting = new SortingItem<TItemType>[0];
			columnsList = new List<IHxGridColumn<TItemType>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItemType>>(columnsList, this.StateHasChanged);
		}

		/// <summary>
		/// Vrací sloupce k vyrenderování.
		/// Hlavním účelem je doplnění sloupce pro menu k uživatelsky definovaným sloupcům
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

		private void PagerShowAllButtonClicked()
		{
			PageSize = 0; // TODO: implementovat přes pochopitelnější stavové (třeba odvozené) proměnné
		}

		private async Task SetSorting(IEnumerable<SortingItem<TItemType>> sorting)
		{
			CurrentSorting = CurrentSorting.ApplySorting(sorting.ToArray());
			await CurrentSortingChanged.InvokeAsync(CurrentSorting);
		}

		protected virtual IEnumerable<TItemType> ApplyPaging(IEnumerable<TItemType> source)
		{
			return (PageSize > 0)
				? source.Skip(PageSize * CurrentPageIndex).Take(PageSize)
				: source;
		}

		protected virtual IEnumerable<TItemType> ApplySorting(IEnumerable<TItemType> source)
		{
			if ((CurrentSorting.Length == 0) || !AutoSort)
			{
				return source;
			}

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
	}
}
