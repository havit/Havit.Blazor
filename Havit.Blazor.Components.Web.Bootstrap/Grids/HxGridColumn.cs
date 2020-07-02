using Havit.Collections;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public class HxGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		// TODO: Suppress SA1134 je v CSPROJ, uvolnit celofiremně?
		[Parameter] public string HeaderText { get; set; }
		[Parameter] public RenderFragment HeaderTemplate { get; set; }
		[Parameter] public string HeaderCssClass { get; set; }

		[Parameter] public Func<TItemType, string> ItemText { get; set; } // TODO: Na rozdíl od běžných vlastností Text tato není typu string, Func<TItemType, string>!
		[Parameter] public RenderFragment<TItemType> ItemTemplate { get; set; }
		[Parameter] public string ItemCssClass { get; set; }
		[Parameter] public Func<TItemType, string> ItemCssClassTemplate { get; set; } // TODO: Pojmenovávací peklo, všude je to jinak, tohle není Template, není to RenderFragment

		[Parameter] public string FooterText { get; set; }
		[Parameter] public RenderFragment FooterTemplate { get; set; }
		[Parameter] public string FooterCssClass { get; set; }

		[Parameter] public string SortString { get; set; }
		[Parameter] public Expression<Func<TItemType, IComparable>> SortExpression { get; set; }
		[Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(HeaderText, HeaderTemplate), HeaderCssClass);

		protected override CellTemplate GetItemCellTemplate(TItemType item)
		{
			string cssClass = (ItemCssClassTemplate == null)
				? ItemCssClass // nemáme ItemCssClassTemplate, použijeme ItemCssClass, i kdyby to bylo null
				: String.IsNullOrEmpty(ItemCssClass) // máme ItemCssClassTemplate
					? ItemCssClassTemplate(item) // a pokud nemáme ItemCssClass, tak použijeme hodnotu z template
					: ItemCssClass + " " + ItemCssClassTemplate(item); // jinak poskládáme ItemCssClass a ItemCssClassTemplate za sebe
			return new CellTemplate(RenderFragmentBuilder.CreateFrom(ItemText?.Invoke(item), ItemTemplate?.Invoke(item)), cssClass);
		}

		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.CreateFrom(FooterText, FooterTemplate), FooterCssClass);

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
