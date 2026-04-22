using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class BlazorApplicationInsightsScriptTests : BlazorApplicationInsightsPageTestBase
{
	[TestMethod]
	public async Task ApplicationInsightsScript_SSR_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.ServerSideRendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServer_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveServer, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveServerPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveServerPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssembly_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveWebAssembly, TestApp.Client.ConnectionStrings.ApplicationInsights);

	[TestMethod]
	public async Task ApplicationInsightsScript_InteractiveWebAssemblyPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveWebAssemblyPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	private async Task TestApplicationInsightsLoadedAndConfigured(string url, string expectedConnectionString)
	{
		// Arrange
		await Page.RouteApplicationInsightsTrackAsync(null);

		// Act
		await Page.GotoAsync(url);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core"); // Wait for the JS SDK full initialization.
		string currentConnectionString = await Page.EvaluateAsync<string>($"window.appInsights.config.connectionString");
		await Page.CloseAsync();

		// Assert
		Assert.AreEqual(expectedConnectionString, currentConnectionString);
	}
}
