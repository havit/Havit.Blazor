using Havit.Collections;
using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents one sorting criteria.
/// </summary>
public sealed record SortingItem<TItem>
{
	/// <summary>
	/// Gets or sets the sorting value as a string. Can be used to pass value between application layers (e.g., WebAPI call parameter).
	/// </summary>
	public string SortString { get; }

	/// <summary>
	/// Gets or sets the selector function for the sorting key. Used for automatic in-memory sorting.
	/// </summary>
	public Expression<Func<TItem, IComparable>> SortKeySelector { get; }

	/// <summary>
	/// Gets or sets the sort direction of SortString/SortKeySelector.
	/// </summary>
	public SortDirection SortDirection { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SortingItem{TItem}"/> class.
	/// </summary>
	public SortingItem(string sortString, Expression<Func<TItem, IComparable>> sortKeySelector, SortDirection sortDirection)
	{
		Contract.Requires((sortString != null) || (sortKeySelector != null));

		SortString = sortString;
		SortKeySelector = sortKeySelector;
		SortDirection = sortDirection;
	}

	/// <summary>
	/// Returns true when this and the sorting item describe the same sorting (direction is ignored).
	/// </summary>
	public bool EqualsIgnoringSortDirection<T>(SortingItem<T> sortingItem)
	{
		return (sortingItem != null)
			&& String.Equals(SortString, sortingItem.SortString, StringComparison.OrdinalIgnoreCase)
			&& (((SortKeySelector == null) && (sortingItem.SortKeySelector == null))
				|| SortKeySelector.ToString().Equals(sortingItem.SortKeySelector.ToString()) /* good-enough for sorting */);
	}

	/// <summary>
	/// Returns the SortingItem describing the same sorting with toggled direction.
	/// </summary>
	public SortingItem<TItem> WithToggledSortDirection()
	{
		return new SortingItem<TItem>(SortString, SortKeySelector, SortDirection.Reverse());
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"SortString: {SortString ?? "(null)"}, SortKeySelector: {SortKeySelector}, SortDirection: {SortDirection}";
	}
}
