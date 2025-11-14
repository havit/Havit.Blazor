namespace Havit.Blazor.E2ETests;

/// <summary>
/// Base class for E2E tests that provides Playwright context and helper methods.
/// </summary>
public abstract class TestAppTestBase : PageTest
{
	/// <summary>
	/// Gets the base URL of the running TestApp instance.
	/// </summary>
	protected static string BaseUrl => TestAppAssemblyInitializer.BaseUrl;

	/// <summary>
	/// Navigates to a relative path within the TestApp.
	/// </summary>
	/// <param name="relativePath">Relative path (e.g., "/counter" or "counter")</param>
	protected async Task NavigateToTestAppAsync(string relativePath)
	{
		if (!relativePath.StartsWith("/"))
		{
			relativePath = "/" + relativePath;
		}

		string fullUrl = BaseUrl + relativePath;
		await Page.GotoAsync(fullUrl);
	}
}
