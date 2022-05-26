namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Strategy how data are displayed in the grid (and loaded to the grid).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/GridContentNavigationMode">https://havit.blazor.eu/types/GridContentNavigationMode</see>
	/// </summary>
	public enum GridContentNavigationMode
	{
		/// <summary>
		/// Use pager.
		/// </summary>
		Pagination = 0,

		/// <summary>
		/// Use infinite scroll (virtualized).
		/// </summary>
		InfiniteScroll

		///// <summary>
		///// Use "Load more"
		///// </summary>
		// LoadMore
	}
}
