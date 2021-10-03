using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Strategy how data are displayed in the grid (and loaded to the grid).
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
