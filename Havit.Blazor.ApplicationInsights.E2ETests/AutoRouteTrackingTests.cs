using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class AutoRouteTrackingTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task EnableAutoRouteTracking_True_SendsPageViewOnNavigation()
	{
		// Arrange
		await using var factory = GetFactoryForTest(true);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await ActAsync(factory);

		// Assert
		await telemetry.WaitForItemAsync(
			i => (i.BaseType == "PageviewData") && (i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true),
			"page view for page 2");

		await ClosePageBeforeHostShutdownAsync();
	}

	[Fact]
	public async Task EnableAutoRouteTracking_False_DoesNotSendPageViewOnNavigation()
	{
		// Arrange
		await using var factory = GetFactoryForTest(false);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await ActAsync(factory);

		// Assert
		// page 2 tracks the sentinel right before it flushes - an auto-tracked page view is issued by the SDK on the navigation itself,
		// so once the sentinel is on the wire, that page view would be there too
		await telemetry.WaitForItemAsync(i => i.Data.BaseData.Name == TestDefaults.SentinelEvents.AutoRouteTrackingPage2Done, "sentinel event");
		Assert.DoesNotContain(telemetry.Items, i => (i.BaseType == "PageviewData") && (i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true));

		await ClosePageBeforeHostShutdownAsync();
	}

	/// <summary>
	/// The test owns the host (<c>await using var factory</c>), which gets disposed before the base class closes the page.
	/// The page has to go first, otherwise the Blazor Server circuit loses its WebSocket and logs a console error.
	/// </summary>
	private Task ClosePageBeforeHostShutdownAsync() => Page.CloseAsync();

	private async Task ActAsync(BlazorWebApplicationFactory factory)
	{
		await Page.GotoAsync(factory.GetServerAddress() + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		// the SDK hooks route tracking during its initialization, navigating earlier would not be observed
		await Page.WaitForApplicationInsightsReadyAsync();
		await Page.ClickAsync("#goto-page2");
	}

	private BlazorWebApplicationFactory GetFactoryForTest(bool enableAutoRouteTracking)
	{
		var factory = new BlazorWebApplicationFactory(options =>
		{
			options.JsSdkOptions.EnableAutoRouteTracking = enableAutoRouteTracking;
			options.EnableInitialPageViewTracking = false; // keep the initial page view out of the picture, the tests are about the navigation only
		});
		factory.CreateClient();
		return factory;
	}
}
