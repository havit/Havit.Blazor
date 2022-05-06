using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;
public static class JSRuntimeExtensions
{
	internal static ValueTask<IJSObjectReference> ImportHavitBlazorBootstrapModule(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		versionIdentifierHavitBlazorBootstrap ??= Havit.Blazor.Components.Web.JSRuntimeExtensions.GetAssemblyVersionIdentifierForUri(typeof(ThemeColor).Assembly);

		var path = "./_content/Havit.Blazor.Components.Web.Bootstrap/" + moduleNameWithoutExtension + ".js?v=" + versionIdentifierHavitBlazorBootstrap;
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
	private static string versionIdentifierHavitBlazorBootstrap;
}