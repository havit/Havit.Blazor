// Based on the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/JavaScriptSDK.Interfaces/ICookieMgr.ts

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Cookie manager configuration.
/// C# representation of the JavaScript <c>ICookieMgrConfig</c> interface
/// from <c>@microsoft/applicationinsights-core-js</c>.
/// </summary>
/// <remarks>
/// Skipped properties (callback functions, not serializable to JSON):
/// <list type="bullet">
///   <item><c>getCookie</c> – <c>(name: string) =&gt; string</c> (custom cookie fetch function)</item>
///   <item><c>setCookie</c> – <c>(name: string, value: string) =&gt; void</c> (custom cookie set function)</item>
///   <item><c>delCookie</c> – <c>(name: string, value: string) =&gt; void</c> (custom cookie delete function)</item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class CookieMgrConfig
{
	/// <summary>
	/// Indicates whether the SDK uses cookies. If <c>false</c>, the instance
	/// won't store or read any data from cookies.
	/// </summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; set; }

	/// <summary>
	/// Custom cookie domain for sharing cookies across subdomains.
	/// Takes precedence over the root <see cref="ApplicationInsightsConfiguration.CookieDomain"/> value.
	/// </summary>
	[JsonPropertyName("domain")]
	public string Domain { get; set; }

	/// <summary>
	/// Specifies the cookie path.
	/// Takes precedence over the root <see cref="ApplicationInsightsConfiguration.CookiePath"/> value.
	/// </summary>
	[JsonPropertyName("path")]
	public string Path { get; set; }

	/// <summary>
	/// Array of cookie names to never read or write. They can still be explicitly purged/deleted.
	/// </summary>
	[JsonPropertyName("ignoreCookies")]
	public string[] IgnoreCookies { get; set; }

	/// <summary>
	/// Array of cookie names to never create or update. Defaults to <see cref="IgnoreCookies"/> when not provided.
	/// Cookies listed here can still be read unless also present in <see cref="IgnoreCookies"/>.
	/// </summary>
	[JsonPropertyName("blockedCookies")]
	public string[] BlockedCookies { get; set; }
}
