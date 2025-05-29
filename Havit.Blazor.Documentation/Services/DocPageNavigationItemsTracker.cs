using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Services;

public class DocPageNavigationItemsTracker : IDocPageNavigationItemsTracker
{
	private readonly Dictionary<string, List<DocPageNavigationItem>> _itemsByPage = new();

	public void RegisterNavigationItem(string url, DocPageNavigationItem item)
	{
		Contract.Requires<ArgumentNullException>(item != null);

		var pageKey = UrlHelper.RemoveFragmentFromUrl(url);

		if (!_itemsByPage.ContainsKey(pageKey))
		{
			_itemsByPage.Add(pageKey, [item]);
		}
		else if (!_itemsByPage[pageKey].Exists(st => st.Id == item.Id))
		{
			_itemsByPage[pageKey].Add(item);
		}
	}

	public List<DocPageNavigationItem> GetPageNavigationItems(string url)
	{
		string pageKey = UrlHelper.RemoveFragmentFromUrl(url);

		if (_itemsByPage.TryGetValue(pageKey, out var items))
		{
			return items;
		}
		return [];
	}
}
