using System;
using Microsoft.AspNetCore.Components;

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

		/// <summary>
		/// Indicates whether to render footer when data are empty.
		/// Default is <c>false</c>.
		/// </summary>
		public bool ShowFooterWhenEmptyData { get; set; } = false;

		/// <summary>
		/// Custom CSS class to render with <code>div</code> element wrapping the main <code>table</code>
		/// (<see cref="HxPager"/> is not wrapped in this <code>div</code> element).
		/// </summary>
		public string TableContainerCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with main <code>table</code> element.
		/// </summary>
		public string TableCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with header <code>tr</code> element.
		/// </summary>
		public string HeaderRowCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with data <code>tr</code> element.
		/// </summary>
		public string ItemRowCssClass { get; set; }

		/// <summary>
		/// Height of the item row.
		/// Used for infinite scroll.
		/// Default is <c>50px</c>.
		/// </summary>
		public float ItemRowHeight { get; set; } = 50;

		/// <summary>
		/// Custom CSS class to render with footer <code>tr</code> element.
		/// </summary>
		public string FooterRowCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with pager wrapping <code>div</code> element.
		/// </summary>
		public string PagerContainerCssClass { get; set; }
	}
}