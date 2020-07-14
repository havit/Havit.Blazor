using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tabs
{
	
	// TODO: Activate first non-disabled tab if possible and no tab active

	public partial class HxTabPanel : ComponentBase, IDisposable
	{
		public const string TabsRegistrationCascadingValueName = "TabsRegistration";

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
			tabsListRegistration = new CollectionRegistration<HxTab>(tabsList, this.StateHasChanged, () => isDisposed);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				if (!tabsList.Any(item => item.IsActive) && (tabsList.Count > 0))
				{
					await tabsList[0].SetActiveAsync(true);
					StateHasChanged(); // TODO: Je potřeba? Nezpropaguje se informace o vybrání z metody SetActive?
				}
			}
		}

		protected async Task HandleClick(HxTab tab)
		{
			foreach (HxTab activeTab in tabsList.Where(item => item.IsActive))
			{
				await activeTab.SetActiveAsync(false);
			}
			await tab.SetActiveAsync(true);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			isDisposed = true;
		}
	}
}
