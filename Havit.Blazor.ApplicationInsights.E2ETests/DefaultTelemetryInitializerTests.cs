using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.Options;
using Havit.Blazor.ApplicationInsights.Telemetry;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class DefaultTelemetryInitializerTests : BlazorApplicationInsightsPageTestBase
{
	// TODO: Check next render modes

	[TestMethod]
	public async Task DefaultTelemetryInitializer_CloudRoleName_AppliedToInitialPageView()
	{
		// Arrange
		const string expectedRoleName = "test-default-role";
		using var factory = new BlazorWebApplicationFactory(options =>
			options.DefaultTelemetryInitializer = new TelemetryInitializer { CloudRoleName = expectedRoleName });
		factory.CreateClient();

		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(factory.ServerAddress + NavigationRoutes.DefaultTelemetryInitializerTest);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

		// Assert
		var pageViewItem = capturedTelemetryItems.FirstOrDefault(i => i.BaseType == "PageviewData");
		Assert.IsNotNull(pageViewItem, "Expected initial PageviewData item not found in captured telemetry.");
		Assert.AreEqual(expectedRoleName, pageViewItem.CloudRoleName, "Initial page view should have CloudRoleName from DefaultTelemetryInitializer.");
	}
}
