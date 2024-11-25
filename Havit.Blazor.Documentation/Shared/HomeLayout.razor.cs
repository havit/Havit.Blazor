using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Shared;

public partial class HomeLayout
{
	[Inject] protected NavigationManager NavigationManager { get; set; }

	private DocHeadContentTracker _docHeadContentTracker;

	protected override void OnInitialized()
	{
		_docHeadContentTracker = new DocHeadContentTracker(NavigationManager);
	}
}
