using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class TelemetryInitializerTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
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

		Assert.NotNull(beforeItem);
		Assert.NotNull(afterItem);

		Assert.NotEqual("test-role", beforeItem.CloudRoleName);
		Assert.Equal("test-role", afterItem.CloudRoleName);
	}
}
