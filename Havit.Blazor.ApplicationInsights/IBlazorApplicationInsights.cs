using Havit.Blazor.ApplicationInsights.JsSdk;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Blazor Application Insights service registered in the DI container.
/// Combines the full <see cref="IApplicationInsights"/> API in a single injectable interface.
/// </summary>
/// <remarks>
/// Register via <c>AddBlazorApplicationInsights()</c> and inject <see cref="IBlazorApplicationInsights"/>
/// into components or services. The concrete implementation adapts automatically to the current
/// render context (SSR prerendering, Blazor Server, or WebAssembly).
/// </remarks>
public interface IBlazorApplicationInsights : IApplicationInsights
{
}
