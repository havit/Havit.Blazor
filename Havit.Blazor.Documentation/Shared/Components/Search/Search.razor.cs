using Havit.Blazor.Documentation.Services;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Search : IAsyncDisposable
{
	[Inject] private NavigationManager NavigationManager { get; set; }
	[Inject] private IDocumentationCatalogService CatalogService { get; set; }
	[Inject] private IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private DotNetObjectReference<Search> _dotNetObjectReference;
	private bool _isMac;
	private SearchItem _selectedResult;

	private string _userInput;
	private List<SearchItem> _searchItems;
	private HxAutosuggest<SearchItem, SearchItem> _autosuggest;

	protected override void OnInitialized()
	{
		_searchItems = CatalogService.GetAll()
			.Select(ci => new SearchItem(ci.Href, ci.Title, ci.Keywords, ci.Level))
			.ToList();
	}

	private bool _disposed;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var isMacNew = await JSRuntime.InvokeAsync<bool>("eval", "navigator.platform.indexOf('Mac') > -1");
			if (isMacNew != _isMac)
			{
				_isMac = isMacNew;
				StateHasChanged();
			}

			_dotNetObjectReference = DotNetObjectReference.Create(this);
			_jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/Components/Search/Search.razor.js");
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initializeGlobalSearchShortcut", _dotNetObjectReference);

			await FocusSearchInputAsync();
		}
	}

	[JSInvokable]
	public async Task FocusSearchInputAsync()
	{
		if (_autosuggest is not null)
		{
			await _autosuggest.FocusAsync();
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

	public void NavigateToSelectedPage()
	{
		var href = _selectedResult.Href;
		_selectedResult = null;
		NavigationManager.NavigateTo(href);
	}

	public async ValueTask DisposeAsync()
	{
		_disposed = true;
		if (_jsModule is not null)
		{
			await _jsModule.InvokeVoidAsync("disposeGlobalSearchShortcut");
			await _jsModule.DisposeAsync();
		}
		_dotNetObjectReference?.Dispose();
	}
}
