using System.Linq.Expressions;
using Havit.Collections;
using Havit.Linq.Expressions;

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
		// The expressions used in the sorting are of type IComparable, but we need to convert them to object to be able to use them
		// in the Entity Framework Core OrderBy/ThenBy methods.
		// Inspired by: https://github.com/dotnet/efcore/issues/30228
		static Expression<Func<TItem, object>> ReplaceConvertType(Expression<Func<TItem, IComparable>> expression) =>
			Expression.Lambda<Func<TItem, object>>(
				Expression.Convert(
					expression.Body.RemoveConvert(),
					typeof(object)),
				expression.Parameters[0]);

		gridDataProviderRequest.CancellationToken.ThrowIfCancellationRequested();

		// Sorting
		if ((gridDataProviderRequest.Sorting != null) && gridDataProviderRequest.Sorting.Any())
		{
			Contract.Assert(gridDataProviderRequest.Sorting.All(item => item.SortKeySelector != null), $"All sorting items must have the {nameof(SortingItem<TItem>.SortKeySelector)} property set.");

			IOrderedQueryable<TItem> orderedDataProvider = (gridDataProviderRequest.Sorting[0].SortDirection == SortDirection.Ascending)
				? source.OrderBy(ReplaceConvertType(gridDataProviderRequest.Sorting[0].SortKeySelector))
				: source.OrderByDescending(ReplaceConvertType(gridDataProviderRequest.Sorting[0].SortKeySelector));

			for (int i = 1; i < gridDataProviderRequest.Sorting.Count; i++)
			{
				orderedDataProvider = (gridDataProviderRequest.Sorting[i].SortDirection == SortDirection.Ascending)
					? orderedDataProvider.ThenBy(ReplaceConvertType(gridDataProviderRequest.Sorting[i].SortKeySelector))
					: orderedDataProvider.ThenByDescending(ReplaceConvertType(gridDataProviderRequest.Sorting[i].SortKeySelector));
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
