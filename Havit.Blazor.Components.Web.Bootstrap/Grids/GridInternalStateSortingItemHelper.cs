namespace Havit.Blazor.Components.Web.Bootstrap;

internal static class GridInternalStateSortingItemHelper
{
	public static GridInternalStateSortingItem<TItem>[] ApplyColumnToSorting<TItem>(IList<GridInternalStateSortingItem<TItem>> currentSorting, IHxGridColumn<TItem> newSortColumn)
	{
		List<GridInternalStateSortingItem<TItem>> newSorting;

		if ((currentSorting == null) || !currentSorting.Any())
		{
			// No current sorting? Create a new one with the newSortColumn.
			newSorting = new List<GridInternalStateSortingItem<TItem>>
											{
												new GridInternalStateSortingItem<TItem>
												{
													Column = newSortColumn,
													ReverseDirection = false
												}
											};
		}
		else
		{
			if (currentSorting[0].Column == newSortColumn)
			{
				// Currently sorting by the newSortColumn?
				// Toggle the sort direction.
				newSorting = new List<GridInternalStateSortingItem<TItem>>(currentSorting);
				newSorting[0] = new GridInternalStateSortingItem<TItem>
				{
					Column = newSortColumn,
					ReverseDirection = !currentSorting[0].ReverseDirection
				};
			}
			else
			{
				// If there is a sorting with another "first" sort column, remove the newSortColumn from the following position.
				// Create a new sorting with the column "in the front".
				newSorting = new List<GridInternalStateSortingItem<TItem>>(currentSorting);
				newSorting.RemoveAll(item => item.Column == newSortColumn);
				newSorting.Insert(0, new GridInternalStateSortingItem<TItem>
				{
					Column = newSortColumn,
					ReverseDirection = false
				});
			}
		}

		return newSorting.ToArray();
	}

	public static IReadOnlyList<SortingItem<TItem>> ToSortingItems<TItem>(IEnumerable<GridInternalStateSortingItem<TItem>> sortingState)
	{
		List<SortingItem<TItem>> result = new List<SortingItem<TItem>>();

		if (sortingState != null)
		{
			foreach (GridInternalStateSortingItem<TItem> sortingStateItem in sortingState)
			{
				SortingItem<TItem>[] sorting = sortingStateItem.Column.GetSorting();

				foreach (var sortingItem in sorting)
				{
					if (result.Any(item => item.EqualsIgnoringSortDirection(sortingItem))) // leads to N^2 (on a couple of items)
					{
						continue;
					}

					result.Add(sortingStateItem.ReverseDirection ? sortingItem.WithToggledSortDirection() : sortingItem);
				}
			}
		}

		return result;
	}
}
