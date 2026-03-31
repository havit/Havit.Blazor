namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// No-op implementation of <see cref="IBlazorApplicationInsightsCookieManager"/>.
/// All calls are silently ignored. Useful for server-side rendering or environments
/// where Application Insights tracking is not desired.
/// </summary>
internal class NullBlazorApplicationInsightsCookieManager : IBlazorApplicationInsightsCookieManager
{
	public Task SetEnabledAsync(bool value)
		=> Task.CompletedTask;

	public Task<bool> IsEnabledAsync()
		=> Task.FromResult(false);

	public Task<bool> SetCookieAsync(string name, string value, int? maxAgeSec = null, string domain = null, string path = null)
		=> Task.FromResult(false);

	public Task<string> GetCookieAsync(string name)
		=> Task.FromResult<string>(null);

	public Task<bool> DeleteCookieAsync(string name, string path = null)
		=> Task.FromResult(false);

	public Task<bool> PurgeCookieAsync(string name, string path = null)
		=> Task.FromResult(false);
}
