namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public class HxMultiSelectGridColumnInternal<TItem> : HxGridColumnBase<TItem>
{
	[Parameter] public HashSet<TItem> SelectedDataItems { get; set; }
	[Parameter] public bool AllDataItemsSelected { get; set; }
	[Parameter] public EventCallback OnSelectAllClicked { get; set; }
	[Parameter] public EventCallback OnSelectNoneClicked { get; set; }
	[Parameter] public EventCallback<TItem> OnSelectDataItemClicked { get; set; }
	[Parameter] public EventCallback<TItem> OnUnselectDataItemClicked { get; set; }

	/// <inheritdoc />
	protected override int GetColumnOrder() => Int32.MinValue;

	protected override GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context)
	{
		return new GridCellTemplate
		{
			CssClass = "text-center",
			Template = (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(100, "input");
				builder.AddAttribute(101, "type", "checkbox");
				builder.AddAttribute(102, "class", "form-check-input");

				builder.AddAttribute(103, "checked", AllDataItemsSelected);
				builder.AddAttribute(104, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, HandleSelectAllOrNoneClick));
				builder.AddEventStopPropagationAttribute(105, "onclick", true);

				builder.CloseElement(); // input
			}
		};
	}

	protected override GridCellTemplate GetItemCellTemplate(TItem item)
	{
		return new GridCellTemplate
		{
			CssClass = "text-center",
			Template = (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(100, "input");
				builder.AddAttribute(101, "type", "checkbox");
				builder.AddAttribute(102, "class", "form-check-input");

				bool selected = SelectedDataItems?.Contains(item) ?? false;
				builder.AddAttribute(103, "checked", selected);
				builder.AddAttribute(104, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, HandleSelectDataItemClick(item, selected)));
				builder.AddEventStopPropagationAttribute(105, "onclick", true);

				builder.CloseElement(); // input
			}
		};
	}

	protected override GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context)
	{
		return GridCellTemplate.Empty;
	}

	protected override GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context)
	{
		return GridCellTemplate.Empty;
	}

	protected override IEnumerable<SortingItem<TItem>> GetSorting()
	{
		return Enumerable.Empty<SortingItem<TItem>>();
	}

	private Func<ChangeEventArgs, Task> HandleSelectDataItemClick(TItem item, bool wasSelected)
	{
		return async (ChangeEventArgs changeEventArgs) =>
		{
			await (wasSelected ? OnUnselectDataItemClicked : OnSelectDataItemClicked).InvokeAsync(item);
		};
	}

	private async Task HandleSelectAllOrNoneClick(ChangeEventArgs args)
	{
		if (AllDataItemsSelected)
		{
			await OnSelectNoneClicked.InvokeAsync();
		}
		else
		{
			await OnSelectAllClicked.InvokeAsync();
		}
	}
}
