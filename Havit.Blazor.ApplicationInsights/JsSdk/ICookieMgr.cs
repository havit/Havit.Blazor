// Based on the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/JavaScriptSDK.Interfaces/ICookieMgr.ts

using System.ComponentModel;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Runtime cookie manager interface for reading, writing, and deleting cookies
/// managed by the Application Insights SDK.
/// C# representation of the JavaScript <c>ICookieMgr</c> interface
/// from <c>@microsoft/applicationinsights-core-js</c>.
/// </summary>
/// <remarks>
/// Skipped members (JS-specific, not applicable in C#):
/// <list type="bullet">
///   <item><c>unload()</c> – removes internal state during SDK unloading</item>
///   <item><c>update()</c> – allows updating the cookie manager's configuration</item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface ICookieMgr
{
	/// <summary>
	/// Enables or disables cookie usage.
	/// </summary>
	Task SetEnabledAsync(bool value);

	/// <summary>
	/// Returns whether the system can use cookies. If <c>false</c>, all cookie operations return nothing.
	/// </summary>
	Task<bool> IsEnabledAsync();

	/// <summary>
	/// Sets a named cookie with optional expiration, domain, and path parameters.
	/// </summary>
	Task<bool> SetCookieAsync(string name, string value, int? maxAgeSec = null, string domain = null, string path = null);

	/// <summary>
	/// Retrieves a cookie's value by name.
	/// </summary>
	Task<string> GetCookieAsync(string name);

	/// <summary>
	/// Deletes a named cookie. Respects the enabled setting.
	/// </summary>
	Task<bool> DeleteCookieAsync(string name, string path = null);

	/// <summary>
	/// Forces cookie removal regardless of the enabled setting.
	/// </summary>
	Task<bool> PurgeCookieAsync(string name, string path = null);
}
