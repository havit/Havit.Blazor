using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider request for grid data.
/// </summary>
public class GridDataProviderRequest<TItem>
{
	/// <summary>
	/// The number of records to skip. In paging mode, it equals the page size multiplied by the requested page index.
	/// </summary>
	public int StartIndex { get; init; }

	/// <summary>
	/// The number of records to return. In paging mode, it equals the size of the page.				
	/// </summary>
	public int? Count { get; init; }

	/// <summary>
	/// Current sorting.
	/// </summary>
	public IReadOnlyList<SortingItem<TItem>> Sorting { get; init; }

	/// <summary>
	/// The <see cref="System.Threading.CancellationToken"/> used to relay cancellation of the request.
	/// </summary>
	public CancellationToken CancellationToken { get; init; }

	/// <summary>
	/// Processes data on the client side (sorting &amp; paging) and returns the result for the grid.
	/// </summary>
	/// <param name="data">The data to process (paging and sorting will be applied).</param>
	public GridDataProviderResult<TItem> ApplyTo(IEnumerable<TItem> data)
	{
		CancellationToken.ThrowIfCancellationRequested();

		if (data == null)
		{
			return new GridDataProviderResult<TItem>
			{
				Data = null,
				TotalCount = null
			};
		}

		IEnumerable<TItem> resultData = data;

		// PERF NOTE: We do not use .ApplyGridDataProviderRequest() here because we want to apply Compile() here?

		// Sorting
		if ((Sorting != null) && Sorting.Any())
		{
			Contract.Assert(Sorting.All(item => item.SortKeySelector != null), "All sorting items must have the SortKeySelector property set.");

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

		// Paging / Infinite scroll
		if (StartIndex > 0)
		{
			resultData = resultData.Skip(StartIndex);
		}
		if (Count > 0)
		{
			resultData = resultData.Take(Count.Value);
		}

		return new GridDataProviderResult<TItem>
		{
			Data = resultData.ToList(),
			TotalCount = data.Count()
		};
	}
}
