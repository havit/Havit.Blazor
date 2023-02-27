namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Strategy how data are displayed in the grid (and loaded to the grid).
/// </summary>
public enum GridContentNavigationMode
{
	/// <summary>
	/// Use pager.
	/// </summary>
	Pagination = 0,

	/// <summary>
	/// Use infinite scroll (virtualized).
	/// </summary>
	InfiniteScroll = 1,

	/// <summary>
	/// Use "Load more" button.
	/// </summary>
	LoadMore = 2
}
