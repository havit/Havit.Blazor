using System.Reflection;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web;
public static class JSRuntimeExtensions
{
	public static ValueTask<IJSObjectReference> ImportHavitBlazorWebModule(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		var path = "./_content/Havit.Blazor.Components.Web/" + moduleNameWithoutExtension + ".js?v=" + GetVersionIdentifier();
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}

	private static string GetVersionIdentifier()
	{
		versionIdentifier ??= Uri.EscapeDataString(((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(typeof(HxDynamicElement).Assembly, typeof(AssemblyInformationalVersionAttribute), false)).InformationalVersion);
		return versionIdentifier;
	}
	private static string versionIdentifier;
}
