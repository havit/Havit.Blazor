namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// User state of the <see cref="HxGrid"/>.
/// </summary>
public record class GridUserState : IEquatable<GridUserState>
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

	public GridUserState() : this(0, null)
	{
		// NOOP
	}

	// Default Equals implementation is not sufficient because of the IReadOnlyList property.
	public virtual bool Equals(GridUserState other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return (PageIndex == other.PageIndex)
			&& (LoadMoreAdditionalItemsCount == other.LoadMoreAdditionalItemsCount)
			&& SortingEquals(Sorting, other.Sorting);
	}

	private static bool SortingEquals(IReadOnlyList<GridUserStateSortingItem> left, IReadOnlyList<GridUserStateSortingItem> right)
	{
		if (ReferenceEquals(left, right))
		{
			return true;
		}

		if ((left is null) || (right is null))
		{
			return false;
		}

		return left.SequenceEqual(right);
	}

	public override int GetHashCode()
	{
		var hash = new HashCode();
		hash.Add(PageIndex);
		hash.Add(LoadMoreAdditionalItemsCount);
		foreach (GridUserStateSortingItem item in Sorting)
		{
			hash.Add(item);
		}
		return hash.ToHashCode();
	}
}
