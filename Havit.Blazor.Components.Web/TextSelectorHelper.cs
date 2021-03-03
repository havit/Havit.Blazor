using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Helper methods for text selectors.
	/// </summary>
	public static class TextSelectorHelper
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
	}
}
