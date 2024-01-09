using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap;

public static class GridDataProviderRequestExtensions
{
	/// <summary>
	/// Applies sorting &amp; paging from <see cref="HxGrid{TItem}.DataProvider"/>.
	/// </summary>
	/// <param name="source">Data source to apply the request to.</param>
	/// <param name="gridDataProviderRequest"><see cref="HxGrid{TItem}.DataProvider"/> request.</param>
	public static IQueryable<TItem> ApplyGridDataProviderRequest<TItem>(this IQueryable<TItem> source, GridDataProviderRequest<TItem> gridDataProviderRequest)
	{
		gridDataProviderRequest.CancellationToken.ThrowIfCancellationRequested();

		// Sorting
		if ((gridDataProviderRequest.Sorting != null) && gridDataProviderRequest.Sorting.Any())
		{
			Contract.Assert(gridDataProviderRequest.Sorting.All(item => item.SortKeySelector != null), $"All sorting items must have the {nameof(SortingItem<TItem>.SortKeySelector)} property set.");

			IOrderedQueryable<TItem> orderedDataProvider = (gridDataProviderRequest.Sorting[0].SortDirection == SortDirection.Ascending)
				? source.OrderBy(gridDataProviderRequest.Sorting[0].SortKeySelector)
				: source.OrderByDescending(gridDataProviderRequest.Sorting[0].SortKeySelector);

			for (int i = 1; i < gridDataProviderRequest.Sorting.Count; i++)
			{
				orderedDataProvider = (gridDataProviderRequest.Sorting[i].SortDirection == SortDirection.Ascending)
					? orderedDataProvider.ThenBy(gridDataProviderRequest.Sorting[i].SortKeySelector)
					: orderedDataProvider.ThenByDescending(gridDataProviderRequest.Sorting[i].SortKeySelector);
			}
			source = orderedDataProvider;
		}

		// Paging / Infinite scroll
		if (gridDataProviderRequest.StartIndex > 0)
		{
			source = source.Skip(gridDataProviderRequest.StartIndex);
		}
		if (gridDataProviderRequest.Count > 0)
		{
			source = source.Take(gridDataProviderRequest.Count.Value);
		}

		return source;
	}
}
