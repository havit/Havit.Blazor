using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public class SearchItem
	{
		public string Href { get; }
		public string Title { get; }
		public string Keywords { get; }

		public SearchItem() { }
		public SearchItem(string href, string title, string keywords)
		{
			Href = href;
			Title = title;
			Keywords = keywords;
		}

		public override string ToString()
		{
			return Title;
		}
	}
}
