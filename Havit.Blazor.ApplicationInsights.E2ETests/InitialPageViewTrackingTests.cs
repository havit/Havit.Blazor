using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class InitialPageViewTrackingTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task EnableInitialPageViewTracking_True_SendsInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: true);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.Contains(capturedTelemetryItems, i => i.BaseType == "PageviewData");
	}

	[Fact]
	public async Task EnableInitialPageViewTracking_False_DoesNotSendInitialPageView()
	{
		// Arrange
		await using var factory = GetFactoryForTest(enableInitialPageViewTracking: false);
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync(factory);

		// Assert
		Assert.DoesNotContain(capturedTelemetryItems, i => i.BaseType == "PageviewData");
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
