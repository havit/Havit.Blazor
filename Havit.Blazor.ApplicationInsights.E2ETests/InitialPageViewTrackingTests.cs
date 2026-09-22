using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class InitialPageViewTrackingTests : BlazorApplicationInsightsPageTestBase
{
	private BlazorWebApplicationFactory _factory;

	[Fact]
	public async Task EnableInitialPageViewTracking_True_SendsInitialPageView()
	{
		// Arrange
		_factory = CreateFactory(enableInitialPageViewTracking: true);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(_factory.GetServerAddress() + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);

		// Assert
		await telemetry.WaitForItemAsync(i => i.BaseType == "PageviewData", "initial page view");
	}

	[Fact]
	public async Task EnableInitialPageViewTracking_False_DoesNotSendInitialPageView()
	{
		// Arrange
		_factory = CreateFactory(enableInitialPageViewTracking: false);
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(_factory.GetServerAddress() + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);

		// Assert
		// the page tracks the sentinel right before it flushes - once the sentinel is on the wire, a page view tracked before it would be there too
		// (the initial page view is issued by the child HxApplicationInsights from its OnInitializedAsync, the sentinel by the page from OnAfterRenderAsync;
		// both go through the FIFO gate, but the order in which they get enqueued is not guaranteed, so this barrier is not strictly tight)
		await telemetry.WaitForItemAsync(i => i.Data.BaseData.Name == TestDefaults.SentinelEvents.InitialPageViewTrackingPageDone, "sentinel event");
		Assert.DoesNotContain(telemetry.Items, i => i.BaseType == "PageviewData");
	}

	private static BlazorWebApplicationFactory CreateFactory(bool enableInitialPageViewTracking)
	{
		var factory = new BlazorWebApplicationFactory(options => options.EnableInitialPageViewTracking = enableInitialPageViewTracking);
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
