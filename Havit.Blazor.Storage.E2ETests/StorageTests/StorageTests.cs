using System.Text.Json;

namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// Base class for the browser storage E2E tests.
/// It contains the shared infrastructure (navigation, locators, and direct browser storage access) used by the test scenarios.
/// </summary>
public abstract class StorageTests : TestAppTestBase
{
	/// <summary>
	/// The storage area route segment used by the test page (<c>"local"</c> or <c>"session"</c>).
	/// </summary>
	protected abstract string StorageArea { get; }

	/// <summary>
	/// The JS <c>window</c> property name of the storage area under test (<c>"localStorage"</c> or <c>"sessionStorage"</c>).
	/// </summary>
	protected abstract string JsStorageName { get; }

	/// <summary>
	/// The route template of the test page (without the storage area segment). The storage area is appended as a route parameter.
	/// </summary>
	protected abstract string PageRouteTemplate { get; }

	/// <summary>
	/// Waits until the test page is ready to be interacted with. The default implementation does nothing; the
	/// WebAssembly variant overrides it to wait for the component to become interactive.
	/// </summary>
	protected virtual Task WaitForPageReadyAsync() => Task.CompletedTask;

	protected ILocator KeyInput => Page.Locator("[data-testid='key-input']");
	protected ILocator ValueInput => Page.Locator("[data-testid='value-input']");
	protected ILocator IndexInput => Page.Locator("[data-testid='index-input']");
	protected ILocator Result => Page.Locator("[data-testid='result']");
	protected ILocator LengthResult => Page.Locator("[data-testid='length-result']");

	protected async Task NavigateAsync()
	{
		await NavigateToTestAppAsync($"{PageRouteTemplate}/{StorageArea}");

		await WaitForPageReadyAsync();

		// start from a clean state in both storage areas
		await Page.EvaluateAsync("() => { window.localStorage.clear(); window.sessionStorage.clear(); }");
	}

	protected Task<JsonElement?> SetItemDirectlyAsync(string key, string value)
		=> Page.EvaluateAsync("args => window[args[0]].setItem(args[1], args[2])", new[] { JsStorageName, key, value });

	protected Task<string> ReadItemDirectlyAsync(string key)
		=> Page.EvaluateAsync<string>("args => window[args[0]].getItem(args[1])", new[] { JsStorageName, key });

	protected Task<int> GetLengthDirectlyAsync()
		=> Page.EvaluateAsync<int>("name => window[name].length", JsStorageName);
}
