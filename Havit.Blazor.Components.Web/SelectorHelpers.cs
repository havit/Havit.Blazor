namespace Havit.Blazor.Components.Web;

/// <summary>
/// Helper methods for selectors.
/// </summary>
public static class SelectorHelpers
{
	/// <summary>
	/// Returns text from an item based on the textSelector.
	/// When the textSelector is <c>null</c>, it returns item.ToString().
	/// It never returns <c>null</c>. Instead, <c>null</c> values are converted to an empty string.
	/// </summary>
	public static string GetText<TItem>(Func<TItem, string> textSelector, TItem item)
	{
		return (textSelector != null)
			? textSelector.Invoke(item) ?? String.Empty
			: item?.ToString() ?? String.Empty;
	}

	/// <summary>
	/// When the item is <c>null</c>, it returns <c>default(TValue)</c>.
	/// Otherwise, it returns the value from the item based on the valueSelector.
	/// When the valueSelector is <c>null</c> and <c>TValue</c> is the same as <c>TItem</c>, it returns the item.
	/// </summary>
	public static TValue GetValue<TItem, TValue>(Func<TItem, TValue> valueSelector, TItem item)
	{
		if (item == null)
		{
			return default;
		}

		if (valueSelector != null)
		{
			return valueSelector(item);
		}

		if (typeof(TValue) == typeof(TItem))
		{
			return (TValue)(object)item;
		}

		throw new InvalidOperationException("ValueSelector is required.");
	}

	/// <summary>
	/// Returns whether an item is disabled based on the itemDisabledSelector.
	/// When the itemDisabledSelector is <c>null</c>, always returns <c>false</c> (item is enabled).
	/// When the item is <c>null</c>, always returns <c>false</c> (item is enabled).
	/// </summary>
	public static bool GetDisabled<TItem>(Func<TItem, bool> itemDisabledSelector, TItem item)
	{
		if ((item == null) || (itemDisabledSelector == null))
		{
			return false;
		}

		return itemDisabledSelector(item);
	}
}
