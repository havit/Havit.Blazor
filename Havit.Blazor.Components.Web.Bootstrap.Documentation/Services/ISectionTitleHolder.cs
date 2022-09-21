using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public interface ISectionTitleHolder
{
	void RegisterNew(SectionTitle sectionTitle, string url);

	ICollection<SectionTitle> RetrieveAll(string url);

	void Clear();
}
