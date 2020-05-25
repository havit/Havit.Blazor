using Havit.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	// TODO: Přesunot do správného namespace
	public static class SortingItemExtensions
	{
		public static SortingItem<T>[] ApplySorting<T>(this SortingItem<T>[] source, SortingItem<T>[] itemsToMerge)
		{
			if (!source.Any())
			{
				// pokud ve zdroji nic není, vezmeme cílové položky
				return itemsToMerge;
			}

			if (source.StartsWith(itemsToMerge))
			{
				// pokud již podle položek řadíme, pak jen otočíme pořadí
				return source.Take(itemsToMerge.Length).ToggleSortDirections().Concat(source.Skip(itemsToMerge.Length)).ToArray();
			}

			// přidáme položky na začátek, odebereme shodné z pole, pokud existují
			return itemsToMerge.Concat(source.Excluding(itemsToMerge)).ToArray();
		}

		internal static bool StartsWith<T>(this SortingItem<T>[] source, SortingItem<T>[] itemsToMerge)
		{
			if ((source.Length < itemsToMerge.Length) || (itemsToMerge.Length == 0))
			{
				return false;
			}

			for (int i = 0; i < itemsToMerge.Length; i++)
			{
				if (!itemsToMerge[i].EqualsIgnoringSortDirection(source[i]))
				{
					return false;
				}
			}

			return true;
		}

		private static IEnumerable<SortingItem<T>> ToggleSortDirections<T>(this IEnumerable<SortingItem<T>> source)
		{
			return source.Select(item => item.WithToggledSortDirection());
		}

		private static IEnumerable<SortingItem<T>> Excluding<T>(this SortingItem<T>[] source, SortingItem<T>[] itemsToMerge)
		{
			return source.Where(sourceItem => !itemsToMerge.Any(itemToMerge => itemToMerge.EqualsIgnoringSortDirection(sourceItem)));
		}

		// TODO: Přesunout do Havit.Collections
		internal static SortDirection Toggle(this SortDirection sortDirection)
		{
			return (SortDirection)(1 - (int)sortDirection);
		}
	}
}
