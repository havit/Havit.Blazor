namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxGrid{TItem}"/>.
	/// </summary>
	public class GridDefaults
	{
		/// <summary>
		/// Strategy how data are displayed in the grid (and loaded to the grid).
		/// </summary>
		public GridContentNavigationMode ContentNavigationMode { get; set; } = GridContentNavigationMode.Pagination;

		/// <summary>
		/// Icon to display ascending sort direction.
		/// </summary>
		public IconBase SortAscendingIcon { get; set; } = BootstrapIcon.SortAlphaDown;

		/// <summary>
		/// Icon to display descending sort direction.
		/// </summary>
		public IconBase SortDescendingIcon { get; set; } = BootstrapIcon.SortAlphaDownAlt;

		/// <summary>
		/// Infinite scroll:
		/// Gets or sets a value that determines how many additional items will be rendered
		/// before and after the visible region. This help to reduce the frequency of rendering
		/// during scrolling. However, higher values mean that more elements will be present
		/// in the page.
		/// </summary>
		public int OverscanCount { get; set; } = 50;

		/// <summary>
		/// Page size.
		/// </summary>
		public int? PageSize { get; set; } = null;
	}
}