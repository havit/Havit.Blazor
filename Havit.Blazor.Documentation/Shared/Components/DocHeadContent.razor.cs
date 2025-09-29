namespace Havit.Blazor.Documentation.Shared.Components;

/// <summary>
/// A component that renders <HeadContent> with metadata (such as canonical URL) for the current page.
/// Can be used several times on a page, but only the first occurence is used.
/// </summary>
public partial class DocHeadContent
{
	[Parameter] public string CanonicalRelativeUrl { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	[CascadingParameter] protected DocHeadContentTracker DocHeadContentTracker { get; set; }

	private bool _shouldRender = false;
	private string _canonicalAbsoluteUrl;

	protected override void OnParametersSet()
	{
		if (CanonicalRelativeUrl != null)
		{
			_shouldRender = DocHeadContentTracker.TryRegisterCanonicalUrlForCurrentPage(CanonicalRelativeUrl);
			if (_shouldRender)
			{
				_canonicalAbsoluteUrl = DocHeadContentTracker.GetAbsoluteCanonicalUrl();
			}
		}
	}
}
