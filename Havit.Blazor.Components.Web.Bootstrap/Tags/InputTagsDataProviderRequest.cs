using System.Threading;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider request for <see cref="HxInputTags"/> data.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/InputTagsDataProviderRequest">https://havit.blazor.eu/types/InputTagsDataProviderRequest</see>
	/// </summary>
	public class InputTagsDataProviderRequest
	{
		/// <summary>
		/// Current user input for new tag suggestions.
		/// </summary>
		public string UserInput { get; init; }

		/// <summary>
		/// The <see cref="System.Threading.CancellationToken"/> used to relay cancellation of the request.
		/// </summary>
		public CancellationToken CancellationToken { get; init; }
	}
}