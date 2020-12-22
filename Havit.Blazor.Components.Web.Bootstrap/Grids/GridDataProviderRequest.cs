using System.Collections.Generic;
using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider request for grid data.
	/// </summary>
	public class GridDataProviderRequest<TItemType>
	{
		/// <summary>
		/// The page index of the data segment requested.
		/// </summary>
		public int PageIndex { get; init; }

		/// <summary>
		/// The size of the page.
		/// </summary>
		public int PageSize { get; init; }

		/// <summary>
		/// Current sorting.
		/// </summary>
		public IReadOnlyList<SortingItem<TItemType>> Sorting { get; init; }

		/// <summary>
		/// The <see cref="System.Threading.CancellationToken"/> used to relay cancellation of the request.
		/// </summary>
		public CancellationToken CancellationToken { get; init; }
	}
}
