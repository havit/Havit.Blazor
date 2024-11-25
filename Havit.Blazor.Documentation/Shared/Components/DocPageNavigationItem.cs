namespace Havit.Blazor.Documentation.Shared.Components;

public class DocPageNavigationItem
{
	public string Id { get; set; }
	public int Level { get; set; }

	public string Title { get; init; }

	public string GetItemUrl(string currentUrl)
	{
		return $"{currentUrl}#{Id}";
	}
}
