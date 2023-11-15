using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Temporarily (?) compensates Blazor deficiency in anchor-fragments (scrolling to <c>page#id</c> URLs). Supports navigation with <see cref="HxScrollspy"/> component.
/// <see href="https://github.com/dotnet/aspnetcore/issues/8393">GitHub Issue: Blazor 0.8.0: hash routing to named element #8393</see>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAnchorFragmentNavigation">https://havit.blazor.eu/components/HxAnchorFragmentNavigation</see>
/// </summary>

#if NET8_0_OR_GREATER
[Obsolete("HxAnchorFragmentNavigation is no longer needed. ASP.NET Core Blazor 8 resolves the issue with anchor fragments.")]
#endif
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
			jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAnchorFragmentNavigation));
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


	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		NavigationManager.LocationChanged -= OnLocationChanged;

		if (jsModule is not null)
		{
			await jsModule.DisposeAsync();
		}
	}
}
