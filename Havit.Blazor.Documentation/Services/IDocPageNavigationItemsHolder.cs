using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Services;

public interface IDocPageNavigationItemsHolder
{
	void RegisterNew(IDocPageNavigationItem item, string url);

	ICollection<IDocPageNavigationItem> RetrieveAll(string url);

	void Clear();
}
