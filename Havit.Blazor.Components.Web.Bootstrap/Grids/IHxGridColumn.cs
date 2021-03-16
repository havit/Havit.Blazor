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
	public interface IHxGridColumn<TItem> : IRenderNotificationComponent
	{
		/// <summary>
		/// Sorting of the column.
		/// </summary>
		SortingItem<TItem>[] GetSorting();

		/// <summary>
		/// Returns header cell template.
		/// </summary>
		CellTemplate GetHeaderCellTemplate();

		/// <summary>
		/// Returns data cell template for the specific item.
		/// </summary>
		CellTemplate GetItemCellTemplate(TItem item);

		/// <summary>
		/// Returns footer cell template.
		/// </summary>
		CellTemplate GetFooterCellTemplate();
	}
}
