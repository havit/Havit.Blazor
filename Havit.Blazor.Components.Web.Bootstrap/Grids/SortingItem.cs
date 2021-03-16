using Havit.Collections;
using Havit.Diagnostics.Contracts;
using System;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item describes one sorting criteria.
	/// </summary>
	public sealed class SortingItem<TItem>
	{
		/// <summary>
		/// Sorting as string value. Can be used to pass value between application layers (ie. WebAPI call parameter).
		/// </summary>
		public string SortString { get; }

		/// <summary>
		/// Selector function of sorting key. To be used for automatic in-memory sorting.
		/// </summary>
		public Expression<Func<TItem, IComparable>> SortKeySelector { get; }

		/// <summary>
		/// Sort direction of SortString/SortKeySelector.
		/// </summary>
		public SortDirection SortDirection { get; }

		/// <summary>
		/// Not-null for default sort item.
		/// For multiple sort items, set value in ascendant order.
		/// </summary>
		/// <remarks>
		/// Current implementation of grid columns uses only null and zero values.
		/// </remarks>
		/// <example>
		/// To set default sorting by LastName, then FirstName use value 1 for LastName and value 2 for FirstName).
		/// </example>
		public int? SortDefaultOrder { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public SortingItem(string sortString, Expression<Func<TItem, IComparable>> sortKeySelector, SortDirection sortDirection, int? sortDefaultOrder)
		{
			Contract.Requires((sortString != null) || (sortKeySelector != null));

			SortString = sortString;
			SortKeySelector = sortKeySelector;
			SortDirection = sortDirection;
			SortDefaultOrder = sortDefaultOrder;
		}

		/// <summary>
		/// Returns true when this and sorting item describes the same sorting (direction is ignored).
		/// </summary>
		public bool EqualsIgnoringSortDirection<T>(SortingItem<T> sortingItem)
		{
			return (sortingItem != null)
				&& String.Equals(this.SortString, sortingItem.SortString, StringComparison.OrdinalIgnoreCase)
				&& (((this.SortKeySelector == null) && (sortingItem.SortKeySelector == null))
					|| this.SortKeySelector.ToString().Equals(sortingItem.SortKeySelector.ToString()) /* pro účely řazení good-enough */);
		}

		/// <summary>
		/// Returns the SortItem describing the same sorting with toggled direction.
		/// </summary>
		public SortingItem<TItem> WithToggledSortDirection()
		{
			return new SortingItem<TItem>(SortString, SortKeySelector, SortDirection.Reverse(), SortDefaultOrder);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"SortString: {SortString ?? "(null)"}, SortKeySelector: {SortKeySelector}, SortDirection: {SortDirection}";
		}
	}
}