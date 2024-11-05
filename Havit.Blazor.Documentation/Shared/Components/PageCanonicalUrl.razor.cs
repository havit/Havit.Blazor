namespace Havit.Blazor.Documentation.Shared.Components;

/// <summary>
/// A component that renders a canonical URL tag for the current page.
/// Can be used several times on a page, but only the first occurence is used.
/// </summary>
public partial class PageCanonicalUrl
{
	private const string BaseUrl = "https://havit.blazor.eu";

	[Parameter] public string RelativeUrl { get; set; }

	/// <summary>
	/// Determines, whether the <c>link</c> tag should be wrapped in <c>HeadContent</c>.
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool RenderInHeadContent { get; set; } = true;

	[CascadingParameter] protected PageCanonicalUrlTracker PageCanonicalUrlTracker { get; set; }

	private bool _shouldRender = false;
	private string _canonicalAbsoluteUrl;

	protected override void OnParametersSet()
	{
		if (RelativeUrl != null)
		{
			_shouldRender = PageCanonicalUrlTracker.TryRegisterCanonicalUrlForCurrentPage(RelativeUrl);
			if (_shouldRender)
			{
				_canonicalAbsoluteUrl = PageCanonicalUrlTracker.GetAbsoluteCanonicalUrl();
			}
		}
	}
}
