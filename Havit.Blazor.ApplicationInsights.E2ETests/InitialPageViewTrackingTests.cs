using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class InitialPageViewTrackingTests : BlazorApplicationInsightsPageTestBase
{
	[TestMethod]
	public async Task EnableInitialPageViewTracking_True_SendsInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: true);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.Contains(i => i.BaseType == "PageviewData", capturedTelemetryItems, "Expected at least one PageviewData item.");
	}

	[TestMethod]
	public async Task EnableInitialPageViewTracking_False_DoesNotSendInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: false);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.DoesNotContain(i => i.BaseType == "PageviewData", capturedTelemetryItems, "Expected no PageviewData items when EnableInitialPageViewTracking is false.");
	}

	private async Task ActAsync(BlazorWebApplicationFactory factory)
	{
		await Page.GotoAsync(factory.GetServerAddress() + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.CloseAsync();
	}

	private BlazorWebApplicationFactory GetFactoryForTest(bool enableInitialPageViewTracking)
	{
		var factory = new BlazorWebApplicationFactory(options => options.EnableInitialPageViewTracking = enableInitialPageViewTracking);
		factory.CreateClient();
		return factory;
	}
}
