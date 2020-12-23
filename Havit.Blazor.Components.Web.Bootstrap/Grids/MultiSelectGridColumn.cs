using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class MultiSelectGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		[Parameter] public HashSet<TItemType> SelectedDataItems { get; set; }
		[Parameter] public bool AllDataItemsSelected { get; set; }
		[Parameter] public EventCallback SelectAllClicked { get; set; }
		[Parameter] public EventCallback SelectNoneClicked { get; set; }
		[Parameter] public EventCallback<TItemType> SelectDataItemClicked { get; set; }
		[Parameter] public EventCallback<TItemType> UnselectDataItemClicked { get; set; }

		protected override CellTemplate GetHeaderCellTemplate()
		{
			return new CellTemplate((RenderTreeBuilder builder) =>
			{
				builder.OpenElement(100, "div");
				builder.AddAttribute(101, "class", "form-check form-check-inline");

				builder.OpenElement(200, "input");
				builder.AddAttribute(201, "type", "checkbox");
				builder.AddAttribute(202, "class", "form-check-input");

				builder.AddAttribute(203, "checked", AllDataItemsSelected);
				builder.AddAttribute(204, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, HandleSelectAllOrNoneClick));

				builder.CloseElement(); // input

				builder.CloseElement(); // div
			}, "text-center");
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

				bool selected = SelectedDataItems?.Contains(item) ?? false;
				builder.AddAttribute(203, "checked", selected);
				builder.AddAttribute(204, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, HandleSelectDataItemClick(item, selected)));

				builder.CloseElement(); // input

				builder.CloseElement(); // div
			}, "text-center");
		}

		protected override CellTemplate GetFooterCellTemplate()
		{
			return new CellTemplate(RenderFragmentBuilder.Empty());
		}

		protected override IEnumerable<SortingItem<TItemType>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItemType>>();
		}

		private Func<ChangeEventArgs, Task> HandleSelectDataItemClick(TItemType item, bool wasSelected)
		{
			return async (ChangeEventArgs changeEventArgs) =>
			{
				await (wasSelected ? UnselectDataItemClicked : SelectDataItemClicked).InvokeAsync(item);
			};
		}

		private async Task HandleSelectAllOrNoneClick(ChangeEventArgs args)
		{
			if (AllDataItemsSelected)
			{
				await SelectNoneClicked.InvokeAsync();
			}
			else
			{
				await SelectAllClicked.InvokeAsync();
			}
		}
	}
}
