using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class TelemetryInitializerTests : BlazorApplicationInsightsPageTestBase
{
	[TestMethod]
	public async Task BlazorApplicationInsights_TelemetryInitializer_CloudRoleNameAppliedAfterRegistration()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(NavigationRoutes.TelemetryInitializerTests);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.CloseAsync();

		// Assert
		var beforeItem = capturedTelemetryItems.FirstOrDefault(i => i.Data.BaseData.Metrics?[0].Name == "before-initializer");
		var afterItem = capturedTelemetryItems.FirstOrDefault(i => i.Data.BaseData.Metrics?[0].Name == "after-initializer");

		Assert.IsNotNull(beforeItem, "before-initializer metric not found in captured telemetry.");
		Assert.IsNotNull(afterItem, "after-initializer metric not found in captured telemetry.");

		Assert.AreNotEqual("test-role", beforeItem.CloudRoleName, "before-initializer should not have ai.cloud.role 'test-role'.");
		Assert.AreEqual("test-role", afterItem.CloudRoleName, "after-initializer should have ai.cloud.role 'test-role'.");
	}
}
