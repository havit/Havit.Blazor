using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public interface ISectionDataHolder
{
	void RegisterNew(ISectionData sectionTitle, string url);

	ICollection<ISectionData> RetrieveAll(string url);

	void Clear();
}
