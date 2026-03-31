using Havit.Blazor.ApplicationInsights.JsSdk;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Blazor Application Insights cookie manager service registered in the DI container.
/// Provides runtime access to the Application Insights SDK cookie manager (<c>getCookieMgr()</c>).
/// </summary>
/// <remarks>
/// Register via <c>AddBlazorApplicationInsights()</c> and inject <see cref="IBlazorApplicationInsightsCookieManager"/>
/// into components or services. The concrete implementation adapts automatically to the current
/// render context (SSR prerendering, Blazor Server, or WebAssembly).
/// </remarks>
public interface IBlazorApplicationInsightsCookieManager : ICookieMgr
{
}
