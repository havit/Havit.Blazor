using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

public class TrackingMethodsTests : BlazorApplicationInsightsPageTestBase
{
	[Fact]
	public async Task BlazorApplicationInsights_TrackingMethods_AllMethodsProduceTelemetry()
	{
		// Arrange
		var capturedTelemetryItems = new List<AiTelemetryItem>();
		await Page.RouteApplicationInsightsTrackAsync(capturedTelemetryItems);

		// Act
		await Page.GotoAsync(NavigationRoutes.TrackingMethodsTests);
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached });
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
		await Page.CloseAsync();

		// Assert
		void AssertItemCaptured(string expectedBaseType, string expectedIdentifier, Func<AiTelemetryItem, string> getIdentifierFunc, string methodNameToAssert)
		{
			var item = capturedTelemetryItems.FirstOrDefault(i => i.BaseType == expectedBaseType && getIdentifierFunc(i) == expectedIdentifier);
			Assert.NotNull(item);
		}

		AssertItemCaptured("EventData", "test-event", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackEventAsync));
		AssertItemCaptured("PageviewData", "test-page-view", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackPageViewAsync));
		AssertItemCaptured("ExceptionData", "test-exception", i => i.Data.BaseData.Exceptions?[0].TypeName, nameof(IBlazorApplicationInsights.TrackExceptionAsync));
		AssertItemCaptured("MessageData", "test-trace", i => i.Data.BaseData.Message, nameof(IBlazorApplicationInsights.TrackTraceAsync));
		AssertItemCaptured("MetricData", "test-metric", i => i.Data.BaseData.Metrics?[0].Name, nameof(IBlazorApplicationInsights.TrackMetricAsync));
		AssertItemCaptured("PageviewData", "test-tracked-page", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.StopTrackPageAsync));
		AssertItemCaptured("EventData", "test-tracked-event", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.StopTrackEventAsync));
		AssertItemCaptured("PageviewPerformanceData", "test-page-view-performance", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackPageViewPerformanceAsync));
		AssertItemCaptured("RemoteDependencyData", "test-dependency", i => i.Data.BaseData.Name, nameof(IBlazorApplicationInsights.TrackDependencyDataAsync));
	}
}
