using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Services;

public class DocPageNavigationItemsHolder : IDocPageNavigationItemsHolder
{
	private Dictionary<string, List<IDocPageNavigationItem>> _items = new();

	public void RegisterNew(IDocPageNavigationItem item, string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);

		if (!_items[page].Any(st => st.Id == item.Id))
		{
			_items[page].Add(item);
		}
	}

	public ICollection<IDocPageNavigationItem> RetrieveAll(string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);
		return _items[page];
	}

	private void EnsureKey(string page)
	{
		if (!_items.ContainsKey(page))
		{
			_items.Add(page, new List<IDocPageNavigationItem>());
		}
	}

	private string GetPageFromUrl(string url)
	{
		return url?.Split('#')[0];
	}

	public void Clear()
	{
		_items.Clear();
	}
}
