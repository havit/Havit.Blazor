using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Havit.Blazor.GoogleTagManager;

///<summary>
/// Adds Google Tag Manager to the application and manages communication with GTM JavaScript (data-layer).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxGoogleTagManager">https://havit.blazor.eu/components/HxGoogleTagManager</see>
/// </summary>
/// <inheritdoc/>
public class HxGoogleTagManager : IHxGoogleTagManager, IAsyncDisposable
{
	private readonly HxGoogleTagManagerOptions _gtmOptions;
	private readonly NavigationManager _navigationManager;
	private readonly IJSRuntime _jsRuntime;

	private bool _isInitialized;
	private IJSObjectReference _jsModule;

	public HxGoogleTagManager(
		IOptions<HxGoogleTagManagerOptions> gtmOptions,
		NavigationManager navigationManager,
		IJSRuntime jsRuntime)
	{
		_gtmOptions = gtmOptions.Value;
		_navigationManager = navigationManager;
		_jsRuntime = jsRuntime;
	}

	/// <inheritdoc/>
	public async Task InitializeAsync()
	{
		_jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.GoogleTagManager/" + nameof(HxGoogleTagManager) + ".js");

		if (_isInitialized)
		{
			return;
		}
		_isInitialized = true;

		await _jsModule.InvokeVoidAsync("initialize", _gtmOptions.GtmId);
	}

	/// <inheritdoc/>
	public async Task PushAsync(object data)
	{
		await InitializeAsync();
		await _jsModule.InvokeVoidAsync("push", data);
	}

	/// <inheritdoc/>
	public async Task PushEventAsync(string eventName, object eventData = null)
	{
		await InitializeAsync();
		await _jsModule.InvokeVoidAsync("pushEvent", eventName, eventData);
	}

	/// <inheritdoc/>
	public async Task PushPageViewAsync(object additionalData = null)
	{
		// An explicit call is always pushed - the caller asked for it.
		await PushPageViewCoreAsync(_navigationManager.Uri, additionalData, deduplicate: false);
	}

	/// <inheritdoc/>
	async Task IHxGoogleTagManager.PushPageViewAsync(LocationChangedEventArgs args)
	{
		// Automatic tracking. The very same navigation can also be seen by the JS initializer
		// (enhancedload) in a Blazor Web App that mixes static SSR and interactive pages,
		// so let JavaScript drop the repeated URL.
		if (args is null)
		{
			// App firstRender
			await PushPageViewCoreAsync(_navigationManager.Uri, additionalData: null, deduplicate: true);
		}
		else
		{
			await PushPageViewCoreAsync(args.Location, new Dictionary<string, string>() { { "isNavigationIntercepted", args.IsNavigationIntercepted.ToString() } }, deduplicate: true);
		}
	}

	private async Task PushPageViewCoreAsync(string url, object additionalData, bool deduplicate)
	{
		await InitializeAsync();

		if (deduplicate)
		{
			await _jsModule.InvokeVoidAsync("pushPageViewEventOnce", _gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData, _gtmOptions.EnableInitialPageViewTracking);
		}
		else
		{
			await _jsModule.InvokeVoidAsync("pushPageViewEvent", _gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData);
		}
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (_jsModule is not null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}
