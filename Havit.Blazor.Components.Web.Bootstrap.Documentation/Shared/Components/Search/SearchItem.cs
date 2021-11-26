using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

		public int GetRelevance(string userInput)
		{
			if (Title.Contains(userInput))
			{
				return 6;
			}
			else if (Title.Contains(userInput, StringComparison.OrdinalIgnoreCase))
			{
				return 5;
			}
			else if (Regex.IsMatch(Title, GetPattern(userInput)))
			{
				return 4;
			}
			else if (Regex.IsMatch(Title, GetPattern(userInput), RegexOptions.IgnoreCase))
			{
				return 3;
			}
			else if (Keywords.Contains(userInput))
			{
				return 2;
			}
			else if (Keywords.Contains(userInput, StringComparison.OrdinalIgnoreCase))
			{
				return 1;
			}

			return 0;
		}

		private string GetPattern(string userInput)
		{
			var pattern = userInput.ToCharArray().Select(c => Regex.Escape(c.ToString()));
			var allowedCharacters = @"[A-Za-z\> ]*";

			return string.Join(allowedCharacters, pattern);
		}

		public override string ToString()
		{
			return Title;
		}
	}
}
