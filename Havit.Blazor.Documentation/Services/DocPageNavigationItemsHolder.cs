using Havit.Blazor.Documentation.Shared.Components;

namespace Havit.Blazor.Documentation.Services;

public class DocPageNavigationItemsHolder : IDocPageNavigationItemsHolder
{
	private Dictionary<string, List<IDocPageNavigationItem>> items = new();

	public void RegisterNew(IDocPageNavigationItem item, string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);

		if (!items[page].Any(st => st.Id == item.Id))
		{
			items[page].Add(item);
		}
	}

	public ICollection<IDocPageNavigationItem> RetrieveAll(string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);
		return items[page];
	}

	private void EnsureKey(string page)
	{
		if (!items.ContainsKey(page))
		{
			items.Add(page, new List<IDocPageNavigationItem>());
		}
	}

	private string GetPageFromUrl(string url)
	{
		return url?.Split('#')[0];
	}

	public void Clear()
	{
		items.Clear();
	}
}
