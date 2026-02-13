using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Search
{
	[Inject] private NavigationManager NavigationManager { get; set; }
	[Inject] private IDocumentationCatalogService CatalogService { get; set; }

	private SearchItem SelectedResult
	{
		get
		{
			return null;
		}
		set
		{
			NavigateToSelectedPage(value);
		}
	}

	private string _userInput;
	private List<SearchItem> _searchItems;

	private HxAutosuggest<SearchItem, SearchItem> _autosuggest;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_searchItems = CatalogService.GetAll()
				.Select(ci => new SearchItem(ci.Href, ci.Title, ci.Keywords, ci.Level))
				.ToList();

			if (_autosuggest is not null)
			{
				await _autosuggest.FocusAsync();
			}
		}
	}

	private Task<AutosuggestDataProviderResult<SearchItem>> ProvideSuggestions(AutosuggestDataProviderRequest request)
	{
		_userInput = request.UserInput.Trim();

		return Task.FromResult(new AutosuggestDataProviderResult<SearchItem>
		{
			Data = GetSearchItems()
		});
	}

	private IEnumerable<SearchItem> GetSearchItems()
	{
		return _searchItems
				.Where(si => si.GetRelevance(_userInput) > 0)
				.OrderBy(si => si.Level)
					.ThenByDescending(si => si.GetRelevance(_userInput))
					.ThenBy(si => si.Title)
				.Take(8);
	}

	public void NavigateToSelectedPage(SearchItem searchItem)
	{
		NavigationManager.NavigateTo(searchItem.Href);
	}
}
