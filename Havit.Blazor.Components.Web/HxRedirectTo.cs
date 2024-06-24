namespace Havit.Blazor.Components.Web;

/// <summary>
/// Rendering a <c>HxRedirectTo</c> will navigate to a new location.<br/>
/// Can be used in <c>AuthorizeRouteView</c>, <c>Router</c> and similar components to redirect to a login page, error page, or similar.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxRedirectTo">https://havit.blazor.eu/components/HxRedirectTo</see>
/// </summary>
public class HxRedirectTo : ComponentBase
{
	/// <summary>
	/// URI to navigate to.
	/// </summary>
	[Parameter] public string Uri { get; set; }

	/// <summary>
	/// If <c>true</c>, bypasses client-side routing and forces the browser to load the new
	/// page from the server, regardless of whether the URI would normally be handled by the
	/// client-side router.<br/>
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool ForceLoad { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }

	protected override void OnInitialized()
	{
		NavigationManager.NavigateTo(Uri, ForceLoad);
	}
}
