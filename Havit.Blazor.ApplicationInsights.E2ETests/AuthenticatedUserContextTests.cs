using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class AuthenticatedUserContextTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task BlazorApplicationInsights_AuthenticationUserContext_Test()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.AuthenticationUserContextTests);

		// Assert
		var message1 = await telemetry.WaitForItemAsync(i => i.Data.BaseData.Metrics?[0].Name == "Message1-WithoutAuth", "metric Message1-WithoutAuth");
		var message2 = await telemetry.WaitForItemAsync(i => i.Data.BaseData.Metrics?[0].Name == "Message2-WithAuth", "metric Message2-WithAuth");
		var message3 = await telemetry.WaitForItemAsync(i => i.Data.BaseData.Metrics?[0].Name == "Message3-WithoutAuth", "metric Message3-WithoutAuth");

		Assert.Null(message1.AuthUserId);
		Assert.Equal("test-user", message2.AuthUserId);
		Assert.Null(message3.AuthUserId);
	}
}
