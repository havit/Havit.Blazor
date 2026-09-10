using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.ApplicationInsights.TestApp.Components;

/// <summary>
/// Maps the <c>{mode}</c> route segment used across test pages to the corresponding Blazor render mode.
/// </summary>
internal static class RenderModeRouteHelper
{
	public static IComponentRenderMode GetRenderMode(string mode) => mode switch
	{
		"ssr" => null,
		"interactive-server" => ExtendedRenderMode.InteractiveServerWithoutPrerendering,
		"interactive-server-prerendering" => RenderMode.InteractiveServer,
		"interactive-wasm" => ExtendedRenderMode.InteractiveWebAssemblyWithoutPrerendering,
		"interactive-wasm-prerendering" => RenderMode.InteractiveWebAssembly,
		_ => throw new ArgumentException($"Unknown render mode route segment: '{mode}'.", nameof(mode))
	};
}
