using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public class MultiSelectGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		[Parameter] public HashSet<TItemType> SelectedDataItems { get; set; }
		[Parameter] public EventCallback<TItemType> ItemSelectionToggled { get; set; }

		protected override bool AutoRegisterColumn => false;

		protected override CellTemplate GetHeaderCellTemplate()
		{
			return new CellTemplate(RenderFragmentBuilder.Empty());
		}

		protected override CellTemplate GetItemCellTemplate(TItemType item)
		{
			return new CellTemplate((RenderTreeBuilder builder) =>
			{
				builder.OpenElement(100, "div");
				builder.AddAttribute(101, "class", "form-check form-check-inline");

				builder.OpenElement(200, "input");
				builder.AddAttribute(201, "type", "checkbox");
				builder.AddAttribute(202, "class", "form-check-input");

				builder.AddAttribute(203, "checked", SelectedDataItems?.Contains(item) ?? false);
				builder.AddAttribute(204, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, async (ChangeEventArgs args) => { await ItemSelectionToggled.InvokeAsync(item); }));

				builder.CloseElement(); // input

				builder.CloseElement(); // div
			});
		}

		protected override CellTemplate GetFooterCellTemplate()
		{
			return new CellTemplate(RenderFragmentBuilder.Empty());
		}

		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItemType>>();
		}
	}
}
