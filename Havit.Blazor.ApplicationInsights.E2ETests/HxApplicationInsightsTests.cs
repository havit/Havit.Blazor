using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class HxApplicationInsightsTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task ApplicationInsightsScript_SSR_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.HxApplicationInsightsTests.ServerSideRendering, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveServer_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.HxApplicationInsightsTests.InteractiveServer, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveServerPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.HxApplicationInsightsTests.InteractiveServerPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveWebAssembly_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.HxApplicationInsightsTests.InteractiveWebAssembly, TestApp.Client.ConnectionStrings.ApplicationInsights);

	[Fact]
	public async Task ApplicationInsightsScript_InteractiveWebAssemblyPrerendering_AppInsightsLoadedAndConfigured() => await TestApplicationInsightsLoadedAndConfigured(NavigationRoutes.HxApplicationInsightsTests.InteractiveWebAssemblyPrerendering, TestApp.ConnectionStrings.ApplicationInsights);

	private async Task TestApplicationInsightsLoadedAndConfigured(string url, string expectedConnectionString)
	{
		// Arrange
		await Page.RouteApplicationInsightsTrackAsync(); // keeps the telemetry from leaving the machine

		// Act
		await Page.GotoAsync(url);
		await Page.WaitForApplicationInsightsReadyAsync();
		string currentConnectionString = await Page.EvaluateAsync<string>("() => window.appInsights.config.connectionString");

		// Assert
		Assert.Equal(expectedConnectionString, currentConnectionString);
	}
}
