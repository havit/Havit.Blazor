using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class OnThisPageNavigation : IDisposable
{
	[Inject] public IDocPageNavigationItemsHolder DocPageNavigationItemsHolder { get; set; }
	[Inject] public NavigationManager NavigationManager { get; set; }

	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	private IEnumerable<IDocPageNavigationItem> Items { get; set; }

	protected override void OnInitialized()
	{
		NavigationManager.LocationChanged += LoadItems;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			Items = DocPageNavigationItemsHolder.RetrieveAll(NavigationManager.Uri);
			StateHasChanged();
		}
	}

	private void LoadItems(object sender, LocationChangedEventArgs eventArgs)
	{
		Items = DocPageNavigationItemsHolder.RetrieveAll(eventArgs.Location);
		StateHasChanged();
	}

	private RenderFragment GenerateNavigationTree() => builder =>
	{
		if (!Items.Any())
		{
			return;
		}

		var items = Items.ToList();
		var topLevel = items.Min(i => i.Level);

		// if there is only single top-level item, we don't need to render the top-level list
		if (items.Count(i => i.Level == topLevel) == 1)
		{
			items = items.Where(i => i.Level != topLevel).ToList();
			topLevel = items.Min(i => i.Level);
		}

		var currentLevel = topLevel;
		int sequence = 1;
		for (int i = 0; i < items.Count; i++)
		{
			IDocPageNavigationItem item = items[i];

			// Handle level adjustments - nested lists.
			int levelDifference = Math.Abs(item.Level - currentLevel);
			if (item.Level > currentLevel)
			{
				for (int j = 0; j < levelDifference; j++)
				{
					builder.OpenElement(sequence++, "ul");
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
			builder.OpenElement(sequence++, "li");

			builder.OpenElement(sequence++, "a");
			builder.AddAttribute(sequence++, "href", item.GetItemUrl(NavigationManager.Uri)); // TODO direct usage of HxAnchorFragmentNavigation.ScrollToAnchorAsync() ?
			builder.AddAttribute(sequence++, "class", "text-secondary mb-1 text-truncate");
			builder.AddContent(sequence++, item.Title);
			builder.AddContent(sequence++, item.ChildContent);
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
		NavigationManager.LocationChanged -= LoadItems;
	}
}
