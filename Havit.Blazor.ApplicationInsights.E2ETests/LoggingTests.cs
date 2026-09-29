using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class LoggingTests : BlazorApplicationInsightsPageTestBase
{
	// The page logs an error with an exception which shows up in the browser console too.
	protected override bool AllowConsoleErrors => true;

	[Fact]
	public async Task BlazorApplicationInsights_Logging_TraceLogProducesMessageDataTelemetry()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.Logging.LoggingTestPage);

		// Assert
		await telemetry.WaitForItemAsync(
			i => (i.BaseType == "MessageData") && (i.Data.BaseData.Message == "test-log-warning"),
			"trace test-log-warning");
	}

	[Fact]
	public async Task BlazorApplicationInsights_Logging_TraceLogRespectsLogLevelConfiguration()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.Logging.LoggingTestPage);

		// Assert
		// the page logs Information, Warning and Error in this order - once the Error (logged last) is on the wire,
		// the Information trace would be there too if the log level configuration did not filter it out
		await telemetry.WaitForItemAsync(
			i => (i.BaseType == "ExceptionData") && (i.Data.BaseData.Exceptions?[0].TypeName?.Contains("InvalidOperationException") == true),
			"exception test-log-exception (barrier)");
		Assert.DoesNotContain(telemetry.Items, i => (i.BaseType == "MessageData") && (i.Data.BaseData.Message == "test-log-information"));
	}

	[Fact]
	public async Task BlazorApplicationInsights_Logging_ExceptionLogProducesExceptionDataTelemetry()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.Logging.LoggingTestPage);

		// Assert
		await telemetry.WaitForItemAsync(
			i => (i.BaseType == "ExceptionData") && (i.Data.BaseData.Exceptions?[0].TypeName?.Contains("InvalidOperationException") == true),
			"exception InvalidOperationException");
	}
}
