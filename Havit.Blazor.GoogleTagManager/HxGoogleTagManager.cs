using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.GoogleTagManager
{
	/// <inheritdoc/>
	public class HxGoogleTagManager : IHxGoogleTagManager, IAsyncDisposable
	{
		private readonly HxGoogleTagManagerOptions gtmOptions;
		private readonly NavigationManager navigationManager;
		private readonly IJSRuntime jsRuntime;

		private bool isInitialized;
		private IJSObjectReference jsModule;

		public HxGoogleTagManager(
			IOptions<HxGoogleTagManagerOptions> gtmOptions,
			NavigationManager navigationManager,
			IJSRuntime jsRuntime)
		{
			this.gtmOptions = gtmOptions.Value;
			this.navigationManager = navigationManager;
			this.jsRuntime = jsRuntime;
		}

		/// <inheritdoc/>
		public async Task InitializeAsync()
		{
			if (isInitialized)
			{
				return;
			}

			jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.GoogleTagManager/" + nameof(HxGoogleTagManager) + ".js");

			await jsModule.InvokeVoidAsync("initialize", gtmOptions.GtmId);

			isInitialized = true;
		}

		/// <inheritdoc/>
		public async Task PushAsync(object data)
		{
			await InitializeAsync();
			await jsModule.InvokeVoidAsync("push", data);
		}

		/// <inheritdoc/>
		public async Task PushEventAsync(string eventName, object eventData = null)
		{
			await InitializeAsync();
			await jsModule.InvokeVoidAsync("pushEvent", eventName, eventData);
		}

		/// <inheritdoc/>
		public async Task PushPageView(object additionalData = null)
		{
			await PushPageViewCoreAsync(navigationManager.Uri, additionalData);
		}

		/// <inheritdoc/>
		async Task IHxGoogleTagManager.PushPageView(LocationChangedEventArgs args)
		{
			if (args is null)
			{
				// App firstRender
				await PushPageView();
			}
			else
			{
				await PushPageViewCoreAsync(args.Location, new Dictionary<string, string>() { { "isNavigationIntercepted", args.IsNavigationIntercepted.ToString() } });
			}
		}

		private async Task PushPageViewCoreAsync(string url, object additionalData = null)
		{
			await InitializeAsync();
			await jsModule.InvokeVoidAsync("pushPageViewEvent", gtmOptions.PageViewEventName, gtmOptions.PageViewUrlVariableName, url, additionalData);
		}

		public async ValueTask DisposeAsync()
		{
			if (jsModule is not null)
			{
				await jsModule.DisposeAsync();
			}
		}
	}
}
