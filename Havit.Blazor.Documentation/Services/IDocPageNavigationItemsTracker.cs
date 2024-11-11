using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Services;

public interface IDocPageNavigationItemsTracker
{
	void RegisterNavigationItem(string url, DocPageNavigationItem item);

	List<DocPageNavigationItem> GetPageNavigationItems(string url);
}
