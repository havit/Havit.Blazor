namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider (delegate) for <see cref="HxInputTags" />.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/InputTagsDataProviderDelegate">https://havit.blazor.eu/types/InputTagsDataProviderDelegate</see>	
	/// </summary>
	public delegate Task<InputTagsDataProviderResult> InputTagsDataProviderDelegate(InputTagsDataProviderRequest request);
}

