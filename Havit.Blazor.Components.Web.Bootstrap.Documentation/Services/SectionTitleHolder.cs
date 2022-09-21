using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public class SectionTitleHolder : ISectionTitleHolder
{
	private Dictionary<string, List<SectionTitle>> sectionTitles = new();

	public void RegisterNew(SectionTitle sectionTitle, string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);

		if (!sectionTitles[page].Any(st => st.Id == sectionTitle.Id))
		{
			sectionTitles[page].Add(sectionTitle);
		}
	}

	public ICollection<SectionTitle> RetrieveAll(string url)
	{
		string page = GetPageFromUrl(url);
		EnsureKey(page);
		return sectionTitles[page];
	}

	private void EnsureKey(string page)
	{
		if (!sectionTitles.ContainsKey(page))
		{
			sectionTitles.Add(page, new List<SectionTitle>());
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
