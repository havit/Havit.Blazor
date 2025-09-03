using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Collections;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Container for <see cref="HxTab"/>s for easier implementation of tabbed UI.<br/>
/// Encapsulates <see cref="HxNav"/> (<see cref="NavVariant.Tabs"/> variant as default) and <see cref="HxNavLink"/>s so you don't have to bother with them explicitly.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxTabPanel">https://havit.blazor.eu/components/HxTabPanel</see>
/// </summary>
public partial class HxTabPanel : ComponentBase
{
	/// <summary>
	/// TabsRegistration cascading value name.
	/// </summary>
	internal const string TabsRegistrationCascadingValueName = "TabsRegistration";

	/// <summary>
	/// The visual variant of the nav items.
	/// Default is <see cref="NavVariant.Tabs"/>.
	/// </summary>
	[Parameter] public NavVariant NavVariant { get; set; } = NavVariant.Tabs;

	/// <summary>
	/// Set to <see cref="TabPanelVariant.Card"/> if you want to wrap the tab panel in a card with the tab navigation in the header.
	/// </summary>
	[Parameter] public TabPanelVariant Variant { get; set; } = TabPanelVariant.Standard;

	/// <summary>
	/// Determines whether the content of all tabs is always rendered or only if the tab is active.<br />
	/// Default is <see cref="TabPanelRenderMode.AllTabs"/>.
	/// </summary>
	[Parameter] public TabPanelRenderMode RenderMode { get; set; } = TabPanelRenderMode.AllTabs;

	/// <summary>
	/// Card settings for the wrapping card.
	/// Applies only if <see cref="Variant"/> is set to <see cref="TabPanelVariant.Card"/>.
	/// </summary>
	[Parameter] public CardSettings CardSettings { get; set; }

	/// <summary>
	/// ID of the active tab (@bindable).
	/// </summary>
	[Parameter] public string ActiveTabId { get; set; }

	/// <summary>
	/// Raised when the ID of the active tab changes.
	/// </summary>
	[Parameter] public EventCallback<string> ActiveTabIdChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="ActiveTabIdChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeActiveTabIdChangedAsync(string newActiveTabId) => ActiveTabIdChanged.InvokeAsync(newActiveTabId);

	/// <summary>
	/// ID of the tab which should be active at the very beginning.<br />
	/// We are considering deprecating this parameter. Please use <see cref="ActiveTabId"/> instead (<c>@bind-ActiveTabId</c>).
	/// </summary>
	[Parameter] public string InitialActiveTabId { get; set; }

	/// <summary>
	/// Tabs.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	private HxTab _previousActiveTab;
	private List<HxTab> _tabsList = new();
	private List<HxTab> _tabsListOrdered; // cached
	private bool _collectingTabs;

	// Caches of method->delegate conversions
	private readonly RenderFragment _renderTabsNavigation;
	private readonly RenderFragment _renderTabsContent;

	public HxTabPanel()
	{
		_renderTabsContent = RenderTabsContent;
		_renderTabsNavigation = RenderTabsNavigation;
	}

	protected override async Task OnInitializedAsync()
	{
		if (!String.IsNullOrWhiteSpace(InitialActiveTabId))
		{
			await SetActiveTabIdAsync(InitialActiveTabId);
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		await NotifyActivationAndDeactivationAsync();
	}

	internal async Task SetActiveTabIdAsync(string newId)
	{
		if (ActiveTabId != newId)
		{
			ActiveTabId = newId;
			await InvokeActiveTabIdChangedAsync(newId);
			await NotifyActivationAndDeactivationAsync();
		}
	}

	private async Task NotifyActivationAndDeactivationAsync()
	{
		HxTab activeTab = _tabsList.FirstOrDefault(tab => IsActive(tab));
		if (activeTab == _previousActiveTab)
		{
			return;
		}

		// Store references for the async operations
		HxTab tabToDeactivate = _previousActiveTab;
		HxTab tabToActivate = activeTab;

		// Update _previousActiveTab BEFORE any async calls to prevent infinite loops
		// This ensures that if async callbacks trigger re-renders, subsequent calls to this method
		// will see that activeTab == _previousActiveTab and return early
		_previousActiveTab = activeTab;

		// Now perform the async operations
		if (tabToDeactivate != null)
		{
			await tabToDeactivate.NotifyDeactivatedAsync();
		}

		if (tabToActivate != null)
		{
			await tabToActivate.NotifyActivatedAsync();
		}
	}

	// Invoked by descendant tabs at a special time during rendering
	internal void AddTab(HxTab tab)
	{
		if (_collectingTabs)
		{
			_tabsList.Add(tab);
		}
	}

	private void StartCollectingTabs()
	{
		_tabsList.Clear();
		_collectingTabs = true;
	}

	private void FinishCollectingTabs()
	{
		_collectingTabs = false;
		_tabsListOrdered = _tabsList.OrderBy(tab => tab.Order).ToList();
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			// when no tab is active after the initial render, activate the first visible & enabled tab
			if (String.IsNullOrWhiteSpace(ActiveTabId))
			{
				var tabToActivate = GetDefaultActiveTab();
				if (tabToActivate != null)
				{
					await SetActiveTabIdAsync(tabToActivate.Id);
					StateHasChanged();
				}
			}
			else
			{
				_previousActiveTab = _tabsList.FirstOrDefault(tab => IsActive(tab));
			}
		}
	}

	/// <summary>
	/// Handle click on the tab title to activate the tab.
	/// </summary>
	protected async Task HandleTabClick(HxTab tab)
	{
		await SetActiveTabIdAsync(tab.Id);
	}

	private bool IsActive(HxTab tab)
	{
		if (String.IsNullOrWhiteSpace(ActiveTabId))
		{
			// no active tab set, activate the first visible & enabled tab
			return tab.Id == GetDefaultActiveTab()?.Id;
		}
		return ActiveTabId == tab.Id;
	}

	private HxTab GetDefaultActiveTab()
	{
		return _tabsList
					.OrderBy(tab => tab.Order)
					.FirstOrDefault(t => t.Visible && (((ICascadeEnabledComponent)t).Enabled ?? true));
	}

	protected string GetNavCssClassInCardMode()
	{
		if (Variant != TabPanelVariant.Card)
		{
			return null;
		}

		if (NavVariant == NavVariant.Pills)
		{
			return "card-header-pills";
		}
		else if (NavVariant == NavVariant.Tabs)
		{
			return "card-header-tabs";
		}

		return null;
	}
}
