namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Pager.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPaginationContext">https://havit.blazor.eu/components/HxPaginationContext</see>
/// </summary>
public class HxPaginationContext
{
	public HxPaginationContext(int totalPages, int currentPageIndex, int itemsPerPage)
	{
		TotalPages = totalPages;
		CurrentPageIndex = currentPageIndex;
		ItemsPerPage = itemsPerPage;
	}

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
}
