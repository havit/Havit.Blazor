namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider request for <see cref="HxInputTags"/> data.
/// </summary>
public class InputTagsDataProviderRequest
{
	/// <summary>
	/// Current user input for new tag suggestions.
	/// </summary>
	public string UserInput { get; init; }

	/// <summary>
	/// The <see cref="System.Threading.CancellationToken"/> used to indicate cancellation of the request.
	/// </summary>
	public CancellationToken CancellationToken { get; init; }
}
