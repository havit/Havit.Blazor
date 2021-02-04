using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider result for autosuggest data.
	/// </summary>
	public class AutosuggestDataProviderResult<TItemType>
	{
		/// <summary>
		/// The provided items by the request.
		/// </summary>
		public IEnumerable<TItemType> Items { get; set; }
	}
}