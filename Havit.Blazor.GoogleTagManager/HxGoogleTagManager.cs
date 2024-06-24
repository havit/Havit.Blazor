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
		if (_isInitialized)
		{
			return;
		}

		_jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.GoogleTagManager/" + nameof(HxGoogleTagManager) + ".js");

		await _jsModule.InvokeVoidAsync("initialize", _gtmOptions.GtmId);

		_isInitialized = true;
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
		await PushPageViewCoreAsync(_navigationManager.Uri, additionalData);
	}

	/// <inheritdoc/>
	async Task IHxGoogleTagManager.PushPageViewAsync(LocationChangedEventArgs args)
	{
		if (args is null)
		{
			// App firstRender
			await PushPageViewAsync();
		}
		else
		{
			await PushPageViewCoreAsync(args.Location, new Dictionary<string, string>() { { "isNavigationIntercepted", args.IsNavigationIntercepted.ToString() } });
		}
	}

	private async Task PushPageViewCoreAsync(string url, object additionalData = null)
	{
		await InitializeAsync();
		await _jsModule.InvokeVoidAsync("pushPageViewEvent", _gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData);
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
