namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider (delegate).
	/// </summary>
	public delegate Task<AutosuggestDataProviderResult<TItem>> AutosuggestDataProviderDelegate<TItem>(AutosuggestDataProviderRequest request);
}
