namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxGrid{TItem}"/> and derived components.
/// </summary>
public record GridSettings
{
	/// <summary>
	/// Strategy how data are displayed in the grid (and loaded to the grid).
	/// </summary>
	public GridContentNavigationMode? ContentNavigationMode { get; set; }

	/// <summary>
	/// Icon to indicate ascending sort direction in column header.
	/// </summary>
	public IconBase SortAscendingIcon { get; set; }

	/// <summary>
	/// Icon to indicate descending sort direction in column header.
	/// </summary>
	public IconBase SortDescendingIcon { get; set; }

	/// <summary>
	/// Height of the item row used for infinite scroll calculations (<see cref="GridContentNavigationMode.InfiniteScroll"/>).
	/// </summary>
	public float? ItemRowHeight { get; set; }

	/// <summary>
	/// Infinite scroll (<see cref="GridContentNavigationMode.InfiniteScroll"/>):
	/// Gets or sets a value that determines how many additional items will be rendered
	/// before and after the visible region. This help to reduce the frequency of rendering
	/// during scrolling. However, higher values mean that more elements will be present
	/// in the page.
	/// </summary>
	public int? OverscanCount { get; set; }

	/// <summary>
	/// Page size for <see cref="GridContentNavigationMode.Pagination"/>, <see cref="GridContentNavigationMode.LoadMore"/> and <see cref="GridContentNavigationMode.PaginationAndLoadMore" />. Set <c>0</c> to disable paging.
	/// </summary>
	public int? PageSize { get; set; }

	/// <summary>
	/// Number of rows with placeholders to render.
	/// </summary>
	public int? PlaceholdersRowCount { get; set; }

	/// <summary>
	/// Indicates whether to render footer when data are empty.
	/// </summary>
	public bool? ShowFooterWhenEmptyData { get; set; }

	/// <summary>
	/// Custom CSS class to render with <c>div</c> element wrapping the main <c>table</c>
	/// (<see cref="HxPager"/> is not wrapped in this <c>div</c> element).
	/// </summary>
	public string TableContainerCssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with main <c>table</c> element.
	/// </summary>
	public string TableCssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with header <c>tr</c> element.
	/// </summary>
	public string HeaderRowCssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with data <c>tr</c> element.
	/// </summary>
	public string ItemRowCssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with footer <c>tr</c> element.
	/// </summary>
	public string FooterRowCssClass { get; set; }

	/// <summary>
	/// Allows the table to be scrolled horizontally with ease across any breakpoint (adds the <c>table-responsive</c> class to the table).
	/// </summary>
	public bool? Responsive { get; set; }

	/// <summary>
	/// Enables hover state on table rows within a <c>&lt;tbody&gt;</c> (sets the <c>table-hover</c> class on the table).
	/// </summary>
	public bool? Hover { get; set; }

	/// <summary>
	/// Adds zebra-striping to any table row within the <c>&lt;tbody&gt;</c> (alternating rows).
	/// </summary>
	public bool? Striped { get; set; }

	/// <summary>
	/// Pager settings.
	/// </summary>
	public PagerSettings PagerSettings { get; set; }

	/// <summary>
	/// Settings for the "Load more" navigation button (<see cref="GridContentNavigationMode.LoadMore"/> or <see cref="GridContentNavigationMode.PaginationAndLoadMore"/>).
	/// </summary>
	public ButtonSettings LoadMoreButtonSettings { get; set; }
}