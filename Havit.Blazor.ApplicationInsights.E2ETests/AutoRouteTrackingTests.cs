using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class AutoRouteTrackingTests : BlazorApplicationInsightsPageTestBase
{
	private BlazorWebApplicationFactory _factory;

	[Fact]
	public async Task EnableAutoRouteTracking_True_SendsPageViewOnNavigation()
	{
		// Arrange
		_factory = CreateFactory(enableAutoRouteTracking: true);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await ActAsync();

		// Assert
		await telemetry.WaitForItemAsync(
			i => (i.BaseType == "PageviewData") && (i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true),
			"page view for page 2");
	}

	[Fact]
	public async Task EnableAutoRouteTracking_False_DoesNotSendPageViewOnNavigation()
	{
		// Arrange
		_factory = CreateFactory(enableAutoRouteTracking: false);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await ActAsync();

		// Assert
		// page 2 tracks the sentinel right before it flushes - an auto-tracked page view is issued by the SDK on the navigation itself,
		// so once the sentinel is on the wire, that page view would be there too
		await telemetry.WaitForItemAsync(i => i.Data.BaseData.Name == TestDefaults.SentinelEvents.AutoRouteTrackingPage2Done, "sentinel event");
		Assert.DoesNotContain(telemetry.Items, i => (i.BaseType == "PageviewData") && (i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true));
	}

	private async Task ActAsync()
	{
		await Page.GotoAsync(_factory.GetServerAddress() + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		// the SDK hooks route tracking during its initialization, navigating earlier would not be observed
		await Page.WaitForApplicationInsightsReadyAsync();
		await Page.ClickAsync("#goto-page2");
	}

	private static BlazorWebApplicationFactory CreateFactory(bool enableAutoRouteTracking)
	{
		var factory = new BlazorWebApplicationFactory(options =>
		{
			options.JsSdkOptions.EnableAutoRouteTracking = enableAutoRouteTracking;
			options.EnableInitialPageViewTracking = false; // keep the initial page view out of the picture, the tests are about the navigation only
		});
		factory.CreateClient();
		return factory;
	}

	/// <summary>
	/// The tests own their host, and it has to outlive the page: the base teardown closes the page first, otherwise the Blazor Server
	/// circuit loses its WebSocket and logs a console error. Done in the teardown rather than at the end of each test so that the ordering
	/// also holds when the test fails half-way.
	/// </summary>
	public override async ValueTask DisposeAsync()
	{
		try
		{
			await base.DisposeAsync();
		}
		finally
		{
			if (_factory != null)
			{
				await _factory.DisposeAsync();
			}
		}
	}
}
