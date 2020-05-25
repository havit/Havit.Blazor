using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public interface IHxGridColumn<TItemType>
	{  
		IEnumerable<SortingItem<TItemType>> GetSorting();
		CellTemplate GetHeaderCellTemplate();
		CellTemplate GetItemCellTemplate(TItemType item);
		CellTemplate GetFooterCellTemplate();
	}
}
