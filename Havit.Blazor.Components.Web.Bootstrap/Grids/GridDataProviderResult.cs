using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider result for grid data.
	/// </summary>
	public class GridDataProviderResult<TItemType>
	{
		/// <summary>
		/// The provided items by the request.
		/// </summary>
		public IEnumerable<TItemType> Data { get; init; }

		/// <summary>
		/// The total item count in the source (for paging).
		/// </summary>
		public int? TotalCount { get; init; }
	}
}
