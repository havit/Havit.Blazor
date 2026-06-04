namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Column for displaying the <see cref="HxContextMenu"/> in the <see cref="HxGrid"/>.
/// </summary>
public class HxContextMenuGridColumn<TItem> : HxGridColumnBase<TItem>
{
#pragma warning disable BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxContextMenuGridColumn<TItem>.Order' should be auto property
	/// <summary>
	/// The order (display index) of the column.
	/// Columns are displayed in the order of this property.
	/// Columns with the same value are displayed in the order of appearance in the code (when the columns are not conditionally displayed using @if).
	/// </summary>
	/// <exception cref="ArgumentException">Value is <c>Int32.MinValue</c> or <c>Int32.MaxValue</c>.</exception>
	[Parameter]
	public int Order
	{
		get => _order;
		set
		{
			// TODO Move validation to OnParametersSet
			// This is to ensure MultiSelectGridColumn is always displayed as the first column.
			// MultiSelectGridColumn uses Int32.MinValue and we do not want to enable columns to have the same value.
			Contract.Requires<ArgumentException>(value != Int32.MinValue);

			_order = value;
		}
	}
	private int _order = 0;
#pragma warning restore BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxContextMenuGridColumn<TItem>.Order' should be auto property

	/// <summary>
	/// Returns the item CSS class (not dependent on data).
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }

	/// <summary>
	/// Returns the item CSS class for the specific data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemCssClassSelector { get; set; }

	/// <summary>
	/// Context menu template.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ChildContent { get; set; }

	/// <inheritdoc />
	protected override string GetId() => nameof(HxContextMenuGridColumn<object>);

	/// <inheritdoc />
	protected override int GetColumnOrder() => _order;

	/// <inheritdoc />
	protected override GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context) => GridCellTemplate.Empty;

	/// <inheritdoc />
	protected override GridCellTemplate GetItemCellTemplate(TItem item)
	{
		string cssClass = CssClassHelper.Combine("hx-context-menu-grid-column", ItemCssClass, ItemCssClassSelector?.Invoke(item));
		return GridCellTemplate.Create(ChildContent(item), cssClass);
	}

	/// <inheritdoc />
	protected override GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context) => GridCellTemplate.Empty;

	/// <inheritdoc />
	protected override GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context) => GridCellTemplate.Empty;

	/// <inheritdoc />
	protected override IEnumerable<SortingItem<TItem>> GetSorting() => Enumerable.Empty<SortingItem<TItem>>();

	/// <inheritdoc />
	protected override int? GetDefaultSortingOrder() => null;

}
