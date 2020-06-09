using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class SuggestionRequest
	{
		public string UserInput { get; }

		public List<string> Suggestions { get; set; }

		public SuggestionRequest(string userInput)
		{
			UserInput = userInput;
		}
	}
}