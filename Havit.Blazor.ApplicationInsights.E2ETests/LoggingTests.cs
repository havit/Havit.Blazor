using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class LoggingTests : BlazorApplicationInsightsPageTestBase
{
	protected override bool AllowConsoleErrors => true;

	[Fact]
	public async Task BlazorApplicationInsights_Logging_TraceLogProducesMessageDataTelemetry()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert
		Assert.Contains(
			capturedTelemetryItems,
			i => i.BaseType == "MessageData" && i.Data.BaseData.Message == "test-log-warning");
	}

	[Fact]
	public async Task BlazorApplicationInsights_Logging_TraceLogRespectsLogLevelConfiguration()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert		
		Assert.DoesNotContain(
			capturedTelemetryItems,
			i => i.BaseType == "MessageData" && i.Data.BaseData.Message == "test-log-information");
	}

	[Fact]
	public async Task BlazorApplicationInsights_Logging_ExceptionLogProducesExceptionDataTelemetry()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert
		Assert.Contains(
			capturedTelemetryItems,
			i => i.BaseType == "ExceptionData"
			  && i.Data.BaseData.Exceptions?[0].TypeName?.Contains("InvalidOperationException") == true);
	}

	private async Task ActAsync()
	{
		await Page.GotoAsync(NavigationRoutes.Logging.LoggingTestPage);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Task.Delay(1000, TestContext.Current.CancellationToken);
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		await Page.CloseAsync();
	}
}
