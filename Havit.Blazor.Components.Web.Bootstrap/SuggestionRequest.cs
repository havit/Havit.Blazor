using System.Collections.Generic;
using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class SuggestionRequest
	{
		public string UserInput { get; }

		public CancellationToken CancellationToken { get; }

		public List<string> Suggestions { get; set; }

		public SuggestionRequest(string userInput, CancellationToken cancellationToken)
		{
			UserInput = userInput;
			CancellationToken = cancellationToken;
		}
	}
}