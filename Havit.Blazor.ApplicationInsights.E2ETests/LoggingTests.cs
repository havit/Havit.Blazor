using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class LoggingTests : BlazorApplicationInsightsPageTestBase
{
	protected override bool AllowConsoleErrors => true;

	[TestMethod]
	public async Task BlazorApplicationInsights_Logging_TraceLogProducesMessageDataTelemetry()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert
		Assert.Contains(
			i => i.BaseType == "MessageData" && i.Data.BaseData.Message == "test-log-warning",
			capturedTelemetryItems,
			"Expected MessageData with message 'test-log-warning'.");
	}

	[TestMethod]
	public async Task BlazorApplicationInsights_Logging_TraceLogRespectsLogLevelConfiguration()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert		
		Assert.DoesNotContain(
			i => i.BaseType == "MessageData" && i.Data.BaseData.Message == "test-log-information",
			capturedTelemetryItems,
			"Should not produce MessageData with message 'test-log-information'.");
	}

	[TestMethod]
	public async Task BlazorApplicationInsights_Logging_ExceptionLogProducesExceptionDataTelemetry()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await ActAsync();

		// Assert
		Assert.Contains(
			i => i.BaseType == "ExceptionData"
			  && i.Data.BaseData.Exceptions?[0].TypeName?.Contains("InvalidOperationException") == true,
			capturedTelemetryItems,
			"Expected ExceptionData for InvalidOperationException.");
	}

	private async Task ActAsync()
	{
		await Page.GotoAsync(NavigationRoutes.Logging.LoggingTestPage);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Task.Delay(1000, TestContext.CancellationToken);
		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		await Page.CloseAsync();
	}
}
