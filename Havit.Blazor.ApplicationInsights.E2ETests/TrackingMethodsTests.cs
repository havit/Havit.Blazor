#nullable enable

using System.Collections.Concurrent;
using System.Text.Json;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class TrackingMethodsTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	// 9 explicitly tracked items:
	// TrackEvent, TrackPageView, TrackException, TrackTrace, TrackMetric,
	// StopTrackPage, StopTrackEvent, TrackPageViewPerformance, TrackDependencyData
	private const int ExpectedItemCount = 9;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task BlazorApplicationInsights_TrackingMethods_AllMethodsProduceTelemetry()
	{
		// Arrange
		var capturedItems = new ConcurrentBag<JsonElement>();
		var batchReceived = new TaskCompletionSource();

		await Page.RouteAsync("**/v2/track", async route =>
		{
			var items = JsonSerializer.Deserialize<JsonElement[]>(route.Request.PostData ?? "[]");
			if (items is not null)
			{
				foreach (var item in items)
				{
					capturedItems.Add(item);
				}
			}

			int itemsCount = items?.Length ?? 0;
			await route.FulfillAsync(new RouteFulfillOptions
			{
				Status = 200,
				ContentType = "application/json",
				Body = $$"""{"itemsReceived":{{itemsCount}},"itemsAccepted":{{itemsCount}},"errors":[]}"""
			});

			if (capturedItems.Count >= ExpectedItemCount)
			{
				batchReceived.TrySetResult();
			}
		});

		// Act
		await Page.GotoAsync(NavigationRoutes.TrackingMethodsTests);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		await batchReceived.Task.WaitAsync(TimeSpan.FromMilliseconds(AppInsightsTimeout), TestContext.CancellationToken);

		// Assert
		void AssertItemCaptured(string expectedBaseType, string expectedIdentifier, Func<JsonElement, string?> getIdentifierFunc, string methodNameToAssert)
		{
			JsonElement? item = capturedItems
				.Where(i => GetBaseType(i) == expectedBaseType && getIdentifierFunc(i) == expectedIdentifier)
				.Cast<JsonElement?>()
				.FirstOrDefault();
			Assert.IsNotNull(item, $"{methodNameToAssert}: Telemetry item with baseType '{expectedBaseType}' (and identifier '{expectedIdentifier}') not found in captured telemetry.");
		}

		AssertItemCaptured("EventData", "test-event", GetBaseDataName, nameof(IBlazorApplicationInsights.TrackEventAsync));
		AssertItemCaptured("PageviewData", "test-page-view", GetBaseDataName, nameof(IBlazorApplicationInsights.TrackPageViewAsync));
		AssertItemCaptured("ExceptionData", "test-exception", GetFirstExceptionTypeName, nameof(IBlazorApplicationInsights.TrackExceptionAsync));
		AssertItemCaptured("MessageData", "test-trace", GetBaseDataMessage, nameof(IBlazorApplicationInsights.TrackTraceAsync));
		AssertItemCaptured("MetricData", "test-metric", GetMetricName, nameof(IBlazorApplicationInsights.TrackMetricAsync));
		AssertItemCaptured("PageviewData", "test-tracked-page", GetBaseDataName, nameof(IBlazorApplicationInsights.StopTrackPageAsync));
		AssertItemCaptured("EventData", "test-tracked-event", GetBaseDataName, nameof(IBlazorApplicationInsights.StopTrackEventAsync));
		AssertItemCaptured("PageviewPerformanceData", "test-page-view-performance", GetBaseDataName, nameof(IBlazorApplicationInsights.TrackPageViewPerformanceAsync));
		AssertItemCaptured("RemoteDependencyData", "test-dependency", GetBaseDataName, nameof(IBlazorApplicationInsights.TrackDependencyDataAsync));
	}

	private static string? GetBaseType(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseType", out var baseType)
			? baseType.GetString()
			: null;

	private static string? GetBaseDataName(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("name", out var name)
			? name.GetString()
			: null;

	private static string? GetBaseDataMessage(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("message", out var message)
			? message.GetString()
			: null;

	private static string? GetMetricName(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("metrics", out var metrics)
		&& metrics.GetArrayLength() > 0
		&& metrics[0].TryGetProperty("name", out var name)
			? name.GetString()
			: null;

	private static string? GetFirstExceptionTypeName(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("exceptions", out var exceptions)
		&& exceptions.GetArrayLength() > 0
		&& exceptions[0].TryGetProperty("typeName", out var message)
			? message.GetString()
			: null;
}
