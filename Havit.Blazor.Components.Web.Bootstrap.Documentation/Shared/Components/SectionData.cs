namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public class SectionData : ISectionData
{
	public string Id { get; set; }
	public int Level { get; set; }

	public string TitleEffective { get; init; }

	public RenderFragment ChildContent { get; set; }

	public string GetSectionUrl(string currentUrl)
	{
		return $"{currentUrl}#{Id}";
	}
}
