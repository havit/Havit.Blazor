namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents the result returned by a data provider for a grid component.
/// This class encapsulates both the collection of items to be displayed in the grid
/// and the total item count which is useful for implementing features like pagination
/// and infinite scrolling.
/// </summary>
/// <typeparam name="TItem">The type of the items in the grid.</typeparam>
public class GridDataProviderResult<TItem>
{
	/// <summary>
	/// The collection of <typeparamref name="TItem"/> items to be displayed in the grid.
	/// These items are the subset of data returned by the data provider based on the current
	/// grid state like page number, page size, and any applied filters.
	/// </summary>
	public IEnumerable<TItem> Data { get; init; }

	/// <summary>
	/// The total count of items in the data source. This count is used by the grid
	/// to calculate the total number of pages and manage the pagination or infinite scrolling.
	/// It represents the total number of items before any paging is applied.
	/// </summary>
	public int? TotalCount { get; init; }
}
