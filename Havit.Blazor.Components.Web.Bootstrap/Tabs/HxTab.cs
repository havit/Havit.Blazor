using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Havit.Blazor.Components.Web.Forms;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Tab in tab panel.
	/// </summary>
	public class HxTab : ComponentBase, IRenderNotificationComponent, ICascadeEnabledComponent
	{
		/// <summary>
		/// Cascading parameter to register the tab.
		/// </summary>
		[CascadingParameter(Name = HxTabPanel.TabsRegistrationCascadingValueName)]
		protected CollectionRegistration<HxTab> TabsRegistration { get; set; }

		/// <summary>
		/// Tab title.
		/// </summary>
		[Parameter] public string Title { get; set; }

		/// <summary>
		/// Tab title template.
		/// </summary>
		[Parameter] public RenderFragment TitleTemplate { get; set; }

		/// <summary>
		/// Content of the tab.
		/// </summary>
		[Parameter] public RenderFragment Content { get; set; }

		/// <summary>
		/// True for just one tab which should be initially active (when using IsCurrentlyActive is overkill).
		/// </summary>
		[Parameter] public bool IsInitiallyActive { get; set; }	 // TODO RH: Naming 

		/// <summary>
		/// Indicates currently tab activation/deactivation. Can be used only with data-binding.
		/// </summary>
		[Parameter] public bool IsCurrentlyActive { get; set; }

		/// <summary>
		/// Notifies change when a tab is activated or deactivated.
		/// </summary>
		[Parameter] public EventCallback<bool> IsCurrentlyActiveChanged { get; set; }

		/// <summary>
		/// True for visible tab. Set false when tab should not be visible.
		/// </summary>
		[Parameter] public bool Visible { get; set; } = true;

		/// <inheritdoc />
		[CascadingParameter] public FormState FormState { get; set; }

		/// <inheritdoc />
		[Parameter] public bool? Enabled { get; set; }

		RenderedEventHandler IRenderNotificationComponent.Rendered { get; set; }

		internal TabActivatedDelegate ActiveTabChangedAsync { get; set; }
		internal delegate Task TabActivatedDelegate(HxTab newActiveTab);

		/// <inheritdoc />
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			
			Contract.Assert(TabsRegistration != null, $"{nameof(HxTab)} invalid usage. Must be used in a {nameof(HxTabPanel)}.");
			TabsRegistration.Register(this);

			// When the tab should be initially active, set it currently active (just here in OnInitializedAsync).
			if (IsInitiallyActive && !IsCurrentlyActive)
			{
				await SetIsCurrentlyActiveAsync(true);
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (!firstRender)
			{
				// support for data-binding in IsCurrentlyActive - when a tab is currently activated, deactivate other tabs
				if (!previousIsCurrentlyActive && IsCurrentlyActive)
				{
					await ActiveTabChangedAsync.Invoke(this);
				}
			}
			previousIsCurrentlyActive = IsCurrentlyActive;

			((IRenderNotificationComponent)this).Rendered?.Invoke(this, firstRender);
		}
		private bool previousIsCurrentlyActive;

		/// <summary>
		/// Sets value to IsCurrentlyActive and fires IsCurrentlyActiveChanged.
		/// </summary>
		internal async Task SetIsCurrentlyActiveAsync(bool newIsCurrentlyActive)
		{
			if (IsCurrentlyActive != newIsCurrentlyActive)
			{
				IsCurrentlyActive = newIsCurrentlyActive;
				await IsCurrentlyActiveChanged.InvokeAsync(IsCurrentlyActive);

				// method can be called from HxTabPanel so we need to notify state change.
				StateHasChanged();
			}
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			TabsRegistration.Unregister(this);
		}
	}
}
