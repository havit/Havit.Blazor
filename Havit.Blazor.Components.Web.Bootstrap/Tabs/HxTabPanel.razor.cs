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
	/// Tab panel (container).
	/// </summary>
	public partial class HxTabPanel : ComponentBase, IAsyncDisposable
	{
		/// <summary>
		/// TabsRegistration cascading value name.
		/// </summary>
		internal const string TabsRegistrationCascadingValueName = "TabsRegistration";

		[Parameter] public string ActiveTabId { get; set; }
		[Parameter] public EventCallback<string> ActiveTabIdChanged { get; set; }
		[Parameter] public string InitialActiveTabId { get; set; }

		/// <summary>
		/// Tabs.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private HxTab previousActiveTab;
		private List<HxTab> tabsList;
		private CollectionRegistration<HxTab> tabsListRegistration;
		private bool isDisposed = false;

		/// <summary>
		/// Constructor.
		/// </summary>
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
