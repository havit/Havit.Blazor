using Havit.Collections;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Sorting criteria for a <see cref="GridUserState"/>.
/// </summary>
public sealed class GridUserStateSortingItem
{
	/// <summary>
	/// Sorting as string value. Can be used to pass value between application layers (ie. WebAPI call parameter).
	/// </summary>
	public string SortString { get; init; }

	/// <summary>
	/// Selector function of sorting key. To be used for automatic in-memory sorting.
	/// </summary>
	public string SortKeySelector { get; init; }

	/// <summary>
	/// Sort direction of SortString/SortKeySelector.
	/// </summary>
	public SortDirection SortDirection { get; init; }

	/// <summary>
	/// Creates GridSortingStateItem for the specified parameters.
	/// <see cref="SortKeySelector"/> is converted from <c>Expression</c> to <c>string</c>.
	/// </summary>
	public static GridUserStateSortingItem Create<TItem>(string sortString = null, Expression<Func<TItem, IComparable>> sortKeySelector = null, SortDirection sortDirection = SortDirection.Ascending)
	{
		return new GridUserStateSortingItem
		{
			SortString = sortString,
			SortKeySelector = CreateSortKeySelector<TItem>(sortKeySelector),
			SortDirection = sortDirection
		};
	}

	/// <summary>
	/// Converts sort key selector as an expression type to a serializable type (<c>string</c>).
	/// </summary>
	/// <returns></returns>
	internal static string CreateSortKeySelector<TItem>(Expression<Func<TItem, IComparable>> expression)
	{
		return (expression == null)
			? null
			: Havit.Linq.Expressions.ExpressionExt.ReplaceParameter(expression, expression.Parameters.Single(), Expression.Parameter(typeof(TItem), "item")).ToString();
	}
}