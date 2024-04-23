namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context for the <see cref="HxGrid{TItem}.PaginationTemplate" /> of the <see cref="HxGrid{TItem}"/>.
/// </summary>
public sealed record GridPaginationTemplateContext
{
	private readonly Func<int, Task> _changeCurrentPageIndexAsyncFunc;

	internal GridPaginationTemplateContext(Func<int, Task> changeCurrentPageIndexAsyncFunc)
	{
		_changeCurrentPageIndexAsyncFunc = changeCurrentPageIndexAsyncFunc;
	}

	/// <summary>
	/// Current grid user state (contains <see cref="GridUserState.PageIndex"/>).
	/// </summary>
	public GridUserState CurrentUserState { get; init; }

	/// <summary>
	/// Items per page.
	/// </summary>
	public int PageSize { get; init; }

	/// <summary>
	/// Total number of data items.
	/// </summary>
	public int TotalCount { get; init; }

	/// <summary>
	/// Settings for the pager (derived from <c>HxGrid.PagerSettings</c> .. <c>HxGrid.Settings.PagerSettings</c> .. <c>HxGrid.Defaults.PagerSettings</c> cascade).
	/// </summary>
	public PagerSettings PagerSettings { get; init; }

	/// <summary>
	/// Instructs the grid to change the current page index.
	/// </summary>
	public Task ChangeCurrentPageIndexAsync(int pageIndex) => _changeCurrentPageIndexAsyncFunc.Invoke(pageIndex);
}
