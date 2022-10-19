using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public class SectionDataHolder : ISectionDataHolder
{
	private Dictionary<string, List<ISectionData>> sectionTitles = new();

	public void RegisterNew(ISectionData sectionTitle, string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);

		if (!sectionTitles[page].Any(st => st.Id == sectionTitle.Id))
		{
			sectionTitles[page].Add(sectionTitle);
		}
	}

	public ICollection<ISectionData> RetrieveAll(string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);
		return sectionTitles[page];
	}

	private void EnsureKey(string page)
	{
		if (!sectionTitles.ContainsKey(page))
		{
			sectionTitles.Add(page, new List<ISectionData>());
		}
	}

	private string GetPageFromUrl(string url)
	{
		return url?.Split('#')[0];
	}

	public void Clear()
	{
		sectionTitles.Clear();
	}
}
