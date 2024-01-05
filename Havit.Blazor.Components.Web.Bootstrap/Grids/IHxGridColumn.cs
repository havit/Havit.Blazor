namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid column.
/// </summary>
public interface IHxGridColumn<TItem>
{
	/// <summary>
	/// Returns the unique column identifier.
	/// </summary>
	string GetId();

	/// <summary>
	/// Indicates whether the column is visible (otherwise, the column is hidden).
	/// It is not suitable to conditionally display the column using an @if statement in the markup code.
	/// </summary>
	bool IsVisible();

	/// <summary>
	/// Gets the column order (for scenarios where the column order can be modified).
	/// The default value should be <c>0</c>.
	/// When columns have the same order, they should render in the order of their registration (which is usually the same as the column appearance in the source code, but it differs when the column is displayed conditionally using an @if statement).
	/// </summary>
	int GetOrder();

	/// <summary>
	/// Gets the sorting of the column.
	/// </summary>
	SortingItem<TItem>[] GetSorting();

	/// <summary>
	/// Gets the default sorting order of the column.
	/// </summary>
	int? GetDefaultSortingOrder();

	/// <summary>
	/// Returns the header cell template.
	/// </summary>
	GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context);

	/// <summary>
	/// Returns the data cell template for the specific item.
	/// </summary>
	GridCellTemplate GetItemCellTemplate(TItem item);

	/// <summary>
	/// Returns the placeholder cell template.
	/// </summary>
	GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context);

	/// <summary>
	/// Returns the footer cell template.
	/// </summary>
	GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context);
}
