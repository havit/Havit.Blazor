using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class InitialPageViewTrackingTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task EnableInitialPageViewTracking_True_SendsInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: true);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(factory.GetServerAddress() + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);

		// Assert
		await telemetry.WaitForItemAsync(i => i.BaseType == "PageviewData", "initial page view");

		await ClosePageBeforeHostShutdownAsync();
	}

	[Fact]
	public async Task EnableInitialPageViewTracking_False_DoesNotSendInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: false);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(factory.GetServerAddress() + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);

		// Assert
		// the page tracks the sentinel right before it flushes - once the sentinel is on the wire, a page view tracked before it would be there too
		await telemetry.WaitForItemAsync(i => i.Data.BaseData.Name == TestDefaults.SentinelEvents.InitialPageViewTrackingPageDone, "sentinel event");
		Assert.DoesNotContain(telemetry.Items, i => i.BaseType == "PageviewData");

		await ClosePageBeforeHostShutdownAsync();
	}

	/// <summary>
	/// The test owns the host (<c>await using var factory</c>), which gets disposed before the base class closes the page.
	/// The page has to go first, otherwise the Blazor Server circuit loses its WebSocket and logs a console error.
	/// </summary>
	private Task ClosePageBeforeHostShutdownAsync() => Page.CloseAsync();

	private BlazorWebApplicationFactory GetFactoryForTest(bool enableInitialPageViewTracking)
	{
		var factory = new BlazorWebApplicationFactory(options => options.EnableInitialPageViewTracking = enableInitialPageViewTracking);
		factory.CreateClient();
		return factory;
	}
}
