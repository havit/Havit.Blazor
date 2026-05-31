using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class AutoRouteTrackingTests : BlazorApplicationInsightsPageTestBase
{
	[TestMethod]
	public async Task EnableAutoRouteTracking_True_SendsPageViewOnNavigation()
	{
		// Arrange
		await using var factory = GetFactoryForTest(true);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.Contains(i => i.BaseType == "PageviewData" && i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true, capturedTelemetryItems, "Expected a PageviewData item for Page 2 URL.");
	}

	[TestMethod]
	public async Task EnableAutoRouteTracking_False_DoesNotSendPageViewOnNavigation()
	{
		// Arrange
		await using var factory = GetFactoryForTest(false);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.DoesNotContain(i => i.BaseType == "PageviewData" && i.Data.BaseData.Url?.Contains(NavigationRoutes.PageViewTracking.AutoRouteTrackingPage2) == true, capturedTelemetryItems, "Expected no PageviewData for Page 2 URL when EnableAutoRouteTracking is false.");
	}

	private async Task ActAsync(BlazorWebApplicationFactory factory)
	{
		await Page.GotoAsync(factory.GetServerAddress() + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core"); // Wait for the JS SDK full initialization.
		await Page.ClickAsync("#goto-page2");
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Task.Delay(1000, TestContext.CancellationToken);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.CloseAsync();
	}

	private BlazorWebApplicationFactory GetFactoryForTest(bool enableAutoRouteTracking)
	{
		var factory = new BlazorWebApplicationFactory(options =>
		{
			options.JsSdkOptions.EnableAutoRouteTracking = enableAutoRouteTracking;
			options.EnableInitialPageViewTracking = false; // Avoid timing-sensitive false positive: initial trackPageView({}) from the snippet could be dequeued by the CDN SDK after navigation (using the new URL).
		});
		factory.CreateClient();
		return factory;
	}
}
