using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class TelemetryInitializerTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task BlazorApplicationInsights_TelemetryInitializer_CloudRoleNameAppliedAfterRegistration()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.TelemetryInitializerTests);

		// Assert
		var beforeItem = await telemetry.WaitForItemAsync(i => i.Data.BaseData.Metrics?[0].Name == "before-initializer", "metric before-initializer");
		var afterItem = await telemetry.WaitForItemAsync(i => i.Data.BaseData.Metrics?[0].Name == "after-initializer", "metric after-initializer");

		Assert.NotEqual("test-role", beforeItem.CloudRoleName);
		Assert.Equal("test-role", afterItem.CloudRoleName);
	}
}
