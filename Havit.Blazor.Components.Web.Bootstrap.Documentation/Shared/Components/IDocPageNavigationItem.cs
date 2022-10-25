namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public interface IDocPageNavigationItem
{
	string Id { get; }
	int Level { get; }
	string Title { get; }
	RenderFragment ChildContent { get; }

	string GetItemUrl(string currentUrl);
}
