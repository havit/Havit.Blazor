// Based on the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/AISKU/src/IApplicationInsights.ts

using System.ComponentModel;
using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Full Application Insights SDK interface, extending the core tracking interface with
/// dependency tracking, user context management, and telemetry flushing.
/// C# representation of the JavaScript <c>IApplicationInsights</c> interface
/// from <c>@microsoft/applicationinsights-web</c>, combining <c>IAppInsights</c>
/// and <c>IDependenciesPlugin</c>.
/// </summary>
/// <remarks>
/// Skipped members (JS-specific, not applicable in C#):
/// <list type="bullet">
///   <item><c>getCookieMgr()</c> – returns a browser-specific cookie manager instance</item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IApplicationInsights : IAppInsights, IDependenciesPlugin
{
	/// <summary>
	/// Sets the authenticated user context. Once set, Application Insights includes the user identity
	/// in all subsequent telemetry — including items tracked automatically by the JavaScript SDK
	/// (unhandled exceptions, page views, XHR requests, etc.).
	/// </summary>
	Task SetAuthenticatedUserContextAsync(string authenticatedUserId, string accountId = null, bool storeInCookie = false);

	/// <summary>Clears the authenticated user context previously set by <see cref="SetAuthenticatedUserContextAsync"/>.</summary>
	Task ClearAuthenticatedUserContextAsync();

	/// <summary>
	/// Flushes any buffered telemetry items, sending them immediately to Application Insights.
	/// Useful before navigating away or on application unload.
	/// </summary>
	Task FlushAsync();

	/// <summary>
	/// Registers a telemetry initializer that applies the given tags to every telemetry envelope
	/// sent by the Application Insights JavaScript SDK — including auto-collected telemetry
	/// (page views, XHR requests, unhandled exceptions).
	/// Call once after the SDK is initialized (e.g. in <c>OnAfterRenderAsync</c>).
	/// </summary>	
	Task AddTelemetryInitializerAsync(TelemetryInitializer initializer);
}
