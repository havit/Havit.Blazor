using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class BlazorApplicationInsightsScriptTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task ApplicationInsightsScript_SSR_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsStriptTests.ServerSideRendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServer_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsStriptTests.InteractiveServer, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServerPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsStriptTests.InteractiveServerPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssembly_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsStriptTests.InteractiveWebAssembly, TestApp.Client.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssemblyPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsStriptTests.InteractiveWebAssemblyPrerendering, TestApp.Client.ConnectionStrings.ApplicationInsights);

	private async Task TestApplicationInsightsLoadedAndConfigured(string url, string expectedConnectionString)
	{
		await Page.GotoAsync(url);
		await Page.WaitForFunctionAsync("window.appInsights !== undefined",
			options: new PageWaitForFunctionOptions { Timeout = AppInsightsTimeout });
		Assert.IsTrue(await Page.EvaluateAsync<bool>($"window.appInsights.config.connectionString == '{expectedConnectionString}'"));
	}
}
