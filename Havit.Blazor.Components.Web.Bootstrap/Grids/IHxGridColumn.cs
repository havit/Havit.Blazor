using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid column.
	/// </summary>
	public interface IHxGridColumn<TItemType> : IRenderNotificationComponent
	{
		/// <summary>
		/// Sorting of the column.
		/// </summary>
		SortingItem<TItemType>[] GetSorting();

		/// <summary>
		/// Returns header cell template.
		/// </summary>
		CellTemplate GetHeaderCellTemplate();

		/// <summary>
		/// Returns data cell template for the specific item.
		/// </summary>
		CellTemplate GetItemCellTemplate(TItemType item);

		/// <summary>
		/// Returns footer cell template.
		/// </summary>
		CellTemplate GetFooterCellTemplate();
	}
}
