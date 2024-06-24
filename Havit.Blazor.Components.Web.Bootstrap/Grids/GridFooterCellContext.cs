namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid footer cell context.
/// </summary>
public record GridFooterCellContext
{
	/// <summary>
	/// The total count of items in the data source. This count is used by the grid
	/// to calculate the total number of pages and manage the pagination or infinite scrolling.
	/// It represents the total number of items before any paging is applied.
	/// </summary>
	public int? TotalCount { get; init; }
}