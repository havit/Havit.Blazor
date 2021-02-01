using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class AutosuggestDataProviderRequest
	{
		public string UserInput { get; init;  }

		public CancellationToken CancellationToken { get; init; }
	}
}