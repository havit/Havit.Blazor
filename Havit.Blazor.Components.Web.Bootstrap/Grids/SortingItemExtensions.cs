using Havit.Collections;
using Havit.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	/// <summary>
	/// Sorting extension methods.
	/// </summary>
	public static class SortingItemExtensions
	{
		/// <summary>
		/// Applies itemsToMerge items to source items of sorting.
		/// When source starts with itemsToMerge, sort direction toggles.
		/// Otherwise itemsToMerges prepends source (and remove possible duplicities).
		/// </summary>
		public static IReadOnlyList<SortingItem<T>> ApplySorting<T>(this IReadOnlyList<SortingItem<T>> source, SortingItem<T>[] itemsToMerge)
		{
			if (!source.Any())
			{
				// nothing in source -> take itemsToMerge
				return itemsToMerge;
			}

			if (source.StartsWith(itemsToMerge))
			{
				// source starts with itemsToMerge, toggle sort direction
				return source.Take(itemsToMerge.Length).ToggleSortDirections().Concat(source.Skip(itemsToMerge.Length)).ToArray();
			}

			// add itemsToMerge to the source excluding itemsToMerge
			return itemsToMerge.Concat(source.Excluding(itemsToMerge)).ToArray();
		}

		/// <summary>
		/// Returns true when source sortings starts with itemsToMerge.
		/// </summary>
		internal static bool StartsWith<T>(this IReadOnlyList<SortingItem<T>> source, SortingItem<T>[] itemsToMerge)
		{
			if ((source.Count < itemsToMerge.Length) || (itemsToMerge.Length == 0))
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

		/// <summary>
		/// Returns source with toggles sort direction.
		/// </summary>
		private static IEnumerable<SortingItem<T>> ToggleSortDirections<T>(this IEnumerable<SortingItem<T>> source)
		{
			return source.Select(item => item.WithToggledSortDirection());
		}

		/// <summary>
		/// Returns source without itemsToMerge.
		/// </summary>
		private static IEnumerable<SortingItem<T>> Excluding<T>(this IEnumerable<SortingItem<T>> source, SortingItem<T>[] itemsToMerge)
		{
			return source.Where(sourceItem => !itemsToMerge.Any(itemToMerge => itemToMerge.EqualsIgnoringSortDirection(sourceItem)));
		}

		// TODO: Chceme takovou metodu? Na demo dobrá, ale jinak je spíš kontraproduktivní.
		/// <summary>
		/// Creates GenericPropertyComparer for the sorting by <see cref="SortingItem{TItemType}.SortString"/> and <see cref="SortingItem{TItemType}.SortDirection"/> properties.
		/// </summary>
		public static GenericPropertyComparer<T> ToGenericPropertyComparer<T>(this IEnumerable<SortingItem<T>> source)
		{
			Contract.Requires<ArgumentNullException>(source != null, nameof(source));

			return new GenericPropertyComparer<T>(source.ToSortItems());
		}

		// TODO: Chceme takovou metodu? Na demo dobrá, ale jinak je spíš kontraproduktivní.
		/// <summary>
		/// Converts <see cref="SortingItem{TItemType}"/> to <see cref="SortItem"/>.
		/// </summary>
		private static SortItem[] ToSortItems<T>(this IEnumerable<SortingItem<T>> source)
		{
			Contract.Requires<ArgumentNullException>(source != null, nameof(source));

			return source.Select(item => new SortItem(item.SortString, item.SortDirection)).ToArray();
		}
	}
}
