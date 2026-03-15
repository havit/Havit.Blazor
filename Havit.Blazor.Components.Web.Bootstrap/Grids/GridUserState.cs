namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// User state of the <see cref="HxGrid"/>.
/// </summary>
public record class GridUserState
{
	/// <summary>
	/// Current page index for <see cref="GridContentNavigationMode.Pagination"/>.
	/// Starting page index for <see cref="GridContentNavigationMode.LoadMore"/> and <see cref="GridContentNavigationMode.PaginationAndLoadMore"/>.
	/// </summary>
	/// <remarks>
	/// This number is not the current page index in the pager when <see cref="LoadMoreAdditionalItemsCount"/> is not zero.
	/// </remarks>
	public int PageIndex { get; init; }

	/// <summary>
	/// Count of additional items to load for <see cref="GridContentNavigationMode.LoadMore"/> or <see cref="GridContentNavigationMode.PaginationAndLoadMore"/>.
	/// </summary>
	public int LoadMoreAdditionalItemsCount { get; init; }

	/// <summary>
	/// Current sorting.
	/// </summary>
	public IReadOnlyList<GridUserStateSortingItem> Sorting { get; init; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <remarks>
	/// For backward compatibility in custom projects.
	/// Once upon a time, this was the only constructor.
	/// </remarks>
	public GridUserState(int pageIndex, IReadOnlyList<GridUserStateSortingItem> sorting)
	{
		PageIndex = pageIndex;
		LoadMoreAdditionalItemsCount = 0;
		Sorting = sorting;
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	public GridUserState() : this(0, null)
	{
		// NOOP
	}
}
