using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class ApplicationInsightsScriptTests : PageTest
{
	private const float AppInsightsTimeout = 2_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task ApplicationInsightsScript_SSR_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.AppInsightsTestServerSideRendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServer_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.AppInsightsTestInteractiveServer, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServerPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.AppInsightsTestInteractiveServerPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssembly_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.AppInsightsTestInteractiveWebAssembly, TestApp.Client.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssemblyPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.AppInsightsTestInteractiveWebAssemblyPrerendering, TestApp.Client.ConnectionStrings.ApplicationInsights);

	private async Task TestApplicationInsightsLoadedAndConfigured(string url, string expectedConnectionString)
	{
		await Page.GotoAsync(url);
		await Page.WaitForFunctionAsync("window.appInsights !== undefined",
			options: new PageWaitForFunctionOptions { Timeout = AppInsightsTimeout });
		Assert.IsTrue(await Page.EvaluateAsync<bool>($"window.appInsights.config.connectionString == '{expectedConnectionString}'"));
	}
}
