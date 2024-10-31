namespace Havit.Blazor.Documentation.Shared.Components;

public partial class PageCanonicalUrl
{
	private const string BaseUrl = "https://havit.blazor.eu";

	[Parameter] public string RelativeUrl { get; set; }

	private string GetCanonicalUrl()
	{
		return $"{BaseUrl}/{RelativeUrl.TrimStart('/')}".TrimEnd('/');
	}
}
