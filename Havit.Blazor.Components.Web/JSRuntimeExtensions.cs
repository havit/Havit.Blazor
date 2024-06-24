using System.Reflection;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web;
public static class JSRuntimeExtensions
{
	public static ValueTask<IJSObjectReference> ImportModuleAsync(this IJSRuntime jsRuntime, string modulePath, Assembly assemblyForVersionInfo = null)
	{
		Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(modulePath));

		if (assemblyForVersionInfo is not null)
		{
			modulePath = modulePath + "?v=" + GetAssemblyVersionIdentifierForUri(assemblyForVersionInfo);
		}
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);
	}

	internal static ValueTask<IJSObjectReference> ImportHavitBlazorWebModuleAsync(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		s_versionIdentifierHavitBlazorWeb ??= GetAssemblyVersionIdentifierForUri(typeof(HxDynamicElement).Assembly);

		var path = "./_content/Havit.Blazor.Components.Web/" + moduleNameWithoutExtension + ".js?v=" + s_versionIdentifierHavitBlazorWeb;
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
	private static string s_versionIdentifierHavitBlazorWeb;

	internal static string GetAssemblyVersionIdentifierForUri(Assembly assembly)
	{
		return Uri.EscapeDataString(((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute), false)).InformationalVersion);
	}
}
