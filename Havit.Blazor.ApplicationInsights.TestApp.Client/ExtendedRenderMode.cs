using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

public class ExtendedRenderMode
{
	public static InteractiveServerRenderMode InteractiveServerWithoutPrerendering { get; } = new InteractiveServerRenderMode(prerender: false);

	public static InteractiveWebAssemblyRenderMode InteractiveWebAssemblyWithoutPrerendering { get; } = new InteractiveWebAssemblyRenderMode(prerender: false);
}
