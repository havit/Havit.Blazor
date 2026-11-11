namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Compares grid items by the key returned by <see cref="HxGrid{TItem}.ItemKeySelector"/>.
/// When the default key selector (the item itself) is in use, <see cref="EqualityComparer{T}.Default"/> is used,
/// so the typed equality of the items (<see cref="IEquatable{T}"/>, records) is preserved.
/// </summary>
internal sealed class GridItemKeyEqualityComparer<TItem> : IEqualityComparer<TItem>
{
	private readonly Func<Func<TItem, object>> _itemKeySelectorAccessor;
	private readonly Func<TItem, object> _defaultItemKeySelector;

	/// <param name="itemKeySelectorAccessor">Returns the current key selector (evaluated on each comparison, the selector is a component parameter and can change).</param>
	/// <param name="defaultItemKeySelector">The default key selector (the item itself); when in use, the typed default equality is used.</param>
	public GridItemKeyEqualityComparer(Func<Func<TItem, object>> itemKeySelectorAccessor, Func<TItem, object> defaultItemKeySelector)
	{
		_itemKeySelectorAccessor = itemKeySelectorAccessor;
		_defaultItemKeySelector = defaultItemKeySelector;
	}

	public bool Equals(TItem x, TItem y)
	{
		var itemKeySelector = _itemKeySelectorAccessor();
		if (UsesDefaultEquality(itemKeySelector))
		{
			return EqualityComparer<TItem>.Default.Equals(x, y);
		}

		if ((x is null) || (y is null))
		{
			return (x is null) && (y is null);
		}

		return Object.Equals(itemKeySelector(x), itemKeySelector(y));
	}

	public int GetHashCode(TItem obj)
	{
		var itemKeySelector = _itemKeySelectorAccessor();
		if (UsesDefaultEquality(itemKeySelector))
		{
			return (obj is null) ? 0 : EqualityComparer<TItem>.Default.GetHashCode(obj);
		}

		return (obj is null) ? 0 : (itemKeySelector(obj)?.GetHashCode() ?? 0);
	}

	private bool UsesDefaultEquality(Func<TItem, object> itemKeySelector)
	{
		return (itemKeySelector is null) || (itemKeySelector == _defaultItemKeySelector);
	}
}
