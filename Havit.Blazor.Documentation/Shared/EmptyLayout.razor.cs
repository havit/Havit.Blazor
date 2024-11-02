using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Shared;

public partial class EmptyLayout
{
	[Inject] protected NavigationManager NavigationManager { get; set; }

	private PageCanonicalUrlTracker _pageCanonicalUrlTracker;

	protected override void OnInitialized()
	{
		_pageCanonicalUrlTracker = new PageCanonicalUrlTracker(NavigationManager);
	}
}
