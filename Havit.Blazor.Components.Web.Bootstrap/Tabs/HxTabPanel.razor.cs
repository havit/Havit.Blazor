using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Tab panel (container).
	/// </summary>
	public partial class HxTabPanel : ComponentBase, IDisposable
	{
		/// <summary>
		/// ColumnsRegistration cascading value name.
		/// </summary>
		public const string TabsRegistrationCascadingValueName = "TabsRegistration";

		/// <summary>
		/// Tabs.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

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
				() => isDisposed,
				(HxTab tab) => tab.ActiveTabChangedAsync += HandleActiveTabChangedAsync, // when tab is added, subsribe to tab activated "event".
				(HxTab tab) => tab.ActiveTabChangedAsync -= HandleActiveTabChangedAsync); // when tab is removed, subsrive to tab activated "event". AFAIK we do not need to handle this in Dispose.
		}
	
		private async Task HandleActiveTabChangedAsync(HxTab newActiveTab)
		{
			// when new tab is activated, deactivates other tabs
			foreach (HxTab activeTab in tabsList.Where(item => item.IsCurrentlyActive && (item != newActiveTab)))
			{
				await activeTab.SetIsCurrentlyActiveAsync(false);
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				// when no tab is active after initial render, activate the first visible & enabled tab
				if (!tabsList.Any(item => item.IsCurrentlyActive) && (tabsList.Count > 0))
				{
					HxTab tabToActivate = tabsList.FirstOrDefault(tab => tab.IsEnabledEffective() && tab.IsVisible);
					if (tabToActivate != null)
					{
						await tabToActivate.SetIsCurrentlyActiveAsync(true);
					}
				}
			}
		}

		/// <summary>
		/// Handle click on tab title to activate tab.
		/// </summary>
		protected async Task HandleTabClick(HxTab tab)
		{
			await tab.SetIsCurrentlyActiveAsync(true);
		}
		/// <inheritdoc />
		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
