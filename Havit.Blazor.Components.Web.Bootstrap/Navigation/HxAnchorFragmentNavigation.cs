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
	/// <see href="https://github.com/dotnet/aspnetcore/issues/8393">GitHub Issue: Blazor 0.8.0: hash routing to named element #8393</see>.
	/// </summary>
	public class HxAnchorFragmentNavigation : ComponentBase, IAsyncDisposable
	{
		[Inject] protected NavigationManager NavigationManager { get; set; }
		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			NavigationManager.LocationChanged += OnLocationChanged;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && !String.IsNullOrEmpty(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Fragment))
			{
				await ScrollToAnchorAsync(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Fragment);
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		private void OnLocationChanged(object sender, LocationChangedEventArgs args)
		{
			InvokeAsync(async () =>
			{
				await ScrollToAnchorAsync(NavigationManager.ToAbsoluteUri(args.Location).Fragment);
			});
		}

		private async Task ScrollToAnchorAsync(string anchor = null, bool forceScroll = false)
		{
			if (!String.IsNullOrEmpty(anchor) || forceScroll)
			{
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/HxAnchorFragmentNavigation.js");
				await jsModule.InvokeVoidAsync("scrollToAnchor", anchor);
			}
		}

		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			NavigationManager.LocationChanged -= OnLocationChanged;

			if (jsModule is not null)
			{
				await jsModule.DisposeAsync();
			}
		}
	}
}
