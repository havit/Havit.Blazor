using Havit.Blazor.Documentation.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class OnThisPageNavigation(
	IDocPageNavigationItemsTracker docPageNavigationItemsHolder,
	NavigationManager navigationManager) : IDisposable
{
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	private readonly IDocPageNavigationItemsTracker _docPageNavigationItemsHolder = docPageNavigationItemsHolder;
	private readonly NavigationManager _navigationManager = navigationManager;

	private List<DocPageNavigationItem> _items;

	protected override void OnInitialized()
	{
		_navigationManager.LocationChanged += LoadItems;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			_items = _docPageNavigationItemsHolder.GetPageNavigationItems(_navigationManager.Uri);
			StateHasChanged();
		}
	}

	private void LoadItems(object sender, LocationChangedEventArgs eventArgs)
	{
		_items = _docPageNavigationItemsHolder.GetPageNavigationItems(eventArgs.Location);
		StateHasChanged();
	}

	private RenderFragment GenerateNavigationTree() => builder =>
	{
		if (!_items.Any())
		{
			return;
		}

		List<DocPageNavigationItem> itemsToRender;
		int topLevel;

		// if there is only single top-level item, we don't need to render the top-level list
		if (_items.Count(i => i.Level == 1) == 1)
		{
			itemsToRender = _items.Where(i => i.Level != 1).ToList();
			topLevel = 2;
		}
		else
		{
			itemsToRender = _items;
			topLevel = 1;
		}

		string _currentUriWithoutFragment = _navigationManager.Uri.Split('#')[0]; // get the current URL without the fragment part
		var currentLevel = topLevel;
		foreach (var item in itemsToRender)
		{
			// Handle level adjustments - nested lists.
			int levelDifference = Math.Abs(item.Level - currentLevel);
			if (item.Level > currentLevel)
			{
				for (int j = 0; j < levelDifference; j++)
				{
					builder.OpenElement(71, "ul");
				}
			}
			else if (item.Level < currentLevel)
			{
				for (int j = 0; j < levelDifference; j++)
				{
					builder.CloseElement();
				}
			}
			currentLevel = item.Level;

			// Render the list item and a link to the heading.
			builder.OpenElement(84, "li");

			builder.OpenElement(86, "a");
			builder.AddAttribute(87, "href", item.GetItemUrl(_currentUriWithoutFragment)); // TODO direct usage of HxAnchorFragmentNavigation.ScrollToAnchorAsync() ?
			builder.AddAttribute(88, "class", "text-secondary mb-1 text-truncate");
			builder.AddContent(89, item.Title);
			builder.CloseElement();

			builder.CloseElement();
		}

		for (int j = topLevel; j < currentLevel; j++)
		{
			builder.CloseElement();
		}
	};

	public void Dispose()
	{
		_navigationManager.LocationChanged -= LoadItems;
	}
}
