namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Placeholder cell context.
/// </summary>
public record GridPlaceholderCellContext
{
	/// <summary>
	/// Index of the row.		
	/// It reflects current page index when <see cref="GridContentNavigationMode.Pagination" />, <see cref="GridContentNavigationMode.LoadMore" /> or <see cref="GridContentNavigationMode.PaginationAndLoadMore" /> mode is used
	/// (ie. when the page size is 10, then on the third page indexes are 20-29).
	/// </summary>
	public int Index { get; init; }
}
