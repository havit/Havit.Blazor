using System.Linq.Expressions;
using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid column.
/// </summary>
/// <typeparam name="TItem">Grid row data type.</typeparam>
public class HxGridColumn<TItem> : HxGridColumnBase<TItem>
{
	/// <summary>
	/// Column unique identifier.
	/// </summary>
	[Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");

	/// <summary>
	/// Indicates whether the column is visible (otherwise the column is hidden).
	/// </summary>
	[Parameter] public bool Visible { get; set; } = true;

	/// <summary>
	/// The order (display index) of the column.
	/// Columns are displayed in the order of this property.
	/// Columns with the same value are displayed in the order of appearance in the code (when the columns are not conditionally displayed using @if).
	/// </summary>
	/// <exception cref="ArgumentException">Value is <c>Int32.MinValue</c> or <c>Int32.MaxValue</c>.</exception>
	[Parameter]
	public int Order
	{
		get => order;
		set
		{
			// This is to ensure MultiSelectGridColumn is displayed always as the first column.
			// MultiSelectGridColumn uses Int32.MinValue and we do not want to enable column to have same value.
			Contract.Requires<ArgumentException>(value != Int32.MinValue);

			order = value;
		}
	}
	private int order = 0;

	#region Header properties
	/// <summary>
	/// Header cell text.
	/// </summary>
	[Parameter] public string HeaderText { get; set; }

	/// <summary>
	/// Header cell template.
	/// </summary>
	[Parameter] public RenderFragment<GridHeaderCellContext> HeaderTemplate { get; set; }

	/// <summary>
	/// Header cell css class.
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }
	#endregion

	#region Item properties
	/// <summary>
	/// Returns text for the item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTextSelector { get; set; }

	/// <summary>
	/// Returns template for the item.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// Returns item css class (not dependent on data).
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }

	/// <summary>
	/// Returns item css class for the specific date item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemCssClassSelector { get; set; }
	#endregion

	/// <summary>
	/// Placeholder cell template.
	/// </summary>
	[Parameter] public RenderFragment<GridPlaceholderCellContext> PlaceholderTemplate { get; set; }

	#region Footer properties
	/// <summary>
	/// Footer cell text.
	/// </summary>
	[Parameter] public string FooterText { get; set; }

	/// <summary>
	/// Footer cell template.
	/// </summary>
	[Parameter] public RenderFragment<GridFooterCellContext> FooterTemplate { get; set; }

	/// <summary>
	/// Footer cell css class.
	/// </summary>
	[Parameter] public string FooterCssClass { get; set; }
	#endregion

	#region Sorting properties
	/// <summary>
	/// Returns column sorting as string.
	/// Use to set sorting as a string, ie. to get value to pass to backend.
	/// Ignored for client-side sorting.
	/// </summary>
	[Parameter] public string SortString { get; set; }

	/// <summary>
	/// Returns column sorting expression for automatic grid sorting.
	/// To be used for &quot;strongly typed&quot; setting of sorting, required for client-side sorting.
	/// Must be <see cref="IComparable"/>.
	/// Sorting of the column does not support multiple expressions. Create an artificial property and implement <see cref="IComparable"/>.
	/// </summary>
	[Parameter] public Expression<Func<TItem, IComparable>> SortKeySelector { get; set; }

	/// <summary>
	/// Initial sorting direction. Default is <see cref="SortDirection.Ascending" />.
	/// </summary>
	[Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

	/// <summary>
	/// Indicates the sorting on the column is default (primary) on the grid.
	/// Set <c>true</c> for the column which is to be used for default sorting.
	/// </summary>
	[Parameter] public bool IsDefaultSortColumn { get; set; } = false;
	#endregion

	/// <inheritdoc />
	protected override string GetId() => Id;

	/// <inheritdoc />
	protected override bool IsColumnVisible() => Visible;

	/// <inheritdoc />
	protected override int GetColumnOrder() => Order;

	/// <inheritdoc />
	protected override GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context) => GridCellTemplate.Create(RenderFragmentBuilder.CreateFrom(HeaderText, HeaderTemplate?.Invoke(context)), HeaderCssClass);

	/// <inheritdoc />
	protected override GridCellTemplate GetItemCellTemplate(TItem item)
	{
		string cssClass = CssClassHelper.Combine(ItemCssClass, ItemCssClassSelector?.Invoke(item));
		return GridCellTemplate.Create(RenderFragmentBuilder.CreateFrom(ItemTextSelector?.Invoke(item), ItemTemplate?.Invoke(item)), cssClass);
	}

	/// <inheritdoc />
	protected override GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context) => GridCellTemplate.Create((PlaceholderTemplate != null) ? PlaceholderTemplate(context) : GetDefaultItemPlaceholder(context));

	private RenderFragment GetDefaultItemPlaceholder(GridPlaceholderCellContext context)
	{
		return (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxPlaceholderContainer>(100);
			builder.AddAttribute(101, nameof(HxPlaceholderContainer.Animation), PlaceholderAnimation.Glow);
			builder.AddAttribute(102, nameof(HxPlaceholderContainer.ChildContent), (RenderFragment)((RenderTreeBuilder builder2) =>
			{
				builder2.OpenComponent<HxPlaceholder>(200);
				builder2.AddAttribute(201, nameof(HxPlaceholder.Columns), placeholderColumns[context.Index % placeholderColumns.Length]);
				builder2.CloseComponent(); // HxPlaceholder
			}));

			builder.CloseComponent(); // HxPlaceholderContainer
		};
	}
	private readonly string[] placeholderColumns = new[] { "6", "9", "4", "10", "5", "2", "7" };

	/// <inheritdoc />
	protected override GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context) => GridCellTemplate.Create(RenderFragmentBuilder.CreateFrom(FooterText, FooterTemplate?.Invoke(context)), FooterCssClass);

	/// <inheritdoc />
	protected override IEnumerable<SortingItem<TItem>> GetSorting()
	{
		if ((SortKeySelector == null) && String.IsNullOrEmpty(SortString))
		{
			yield break;
		}

		yield return new SortingItem<TItem>(this.SortString, this.SortKeySelector, this.SortDirection);
	}

	/// <inheritdoc />
	protected override int? GetDefaultSortingOrder() => IsDefaultSortColumn ? 0 : null;
}
