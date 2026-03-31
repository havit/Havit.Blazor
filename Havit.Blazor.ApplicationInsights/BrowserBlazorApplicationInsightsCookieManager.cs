using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Browser implementation of <see cref="IBlazorApplicationInsightsCookieManager"/>.
/// Delegates every call to the Application Insights JS SDK cookie manager via <see cref="IJSRuntime"/>.
/// </summary>
public class BrowserBlazorApplicationInsightsCookieManager : IBlazorApplicationInsightsCookieManager
{
	private readonly IJSRuntime _jsRuntime;

	/// <summary>
	/// Constructs a new instance of <see cref="BrowserBlazorApplicationInsightsCookieManager"/>.
	/// </summary>
	public BrowserBlazorApplicationInsightsCookieManager(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	/// <inheritdoc/>
	public async Task SetEnabledAsync(bool value)
		=> await _jsRuntime.InvokeVoidAsync("havitBlazorAppInsights.cookieMgr.setEnabled", value);

	/// <inheritdoc/>
	public async Task<bool> IsEnabledAsync()
		=> await _jsRuntime.InvokeAsync<bool>("havitBlazorAppInsights.cookieMgr.isEnabled");

	/// <inheritdoc/>
	public async Task<bool> SetCookieAsync(string name, string value, int? maxAgeSec = null, string domain = null, string path = null)
		=> await _jsRuntime.InvokeAsync<bool>("havitBlazorAppInsights.cookieMgr.set", name, value, maxAgeSec, domain, path);

	/// <inheritdoc/>
	public async Task<string> GetCookieAsync(string name)
		=> await _jsRuntime.InvokeAsync<string>("havitBlazorAppInsights.cookieMgr.get", name);

	/// <inheritdoc/>
	public async Task<bool> DeleteCookieAsync(string name, string path = null)
		=> await _jsRuntime.InvokeAsync<bool>("havitBlazorAppInsights.cookieMgr.del", name, path);

	/// <inheritdoc/>
	public async Task<bool> PurgeCookieAsync(string name, string path = null)
		=> await _jsRuntime.InvokeAsync<bool>("havitBlazorAppInsights.cookieMgr.purge", name, path);
}
