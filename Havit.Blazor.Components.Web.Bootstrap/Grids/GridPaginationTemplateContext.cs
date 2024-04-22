namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Pager.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/GridPaginationTemplateContext">https://havit.blazor.eu/components/GridPaginationTemplateContext</see>
/// </summary>
public class GridPaginationTemplateContext
{
	public GridPaginationTemplateContext(int totalItems, int totalPages, int currentPageIndex, int itemsPerPage, int firstItemIndex, int lastItemIndex)
	{
		TotalItems = totalItems;
		TotalPages = totalPages;
		CurrentPageIndex = currentPageIndex;
		ItemsPerPage = itemsPerPage;
		FirstItemIndex = firstItemIndex;
		LastItemIndex = lastItemIndex;
	}

	/// <summary>
	/// Total number of data items.
	/// </summary>
	public int TotalItems { get; set; }

	/// <summary>
	/// Total number of pages of data items.
	/// </summary>
	public int TotalPages { get; set; }

	/// <summary>
	/// Current page index.
	/// </summary>
	public int CurrentPageIndex { get; set; }

	/// <summary>
	/// Items per page.
	/// </summary>
	public int ItemsPerPage { get; set; }

	/// <summary>
	/// Index of first item (relative to total items).
	/// </summary>
	public int FirstItemIndex { get; set; }

	/// <summary>
	/// Index of last item (relative to total items).
	/// </summary>
	public int LastItemIndex { get; set; }

	/// <summary>
	/// Method called to change the current page index.
	/// </summary>
	public virtual Task ChangeCurrentPageIndexAsync(int pageIndex) => Task.CompletedTask;

	/// <summary>
	/// Method called to change the current page size.
	/// </summary>
	public virtual Task ChangeCurrentPageSizeAsync(int pageSize) => Task.CompletedTask;

	/// <summary>
	/// Event raised when the page index is changed.
	/// </summary>
	public EventCallback<int> CurrentPageIndexChanged { get; set; }

	/// <summary>
	/// Triggers the <see cref="CurrentPageIndexChanged"/> event. Allows interception of the event.
	/// </summary>
	protected virtual Task InvokeCurrentPageIndexChangedAsync(int newPageIndex) => CurrentPageIndexChanged.InvokeAsync(newPageIndex);

	/// <summary>
	/// Event raised when the page size is changed.
	/// </summary>
	public EventCallback<int> CurrentPageSizeChanged { get; set; }

	/// <summary>
	/// Triggers the <see cref="CurrentPageSizeChanged"/> event. Allows interception of the event.
	/// </summary>
	protected virtual Task InvokeCurrentPageSizeChangedAsync(int newPageSize) => CurrentPageSizeChanged.InvokeAsync(newPageSize);
}
