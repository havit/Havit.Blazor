using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class AuthenticatedUserContextTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task BlazorApplicationInsights_AuthenticationUserContext_Test()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(NavigationRoutes.AuthenticationUserContextTests);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.CloseAsync();

		// Assert
		var message1 = capturedTelemetryItems.FirstOrDefault(i => i.Data.BaseData.Metrics?[0].Name == "Message1-WithoutAuth");
		var message2 = capturedTelemetryItems.FirstOrDefault(i => i.Data.BaseData.Metrics?[0].Name == "Message2-WithAuth");
		var message3 = capturedTelemetryItems.FirstOrDefault(i => i.Data.BaseData.Metrics?[0].Name == "Message3-WithoutAuth");

		Assert.NotNull(message1);
		Assert.NotNull(message2);
		Assert.NotNull(message3);

		Assert.Null(message1.AuthUserId);
		Assert.Equal("test-user", message2.AuthUserId);
		Assert.Null(message3.AuthUserId);
	}
}
