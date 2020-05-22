using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public interface IHxGridColumn<TItemType>
	{   // TODO: CssClass? (možnost zarovnání, atp.): Implementace
		// a) GetHeaderCssClass nebo
		// b) GetHeaderTemplate nebude vracet RenderFragment, ale třídu o dvou vlastnostech (string nebo Func<TItemType, string> pro css classu a RenderFragment pro buňku.
		// Preferovaná je varianta b), neboť v případě dalších rozšíření nepovede k haldě dalších metod.
		IEnumerable<SortingItem<TItemType>> GetSorting();
		RenderFragment GetHeaderTemplate();
		RenderFragment GetItemTemplate(TItemType item);
		RenderFragment GetFooterTemplate();
	}
}
