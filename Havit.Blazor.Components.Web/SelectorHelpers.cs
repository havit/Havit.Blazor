using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Helper methods for selectors.
	/// </summary>
	public static class SelectorHelpers
	{
		/// <summary>
		/// Returns text from item by textSelector.
		/// When textSelector is null, returns item.ToString().
		/// Never returns null, null values are converted to empty string.
		/// </summary>
		public static string GetText<TItem>(Func<TItem, string> textSelector, TItem item)
		{
			return (textSelector != null)
				? textSelector.Invoke(item) ?? String.Empty
				: item?.ToString() ?? String.Empty;
		}

		/// <summary>
		/// When item is null, returns default(TValue).
		/// Otherwise returns value from item by valueSelector.
		/// When valueSelector is null and TValue is same as TItem, returns item.
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
	}
}
