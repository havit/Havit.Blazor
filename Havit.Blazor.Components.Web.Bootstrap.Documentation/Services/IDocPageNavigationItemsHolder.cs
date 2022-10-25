using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public interface IDocPageNavigationItemsHolder
{
	void RegisterNew(IDocPageNavigationItem item, string url);

	ICollection<IDocPageNavigationItem> RetrieveAll(string url);

	void Clear();
}
