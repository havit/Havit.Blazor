namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public interface ISectionData
{
	string Id { get; set; }
	int Level { get; set; }
	string TitleEffective { get; }
	RenderFragment ChildContent { get; set; }

	string GetSectionUrl(string currentUrl);
}
