using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Temporarily (?) compensates for a Blazor deficiency in anchor fragments (scrolling to <c>page#id</c> URLs). Supports navigation with the <see cref="HxScrollspy"/> component.
/// <see href="https://github.com/dotnet/aspnetcore/issues/8393">GitHub Issue: Blazor 0.8.0: hash routing to named element #8393</see>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAnchorFragmentNavigation">https://havit.blazor.eu/components/HxAnchorFragmentNavigation</see>
/// </summary>
public class HxAnchorFragmentNavigation : ComponentBase, IAsyncDisposable
{
	/// <summary>
	/// The level of automation.
	/// The default is <see cref="AnchorFragmentNavigationAutomationMode.Full"/>.
	/// </summary>
	[Parameter] public AnchorFragmentNavigationAutomationMode Automation { get; set; } = AnchorFragmentNavigationAutomationMode.Full;

	[Inject] protected NavigationManager NavigationManager { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private string _lastKnownLocation;
	private bool _registerForScrollToCurrentUriFragmentAsyncOnAfterRender;

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_lastKnownLocation = NavigationManager.Uri;
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
			|| _registerForScrollToCurrentUriFragmentAsyncOnAfterRender)
		{
			_registerForScrollToCurrentUriFragmentAsyncOnAfterRender = false;
			await ScrollToCurrentUriFragmentAsync();
		}

		await base.OnAfterRenderAsync(firstRender);
	}

	public async Task ScrollToAnchorAsync(string anchor)
	{
		if (!String.IsNullOrEmpty(anchor))
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAnchorFragmentNavigation));
			await _jsModule.InvokeVoidAsync("scrollToAnchor", anchor);
		}
	}

	private void OnLocationChanged(object sender, LocationChangedEventArgs args)
	{
		if ((Automation == AnchorFragmentNavigationAutomationMode.Full)
			|| ((Automation == AnchorFragmentNavigationAutomationMode.SamePage)
						&& (NavigationManager.ToAbsoluteUri(_lastKnownLocation).PathAndQuery == NavigationManager.ToAbsoluteUri(args.Location).PathAndQuery)))
		{
			_ = InvokeAsync(() =>
			{
				_registerForScrollToCurrentUriFragmentAsyncOnAfterRender = true;
				StateHasChanged();
			});
		}
		_lastKnownLocation = args.Location;
	}


	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		NavigationManager.LocationChanged -= OnLocationChanged;

		if (_jsModule is not null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}
