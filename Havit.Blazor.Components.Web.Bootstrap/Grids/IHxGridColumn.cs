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
		/// Indicates whether the column is visible (otherwise the column is hidden).
		/// It is not suitable to conditionaly display the column using @if statement in the markup code.
		/// </summary>
		bool IsVisible();

		/// <summary>
		/// Get column order (for scenarios where column order can be modified).
		/// Default should be 0.
		/// When columns have same order they should render in the order of their registration (Which is usually the same as the column appereance in the source code.
		/// But it differs when the column is displayed conditionaly using @if statement.).
		/// </summary>
		int GetOrder();

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
		/// Returns placeholder cell template.
		/// </summary>
		CellTemplate GetItemPlaceholderCellTemplate();

		/// <summary>
		/// Returns footer cell template.
		/// </summary>
		CellTemplate GetFooterCellTemplate();
	}
}
