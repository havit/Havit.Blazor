using Havit.Blazor.Components.Web.Infrastructure;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Container for <see cref="HxTab"/>s for easier implementation of tabbed UI.<br/>
/// Encapsulates <see cref="HxNav"/> (<see cref="NavVariant.Tabs"/> variant as default) and <see cref="HxNavLink"/>s so you don't have to bother with them explicitly.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxTabPanel">https://havit.blazor.eu/components/HxTabPanel</see>
/// </summary>
public partial class HxTabPanel : ComponentBase, IAsyncDisposable
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
	/// Whether to wrap the tab panel in a card with the tab navigation in the header.
	/// </summary>
	[Parameter] public TabPanelRenderMode RenderMode { get; set; } = TabPanelRenderMode.Standard;

	/// <summary>
	/// Card settings for the wrapping card.
	/// Applies only if <see cref="RenderMode"/> is set to <see cref="TabPanelRenderMode.Card"/>.
	/// </summary>
	[Parameter] public CardSettings CardSettings { get; set; }

	/// <summary>
	/// ID of the active tab (@bindable).
	/// </summary>
	[Parameter] public string ActiveTabId { get; set; }

	/// <summary>
	/// Raised when ID of the active tab changes.
	/// </summary>
	[Parameter] public EventCallback<string> ActiveTabIdChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="ActiveTabIdChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeActiveTabIdChangedAsync(string newActiveTabId) => ActiveTabIdChanged.InvokeAsync(newActiveTabId);

	/// <summary>
	/// ID of the tab which should be active at the very beginning.
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

	private HxTab previousActiveTab;
	private List<HxTab> tabsList;
	private CollectionRegistration<HxTab> tabsListRegistration;
	private bool isDisposed = false;

	public HxTabPanel()
	{
		tabsList = new List<HxTab>();
		tabsListRegistration = new CollectionRegistration<HxTab>(tabsList,
			async () => await InvokeAsync(this.StateHasChanged),
			() => isDisposed);
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (!String.IsNullOrWhiteSpace(InitialActiveTabId))
		{
			await SetActiveTabIdAsync(InitialActiveTabId);
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		await NotifyActivationAndDeactivationAsync();
	}

	internal async Task SetActiveTabIdAsync(string newId)
	{
		if (this.ActiveTabId != newId)
		{
			ActiveTabId = newId;
			await InvokeActiveTabIdChangedAsync(newId);
			await NotifyActivationAndDeactivationAsync();
		}
	}

	private async Task NotifyActivationAndDeactivationAsync()
	{
		HxTab activeTab = tabsList.FirstOrDefault(tab => IsActive(tab));
		if (activeTab == previousActiveTab)
		{
			return;
		}

		if (previousActiveTab != null)
		{
			await previousActiveTab.NotifyDeactivatedAsync();
		}

		if (activeTab != null)
		{
			await activeTab.NotifyActivatedAsync();
		}

		previousActiveTab = activeTab;
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			// when no tab is active after initial render, activate the first visible & enabled tab
			if (!tabsList.Any(tab => IsActive(tab)) && (tabsList.Count > 0))
			{
				HxTab tabToActivate = tabsList.FirstOrDefault(tab => CascadeEnabledComponent.EnabledEffective(tab) && tab.Visible);
				if (tabToActivate != null)
				{
					await SetActiveTabIdAsync(tabToActivate.Id);
				}
			}
		}
	}

	/// <summary>
	/// Handle click on tab title to activate tab.
	/// </summary>
	protected async Task HandleTabClick(HxTab tab)
	{
		await SetActiveTabIdAsync(tab.Id);
	}

	private bool IsActive(HxTab tab)
	{
		return ActiveTabId == tab.Id;
	}

	protected string GetNavCssClassInCardMode()
	{
		if (RenderMode != TabPanelRenderMode.Card)
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

	/// <inheritdoc />

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (!isDisposed && (previousActiveTab != null))
		{
			await previousActiveTab.NotifyDeactivatedAsync();
			previousActiveTab = null;
		}

		isDisposed = true;
	}
}
