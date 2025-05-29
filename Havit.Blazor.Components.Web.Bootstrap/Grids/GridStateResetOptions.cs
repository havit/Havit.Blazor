namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Specifies which aspects of the grid state should be reset.
/// </summary>
[Flags]
public enum GridStateResetOptions
{
	/// <summary>
	/// Do not reset grid state.
	/// </summary>
	None = 0,

	/// <summary>
	/// Reset the grid's position (reset <see cref="GridUserState.PageIndex" /> and <see cref="GridUserState.LoadMoreAdditionalItemsCount" /> for <see cref="GridContentNavigationMode.Pagination" />, <see cref="GridContentNavigationMode.PaginationAndLoadMore" /> and <see cref="GridContentNavigationMode.LoadMore" />, reset scroll position for <see cref="GridContentNavigationMode.InfiniteScroll" />).
	/// </summary>
	ResetPosition = 1 << 0,

	/// <summary>
	/// Reset the grid's sorting state to default (reset <see cref="GridUserState.Sorting" />).
	/// </summary>
	ResetSorting = 1 << 1,

	/// <summary>
	/// Reset all grid state aspects.
	/// </summary>
	All = 0xFFFF
}
