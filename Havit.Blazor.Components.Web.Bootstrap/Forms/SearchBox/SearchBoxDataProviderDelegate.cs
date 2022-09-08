namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider (delegate).
/// </summary>
public delegate Task<SearchBoxDataProviderResult<TItem>> SearchBoxDataProviderDelegate<TItem>(SearchBoxDataProviderRequest request);
