using Havit.Collections;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	/// <summary>
	/// Grid column.
	/// </summary>
	/// <typeparam name="TItemType"></typeparam>
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
		[Parameter] public Func<TItemType, string> ItemText { get; set; } // TODO: Na rozdíl od běžných vlastností Text tato není typu string, Func<TItemType, string>!

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
		[Parameter] public Func<TItemType, string> ItemCssClassTemplate { get; set; } // TODO: Pojmenovávací peklo, všude je to jinak, tohle není Template, není to RenderFragment
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
		[Parameter] public Expression<Func<TItemType, IComparable>> SortExpression { get; set; }

		/// <summary>
		/// Sort direction.
		/// </summary>
		[Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
		#endregion

		/// <inheritdoc />
		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(HeaderText, HeaderTemplate), HeaderCssClass);

		/// <inheritdoc />
		protected override CellTemplate GetItemCellTemplate(TItemType item)
		{
			string cssClass = (ItemCssClassTemplate == null)
				? ItemCssClass // nemáme ItemCssClassTemplate, použijeme ItemCssClass, i kdyby to bylo null
				: String.IsNullOrEmpty(ItemCssClass) // máme ItemCssClassTemplate
					? ItemCssClassTemplate(item) // a pokud nemáme ItemCssClass, tak použijeme hodnotu z template
					: ItemCssClass + " " + ItemCssClassTemplate(item); // jinak poskládáme ItemCssClass a ItemCssClassTemplate za sebe
			return new CellTemplate(RenderFragmentBuilder.CreateFrom(ItemText?.Invoke(item), ItemTemplate?.Invoke(item)), cssClass);
		}

		/// <inheritdoc />
		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(FooterText, FooterTemplate), FooterCssClass);

		/// <inheritdoc />
		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			if ((SortExpression == null) && String.IsNullOrEmpty(SortString))
			{
				yield break;
			}

			yield return new SortingItem<TItemType>(this.SortString, this.SortExpression, this.SortDirection);
		}
	}
}
