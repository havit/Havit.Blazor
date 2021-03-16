using Havit.Collections;
using Havit.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider request for grid data.
	/// </summary>
	public class GridDataProviderRequest<TItem>
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
		public IReadOnlyList<SortingItem<TItem>> Sorting { get; init; }

		/// <summary>
		/// The <see cref="System.Threading.CancellationToken"/> used to relay cancellation of the request.
		/// </summary>
		public CancellationToken CancellationToken { get; init; }

		/// <summary>
		/// Process data on client side (process sorting &amp; paging) and returns result for the grid.
		/// </summary>
		/// <param name="data">data to process (paging and sorting will be applied)</param>
		public GridDataProviderResult<TItem> ApplyTo(IEnumerable<TItem> data)
		{
			if (data == null)
			{
				return new GridDataProviderResult<TItem>
				{
					Data = null,
					TotalCount = null
				};
			}

			IEnumerable<TItem> resultData = data;

			#region Sorting
			if ((Sorting != null) && Sorting.Any())
			{
				Contract.Assert(Sorting.All(item => item.SortKeySelector != null), "All sorting items must have set SortKeySelector property.");

				IOrderedEnumerable<TItem> orderedData = (Sorting[0].SortDirection == SortDirection.Ascending)
					? resultData.OrderBy(Sorting[0].SortKeySelector.Compile())
					: resultData.OrderByDescending(Sorting[0].SortKeySelector.Compile());

				for (int i = 1; i < Sorting.Count; i++)
				{
					orderedData = (Sorting[i].SortDirection == SortDirection.Ascending)
						? orderedData.ThenBy(Sorting[i].SortKeySelector.Compile())
						: orderedData.ThenByDescending(Sorting[i].SortKeySelector.Compile());
				}
				resultData = orderedData;
			}
			#endregion

			#region Paging
			if (PageSize > 0)
			{
				resultData = resultData.Skip(PageSize * PageIndex).Take(PageSize);
			}
			#endregion

			return new GridDataProviderResult<TItem>
			{
				Data = resultData.ToList(),
				TotalCount = data.Count()
			};
		}
	}
}
