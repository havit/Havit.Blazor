namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Strategy for displaying and loading data in the grid.
/// </summary>
public enum GridContentNavigationMode
{
	/// <summary>
	/// Use pagination.
	/// </summary>
	Pagination = 1,

	/// <summary>
	/// Use "Load more" button.
	/// </summary>
	LoadMore = 2,

	/// <summary>
	/// Use pagination and "Load more" button.
	/// </summary>
	/// <remarks>
	/// This value is used as a flagged enum (but it is not).
	/// </remarks>
	PaginationAndLoadMore = 3,

	/// <summary>
	/// Use infinite scroll (virtualized).
	/// </summary>
	InfiniteScroll = 4,
}
