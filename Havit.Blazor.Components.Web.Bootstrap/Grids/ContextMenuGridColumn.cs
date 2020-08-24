using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public class ContextMenuGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		[Parameter] public RenderFragment<TItemType> ContextMenu { get; set; }

		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override CellTemplate GetItemCellTemplate(TItemType item) => new CellTemplate(ContextMenu(item));

		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItemType>>();
		}
	}
}
