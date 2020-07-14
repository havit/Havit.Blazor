using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Havit.Blazor.Components.Web.Bootstrap.Tabs
{
	public class HxTab : ComponentBase, IRenderNotificationComponent
	{
		/// <summary>
		/// Cascading parameter to register the tab.
		/// </summary>
		[CascadingParameter(Name = HxTabPanel.TabsRegistrationCascadingValueName)]
		protected CollectionRegistration<HxTab> TabsRegistration { get; set; }

		[Parameter] public string Title { get; set; }
		[Parameter] public RenderFragment TitleTemplate { get; set; }

		[Parameter] public RenderFragment Content { get; set; }

		// TODO: When activated via property notify deactivate other tabs.
		[Parameter] public bool IsActive { get; set; }
		[Parameter] public EventCallback<bool> IsActiveChanged { get; set; }

		[Parameter] public bool IsVisible { get; set; } = true;

		[CascadingParameter] public FormState FormState { get; set; }
		[Parameter] public bool? IsEnabled { get; set; }
		protected internal bool IsEnabledEffective => FormState?.IsEnabled ?? IsEnabled ?? true;

		RenderedEventHandler IRenderNotificationComponent.Rendered { get; set; }

		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			Contract.Requires(TabsRegistration != null, $"{nameof(HxTab)} invalid usage. Must be used in a {nameof(HxTabPanel)}.");
			TabsRegistration.Register(this);
		}

		internal async Task SetActiveAsync(bool newActive)
		{
			IsActive = newActive;
			await IsActiveChanged.InvokeAsync(IsActive);
			StateHasChanged();
		}

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);

			((IRenderNotificationComponent)this).Rendered?.Invoke(this, firstRender);
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			TabsRegistration.Unregister(this);
		}
	}
}
