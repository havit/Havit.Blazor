using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	internal class ContextMenuGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		public RenderFragment<TItemType> ItemTemplate { get; }

		public ContextMenuGridColumn(RenderFragment<TItemType> itemTemplate)
		{
			ItemTemplate = itemTemplate;
		}

		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override CellTemplate GetItemCellTemplate(TItemType item) => new CellTemplate(ItemTemplate(item));

		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItemType>>();
		}
	}
}
