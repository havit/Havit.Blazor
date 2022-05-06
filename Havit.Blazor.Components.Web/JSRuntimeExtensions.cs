using System.Reflection;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web;
public static class JSRuntimeExtensions
{
	public static ValueTask<IJSObjectReference> ImportModule(this IJSRuntime jsRuntime, string modulePath, Assembly assemblyForVersionInfo = null)
	{
		Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(modulePath));

		if (assemblyForVersionInfo is not null)
		{
			modulePath = modulePath + "?v=" + GetAssemblyVersionIdentifierForUri(assemblyForVersionInfo);
		}
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);
	}

	internal static ValueTask<IJSObjectReference> ImportHavitBlazorWebModule(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		versionIdentifierHavitBlazorWeb ??= GetAssemblyVersionIdentifierForUri(typeof(HxDynamicElement).Assembly);

		var path = "./_content/Havit.Blazor.Components.Web/" + moduleNameWithoutExtension + ".js?v=" + versionIdentifierHavitBlazorWeb;
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
	private static string versionIdentifierHavitBlazorWeb;

	internal static string GetAssemblyVersionIdentifierForUri(Assembly assembly)
	{
		return Uri.EscapeDataString(((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute), false)).InformationalVersion);
	}
}
