using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

public static class JSRuntimeExtensions
{
	internal static ValueTask<IJSObjectReference> ImportHavitBlazorBootstrapModuleAsync(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		var path = "./_content/Havit.Blazor.Components.Web.Bootstrap/" + moduleNameWithoutExtension + ".js";
#if !NET9_0_OR_GREATER
		// pre-NET9 does not support StaticAssets with ImportMap
		path = path + "?v=" + HxSetup.VersionIdentifierHavitBlazorBootstrap;
#endif
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
}