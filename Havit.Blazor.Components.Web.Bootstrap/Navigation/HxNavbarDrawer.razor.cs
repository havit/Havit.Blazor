using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Container for the responsive content of the <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/navbar/">navbar</see>.
/// Bootstrap 6 replaced the v5 <c>.navbar-collapse</c> (Collapse plugin) with a <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Drawer</see>:
/// below the navbar's expand breakpoint the content opens as a drawer (toggled by <see cref="HxNavbarToggler"/> via the Bootstrap data API),
/// at or above the breakpoint the drawer markup is flattened into inline navbar content by the Bootstrap CSS.
/// </summary>
public partial class HxNavbarDrawer : ComponentBase, IAsyncDisposable
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// The drawer element ID. The default value is derived from the parent navbar (matches the default <see cref="HxNavbarToggler"/> target).
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Title rendered in the drawer header (visible only below the navbar's expand breakpoint).
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Placement of the drawer when opened below the expand breakpoint. The default is <see cref="DrawerPlacement.End"/>.
	/// </summary>
	[Parameter] public DrawerPlacement Placement { get; set; } = DrawerPlacement.End;

	/// <summary>
	/// Additional CSS class(es) for the drawer element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the drawer element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected string IdEffective => Id ?? NavbarContainer?.GetDefaultDrawerId();

	private ElementReference _drawerElement;
	private IJSObjectReference _jsModule;
	private bool _navigationSubscribed;

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarDrawer)} requires the parent {nameof(HxNavbar)}.");
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			// Close the drawer upon in-app navigation - the modal drawer would otherwise stay open
			// over the (inert) navigated page.
			NavigationManager.LocationChanged += HandleLocationChanged;
			_navigationSubscribed = true;
		}
	}

	private void HandleLocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
	{
		_ = InvokeAsync(CloseAsync);
	}

	private async Task CloseAsync()
	{
		try
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDrawer));
			await _jsModule.InvokeVoidAsync("hide", _drawerElement); // no-op when the drawer is not open
		}
		catch (JSDisconnectedException)
		{
			// NOOP
		}
		catch (TaskCanceledException)
		{
			// NOOP
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (_navigationSubscribed)
		{
			NavigationManager.LocationChanged -= HandleLocationChanged;
		}

		if (_jsModule is not null)
		{
			try
			{
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}
	}

	private string GetPlacementCssClass()
	{
		return Placement switch
		{
			DrawerPlacement.Start => "drawer-start",
			DrawerPlacement.End => "drawer-end",
			DrawerPlacement.Top => "drawer-top",
			DrawerPlacement.Bottom => "drawer-bottom",
			_ => throw new InvalidOperationException($"Unknown {nameof(DrawerPlacement)} value {Placement}.")
		};
	}
}
