using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public class SearchItem
	{
		public string Href { get; }
		public string Title { get; }
		public string Keywords { get; }
		public int Level { get; set; }


		public SearchItem() { }
		public SearchItem(string href, string title, string keywords)
		{
			Href = href;
			Title = title;
			Keywords = keywords;
			Level = title.Count(c => c == '>');
		}

		public override string ToString()
		{
			return Title;
		}
	}
}
