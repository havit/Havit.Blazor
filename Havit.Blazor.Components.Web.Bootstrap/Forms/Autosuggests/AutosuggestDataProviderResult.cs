namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider result for autosuggest data.
/// </summary>
public class AutosuggestDataProviderResult<TItem>
{
	/// <summary>
	/// The items provided by the request.
	/// </summary>
	public IEnumerable<TItem> Data { get; set; }
}
