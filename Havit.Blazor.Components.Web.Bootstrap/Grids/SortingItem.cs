using Havit.Collections;
using System;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	// TODO: Přesunot do správného namespace
	public class SortingItem<TItemType>
	{
		public Expression<Func<TItemType, IComparable>> SortExpression { get; }
		public SortDirection SortDirection { get; }

		public SortingItem(Expression<Func<TItemType, IComparable>> sortExpression, SortDirection sortDirection)
		{
			SortExpression = sortExpression;
			SortDirection = sortDirection;
		}

		public bool EqualsIgnoringSortDirection<T>(SortingItem<T> sortingItem)
		{
			// pro účely řazení good-enough
			return this.SortExpression.ToString().Equals(sortingItem.SortExpression.ToString());
		}

		public SortingItem<TItemType> WithToggledSortDirection()
		{
			return new SortingItem<TItemType>(SortExpression, SortDirection.Toggle());
		}

		public override string ToString()
		{
			return $"SortExpression: {SortExpression}, SortDirection: {SortDirection}";
		}
	}
}