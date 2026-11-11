using System.Reflection;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web;

#pragma warning disable BL0016 // JS interop call is not guarded with a try/catch block: Thin wrapper returning the interop task; error handling belongs to the caller.

public static class JSRuntimeExtensions
{
	public static ValueTask<IJSObjectReference> ImportModuleAsync(this IJSRuntime jsRuntime, string modulePath, Assembly assemblyForVersionInfo = null)
	{
		Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(modulePath));

		return jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);
	}

	internal static ValueTask<IJSObjectReference> ImportHavitBlazorWebModuleAsync(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		var path = "./_content/Havit.Blazor.Components.Web/" + moduleNameWithoutExtension + ".js";

		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}

	internal static string GetAssemblyVersionIdentifierForUri(Assembly assembly)
	{
		return Uri.EscapeDataString(((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute), false)).InformationalVersion);
	}
}
