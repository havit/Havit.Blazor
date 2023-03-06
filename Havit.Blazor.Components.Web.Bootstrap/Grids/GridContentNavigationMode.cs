namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Strategy how data are displayed in the grid (and loaded to the grid).
/// </summary>
public enum GridContentNavigationMode
{
	/// <summary>
	/// Use pager.
	/// </summary>
	Pagination = 1,

	/// <summary>
	/// Use "Load more" button.
	/// </summary>
	LoadMore = 2,

	/// <summary>
	/// Use pager and "Load more" button.
	/// </summary>
	/// <remarks>
	/// Value used as this is a flagged enum (but it is not).
	/// </remarks>
	PaginationAndLoadMore = 3,

	/// <summary>
	/// Use infinite scroll (virtualized).
	/// </summary>
	InfiniteScroll = 4,
}
