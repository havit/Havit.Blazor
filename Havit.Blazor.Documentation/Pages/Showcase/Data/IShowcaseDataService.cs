
namespace Havit.Blazor.Documentation.Pages.Showcase.Data;

public interface IShowcaseDataService
{
	ShowcaseModel GetNextShowcase(string currentKey);
	ShowcaseModel GetPreviousShowcase(string currentKey);
	ShowcaseModel GetShowcase(string key);
	IEnumerable<ShowcaseModel> GetShowcases();
}