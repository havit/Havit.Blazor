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
		using var factory = GetFactoryForTest(true);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(factory.ServerAddress + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core"); // Wait for the JS SDK full initialization.
		await Page.ClickAsync("#goto-page2");
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);

		// Assert
		Assert.Contains(i => i.BaseType == "PageviewData" && i.Data.BaseData.Url?.Contains("auto-route-tracking-2") == true, capturedTelemetryItems, "Expected a PageviewData item for Page 2 URL.");
	}

	[TestMethod]
	public async Task EnableAutoRouteTracking_False_DoesNotSendPageViewOnNavigation()
	{
		// Arrange
		using var factory = GetFactoryForTest(false);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(factory.ServerAddress + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core"); // Wait for the JS SDK full initialization.
		await Page.ClickAsync("#goto-page2");
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);

		// Assert
		Assert.DoesNotContain(i => i.BaseType == "PageviewData" && i.Data.BaseData.Url?.Contains("auto-route-tracking-2") == true, capturedTelemetryItems, "Expected no PageviewData for Page 2 URL when EnableAutoRouteTracking is false.");
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
