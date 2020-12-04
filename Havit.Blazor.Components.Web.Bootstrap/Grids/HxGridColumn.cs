using Havit.Collections;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid column.
	/// </summary>
	/// <typeparam name="TItemType">Grid row data type.</typeparam>
	public class HxGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		// TODO: Suppress SA1134 je v CSPROJ, uvolnit celofiremně?

		#region Header properties
		/// <summary>
		/// Header cell text.
		/// </summary>
		[Parameter] public string HeaderText { get; set; }

		/// <summary>
		/// Header cell template.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Header cell css class.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }
		#endregion

		#region Item properties
		/// <summary>
		/// Returns text for the item.
		/// </summary>
		[Parameter] public Func<TItemType, string> ItemTextSelector { get; set; }

		/// <summary>
		/// Returns template for the item.
		/// </summary>
		[Parameter] public RenderFragment<TItemType> ItemTemplate { get; set; }

		/// <summary>
		/// Returns item css class (not dependent on data).
		/// </summary>
		[Parameter] public string ItemCssClass { get; set; }

		/// <summary>
		/// Returns item css class for the specific date item.
		/// </summary>
		[Parameter] public Func<TItemType, string> ItemCssClassSelector { get; set; }
		#endregion

		#region Footer properties
		/// <summary>
		/// Footer cell text.
		/// </summary>
		[Parameter] public string FooterText { get; set; }

		/// <summary>
		/// Footer cell template.
		/// </summary>
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Footer cell css class.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }
		#endregion

		#region Sorting properties
		/// <summary>
		/// Returns column sorting as string.
		/// </summary>
		[Parameter] public string SortString { get; set; }

		/// <summary>
		/// Returns column sorting expression for automatic grid sorting.
		/// </summary>
		[Parameter] public Expression<Func<TItemType, IComparable>> SortKeySelector { get; set; }

		/// <summary>
		/// Sort direction.
		/// </summary>
		[Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

		/// <summary>
		/// Indicates the sorting on the column is default (primary) on the grid.
		/// </summary>
		[Parameter] public bool IsDefaultSortColumn { get; set; } = false;
		#endregion

		/// <inheritdoc />
		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(HeaderText, HeaderTemplate), HeaderCssClass);

		/// <inheritdoc />
		protected override CellTemplate GetItemCellTemplate(TItemType item)
		{
			string cssClass = CssClassHelper.Combine(ItemCssClass, ItemCssClassSelector?.Invoke(item));
			return new CellTemplate(RenderFragmentBuilder.CreateFrom(ItemTextSelector?.Invoke(item), ItemTemplate?.Invoke(item)), cssClass);
		}

		/// <inheritdoc />
		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(FooterText, FooterTemplate), FooterCssClass);

		/// <inheritdoc />
		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			if ((SortKeySelector == null) && String.IsNullOrEmpty(SortString))
			{
				yield break;
			}

			yield return new SortingItem<TItemType>(this.SortString, this.SortKeySelector, this.SortDirection, sortDefaultOrder: IsDefaultSortColumn ? 0 : (int?)null);
		}
	}
}
