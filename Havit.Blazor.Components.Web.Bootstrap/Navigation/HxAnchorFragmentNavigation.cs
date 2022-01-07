using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Temporarily (?) compensates Blazor deficiency in anchor-fragments (scrolling to <c>page#id</c> URLs). Supports navigation with <see cref="HxScrollspy"/> component.
	/// <a href="https://github.com/dotnet/aspnetcore/issues/8393">GitHub Issue: Blazor 0.8.0: hash routing to named element #8393</a>.
	/// </summary>
	public class HxAnchorFragmentNavigation : ComponentBase, IAsyncDisposable
	{
		/// <summary>
		/// Level of automation.
		/// Default is <see cref="AnchorFragmentNavigationAutomationMode.Full"/>.
		/// </summary>
		[Parameter] public AnchorFragmentNavigationAutomationMode Automation { get; set; } = AnchorFragmentNavigationAutomationMode.Full;

		[Inject] protected NavigationManager NavigationManager { get; set; }
		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;
		private string lastKnownLocation;
		private bool registerForScrollToCurrentUriFragmentAsyncOnAfterRender;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			lastKnownLocation = NavigationManager.Uri;
			NavigationManager.LocationChanged += OnLocationChanged;
		}

		public async Task ScrollToCurrentUriFragmentAsync()
		{
			if (!String.IsNullOrEmpty(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Fragment))
			{
				await ScrollToAnchorAsync(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Fragment);
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if ((firstRender && (Automation == AnchorFragmentNavigationAutomationMode.Full))
				|| registerForScrollToCurrentUriFragmentAsyncOnAfterRender)
			{
				registerForScrollToCurrentUriFragmentAsyncOnAfterRender = false;
				await ScrollToCurrentUriFragmentAsync();
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		public async Task ScrollToAnchorAsync(string anchor)
		{
			if (!String.IsNullOrEmpty(anchor))
			{
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxAnchorFragmentNavigation) + ".js");
				await jsModule.InvokeVoidAsync("scrollToAnchor", anchor);
			}
		}

		private void OnLocationChanged(object sender, LocationChangedEventArgs args)
		{
			if ((this.Automation == AnchorFragmentNavigationAutomationMode.Full)
				|| ((this.Automation == AnchorFragmentNavigationAutomationMode.SamePage)
							&& (NavigationManager.ToAbsoluteUri(lastKnownLocation).PathAndQuery == NavigationManager.ToAbsoluteUri(args.Location).PathAndQuery)))
			{
				InvokeAsync(() =>
				{
					//await Task.Delay(1);
					//await ScrollToCurrentUriFragmentAsync();
					registerForScrollToCurrentUriFragmentAsyncOnAfterRender = true;
					StateHasChanged();
				});
			}
			lastKnownLocation = args.Location;
		}

		public virtual async ValueTask DisposeAsync()
		{
			NavigationManager.LocationChanged -= OnLocationChanged;

			if (jsModule is not null)
			{
				await jsModule.DisposeAsync();
			}
		}
	}
}
