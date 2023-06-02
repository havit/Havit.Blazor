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
	/// Indicates whether the column is visible (otherwise the column is hidden).
	/// It is not suitable to conditionally display the column using @if statement in the markup code.
	/// </summary>
	bool IsVisible();

	/// <summary>
	/// Get column order (for scenarios where column order can be modified).
	/// Default should be <c>0</c>.
	/// When columns have same order they should render in the order of their registration (Which is usually the same as the column appearance in the source code.
	/// But it differs when the column is displayed conditionally using @if statement.).
	/// </summary>
	int GetOrder();

	/// <summary>
	/// Sorting of the column.
	/// </summary>
	SortingItem<TItem>[] GetSorting();

	/// <summary>
	/// Sorting of the column.
	/// </summary>
	int? GetDefaultSortingOrder();

	/// <summary>
	/// Returns header cell template.
	/// </summary>
	GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context);

	/// <summary>
	/// Returns data cell template for the specific item.
	/// </summary>
	GridCellTemplate GetItemCellTemplate(TItem item);

	/// <summary>
	/// Returns placeholder cell template.
	/// </summary>
	GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context);

	/// <summary>
	/// Returns footer cell template.
	/// </summary>
	GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context);
}
