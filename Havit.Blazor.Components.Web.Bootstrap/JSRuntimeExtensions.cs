using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

#pragma warning disable BL0016 // JS interop call is not guarded with a try/catch block: Thin wrapper returning the interop task; error handling belongs to the caller.

public static class JSRuntimeExtensions
{
	internal static ValueTask<IJSObjectReference> ImportHavitBlazorBootstrapModuleAsync(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		var path = "./_content/Havit.Blazor.Components.Web.Bootstrap/" + moduleNameWithoutExtension + ".js";
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
}