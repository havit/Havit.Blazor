using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Container for <see cref="HxTab"/>s for easier implementation of tabbed UI.
	/// Encapsulates <see cref="HxNav"/> (<see cref="NavVariant.Tabs"/> variant) and <see cref="HxNavLink"/>s so you don't have to bother with them explicitly.
	/// </summary>
	public partial class HxTabPanel : ComponentBase, IAsyncDisposable
	{
		/// <summary>
		/// TabsRegistration cascading value name.
		/// </summary>
		internal const string TabsRegistrationCascadingValueName = "TabsRegistration";

		/// <summary>
		/// ID of the active tab (@bindable).
		/// </summary>
		[Parameter] public string ActiveTabId { get; set; }

		/// <summary>
		/// Raised when ID of the active tab changes.
		/// </summary>
		[Parameter] public EventCallback<string> ActiveTabIdChanged { get; set; }

		/// <summary>
		/// ID of the tab which should be active at the very beginning.
		/// </summary>
		[Parameter] public string InitialActiveTabId { get; set; }

		/// <summary>
		/// Tabs.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private HxTab previousActiveTab;
		private List<HxTab> tabsList;
		private CollectionRegistration<HxTab> tabsListRegistration;
		private bool isDisposed = false;

		public HxTabPanel()
		{
			tabsList = new List<HxTab>();
			tabsListRegistration = new CollectionRegistration<HxTab>(tabsList,
				this.StateHasChanged,
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
			await NotifyActivationAndDeativatationAsync();
		}

		internal async Task SetActiveTabIdAsync(string newId)
		{
			if (this.ActiveTabId != newId)
			{
				ActiveTabId = newId;
				await ActiveTabIdChanged.InvokeAsync(newId);

				await NotifyActivationAndDeativatationAsync();
			}
		}

		private async Task NotifyActivationAndDeativatationAsync()
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

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (!isDisposed && (previousActiveTab != null))
			{
				await previousActiveTab.NotifyDeactivatedAsync();
				previousActiveTab = null;
			}
			isDisposed = true;
		}
	}
}
