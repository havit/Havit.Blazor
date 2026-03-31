namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Server-side implementation of <see cref="IBlazorApplicationInsightsCookieManager"/> that adapts to the current
/// render mode. During prerendering (where JS is unavailable), all calls are silently ignored.
/// Once the interactive circuit is established and JS becomes available, calls are forwarded
/// to <see cref="BrowserBlazorApplicationInsightsCookieManager"/>.
/// </summary>
/// <remarks>
/// Use this implementation on the server side (registered automatically by
/// <c>AddBlazorApplicationInsights()</c> when not running in a browser/WASM context).
/// </remarks>
internal class AdaptiveBlazorApplicationInsightsCookieManager : IBlazorApplicationInsightsCookieManager
{
	private readonly BrowserBlazorApplicationInsightsCookieManager _browserCookieMgr;
	private bool? _isJsAvailable; // null = not yet tried, false = prerender/SSR, true = JS works

	public AdaptiveBlazorApplicationInsightsCookieManager(BrowserBlazorApplicationInsightsCookieManager browserCookieMgr)
	{
		_browserCookieMgr = browserCookieMgr;
	}

	private async Task InvokeJsSafeAsync(Func<Task> browserInvocation)
	{
		if (_isJsAvailable == false)
		{
			return;
		}

		try
		{
			await browserInvocation();
			_isJsAvailable = true;
		}
		catch (InvalidOperationException) when (_isJsAvailable is null)
		{
			_isJsAvailable = false;
		}
	}

	private async Task<T> InvokeJsSafeAsync<T>(Func<Task<T>> browserInvocation)
	{
		if (_isJsAvailable == false)
		{
			return default;
		}

		try
		{
			var result = await browserInvocation();
			_isJsAvailable = true;
			return result;
		}
		catch (InvalidOperationException) when (_isJsAvailable is null)
		{
			_isJsAvailable = false;
			return default;
		}
	}

	/// <inheritdoc/>
	public async Task SetEnabledAsync(bool value)
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.SetEnabledAsync(value));

	/// <inheritdoc/>
	public async Task<bool> IsEnabledAsync()
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.IsEnabledAsync());

	/// <inheritdoc/>
	public async Task<bool> SetCookieAsync(string name, string value, int? maxAgeSec = null, string domain = null, string path = null)
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.SetCookieAsync(name, value, maxAgeSec, domain, path));

	/// <inheritdoc/>
	public async Task<string> GetCookieAsync(string name)
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.GetCookieAsync(name));

	/// <inheritdoc/>
	public async Task<bool> DeleteCookieAsync(string name, string path = null)
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.DeleteCookieAsync(name, path));

	/// <inheritdoc/>
	public async Task<bool> PurgeCookieAsync(string name, string path = null)
		=> await InvokeJsSafeAsync(() => _browserCookieMgr.PurgeCookieAsync(name, path));
}
