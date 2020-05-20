using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

		protected override RenderFragment GetHeaderTemplate() => RenderFragmentBuilder.Empty();

		protected override RenderFragment GetItemTemplate(TItemType item) => ItemTemplate(item);

		protected override RenderFragment GetFooterTemplate() => RenderFragmentBuilder.Empty();

	}
}
