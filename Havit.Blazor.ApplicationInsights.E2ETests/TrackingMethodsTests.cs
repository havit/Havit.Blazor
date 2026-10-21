using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class TrackingMethodsTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task BlazorApplicationInsights_TrackingMethods_AllMethodsProduceTelemetry()
	{
		// Arrange
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();

		// Act
		await Page.GotoAsync(NavigationRoutes.TrackingMethodsTests);

		// Assert
		async Task AssertItemCapturedAsync(string expectedBaseType, string expectedIdentifier, Func<AiTelemetryItem, string> getIdentifierFunc, string methodNameToAssert)
		{
			await telemetry.WaitForItemAsync(
				i => (i.BaseType == expectedBaseType) && (getIdentifierFunc(i) == expectedIdentifier),
				$"{methodNameToAssert} -> {expectedBaseType} '{expectedIdentifier}'");
		}

		await AssertItemCapturedAsync("EventData", "test-event", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackEventAsync));
		await AssertItemCapturedAsync("PageviewData", "test-page-view", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackPageViewAsync));
		await AssertItemCapturedAsync("ExceptionData", "test-exception", i => i.Data.BaseData.Exceptions?[0].TypeName, nameof(IBlazorApplicationInsights.TrackExceptionAsync));
		await AssertItemCapturedAsync("MessageData", "test-trace", i => i.Data.BaseData.Message, nameof(IBlazorApplicationInsights.TrackTraceAsync));
		await AssertItemCapturedAsync("MetricData", "test-metric", i => i.Data.BaseData.Metrics?[0].Name, nameof(IBlazorApplicationInsights.TrackMetricAsync));
		await AssertItemCapturedAsync("PageviewData", "test-tracked-page", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.StopTrackPageAsync));
		await AssertItemCapturedAsync("EventData", "test-tracked-event", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.StopTrackEventAsync));
		await AssertItemCapturedAsync("PageviewPerformanceData", "test-page-view-performance", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackPageViewPerformanceAsync));
		await AssertItemCapturedAsync("RemoteDependencyData", "test-dependency", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackDependencyDataAsync));
	}
}
