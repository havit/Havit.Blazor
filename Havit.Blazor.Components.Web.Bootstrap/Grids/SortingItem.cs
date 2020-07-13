using Havit.Collections;
using Havit.Diagnostics.Contracts;
using System;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	/// <summary>
	/// Item describes one sorting criteria.
	/// </summary>
	public sealed class SortingItem<TItemType>
	{
		/// <summary>
		/// Sorting as string value. Can be used to pass value between application layers (ie. WebAPI call parameter).
		/// </summary>
		public string SortString { get; }

		/// <summary>
		/// Sorting expression. To be used for automatic in memory sorting.
		/// </summary>
		public Expression<Func<TItemType, IComparable>> SortExpression { get; }

		/// <summary>
		/// Sort direction of SortString/SortExpression.
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
		public SortingItem(string sortString, Expression<Func<TItemType, IComparable>> sortExpression, SortDirection sortDirection, int? sortDefaultOrder)
		{
			Contract.Requires((sortString != null) || (sortExpression != null));

			SortString = sortString;
			SortExpression = sortExpression;
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
				&& (((this.SortExpression == null) && (sortingItem.SortExpression == null))
					|| this.SortExpression.ToString().Equals(sortingItem.SortExpression.ToString()) /* pro účely řazení good-enough */);
		}

		/// <summary>
		/// Returns the SortItem describing the same sorting with toggled direction.
		/// </summary>
		public SortingItem<TItemType> WithToggledSortDirection()
		{
			return new SortingItem<TItemType>(SortString, SortExpression, SortDirection.Reverse(), SortDefaultOrder);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"SortString: {SortString ?? "(null)"}, SortExpression: {SortExpression}, SortDirection: {SortDirection}";
		}
	}
}