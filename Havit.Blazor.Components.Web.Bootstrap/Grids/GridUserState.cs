namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// User state of the <see cref="HxGrid"/>.
/// </summary>
public record class GridUserState
{
	/// <summary>
	/// Current page index.
	/// </summary>
	public int PageIndex { get; init; }

	/// <summary>
	/// Current sorting.
	/// </summary>
	public IReadOnlyList<GridUserStateSortingItem> Sorting { get; init; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <remarks>
	/// For backward compatibility in custom projects.
	/// Once upon a time this one was the only constructor.
	/// </remarks>
	public GridUserState(int pageIndex, IReadOnlyList<GridUserStateSortingItem> sorting)
	{
		PageIndex = pageIndex;
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
