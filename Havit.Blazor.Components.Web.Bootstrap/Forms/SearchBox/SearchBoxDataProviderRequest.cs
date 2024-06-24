namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider request for search box data.
/// </summary>
public class SearchBoxDataProviderRequest
{
	/// <summary>
	/// Current input entered in the search box.
	/// </summary>
	public string UserInput { get; init; }

	/// <summary>
	/// The <see cref="System.Threading.CancellationToken"/> used to indicate cancellation of the request.
	/// </summary>
	public CancellationToken CancellationToken { get; init; }
}
