using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class ContextMenuGridColumn<TItem> : HxGridColumnBase<TItem>
	{
		[Parameter] public RenderFragment<TItem> ContextMenu { get; set; }

		protected override CellTemplate GetHeaderCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override CellTemplate GetItemCellTemplate(TItem item) => new CellTemplate(ContextMenu(item));

		protected override CellTemplate GetFooterCellTemplate() => new CellTemplate(RenderFragmentBuilder.Empty());

		protected override IEnumerable<SortingItem<TItem>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItem>>();
		}
	}
}
