namespace Havit.Blazor.Components.Web.Bootstrap
{
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
		/// Icon to display ascending sort direction.
		/// </summary>
		public IconBase SortAscendingIcon { get; set; }

		/// <summary>
		/// Icon to display descending sort direction.
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
		/// Page size for <see cref="GridContentNavigationMode.Pagination"/>.
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
		/// Custom CSS class to render with pager.
		/// </summary>
		public string PagerCssClass { get; set; }

		/// <summary>
		/// Allow the table to be scrolled horizontally with ease accross any breakpoint.
		/// </summary>
		public bool? IsResponsive { get; set; }
	}
}