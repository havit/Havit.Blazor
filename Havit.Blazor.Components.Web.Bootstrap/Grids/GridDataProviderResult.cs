using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider result for grid data.
	/// </summary>
	public class GridDataProviderResult<TItem>
	{
		/// <summary>
		/// The provided items by the request.
		/// </summary>
		public IEnumerable<TItem> Data { get; init; }

		/// <summary>
		/// The total item count in the source (for pagination and infinite scroll).
		/// </summary>
		public int? TotalCount { get; init; }
	}
}
