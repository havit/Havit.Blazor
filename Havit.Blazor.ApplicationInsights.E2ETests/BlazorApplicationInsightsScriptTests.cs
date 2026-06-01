using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class BlazorApplicationInsightsScriptTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task ApplicationInsightsScript_SSR_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.ServerSideRendering, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveServer_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveServer, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveServerPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveServerPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveWebAssembly_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.BlazorApplicationInsightsScriptTests.InteractiveWebAssembly, TestApp.Client.ConnectionStrings.ApplicationInsights);

	[Fact]
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
		Assert.Equal(expectedConnectionString, currentConnectionString);
	}
}
