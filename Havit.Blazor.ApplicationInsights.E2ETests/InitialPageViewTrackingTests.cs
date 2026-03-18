using System.Collections.Concurrent;
using System.Text.Json;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class InitialPageViewTrackingTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	private static BlazorWebApplicationFactory _factoryWithTrackingDisabled;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[ClassInitialize]
	public static void ClassInitialize(TestContext _)
	{
		_factoryWithTrackingDisabled = new BlazorWebApplicationFactory(
			opts => opts.EnableInitialPageViewTracking = false);
		_factoryWithTrackingDisabled.CreateClient();
	}

	[ClassCleanup]
	public static void ClassCleanup() => _factoryWithTrackingDisabled?.Dispose();

	[TestMethod]
	public async Task EnableInitialPageViewTracking_True_SendsInitialPageView()
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

			if (capturedItems.Any(i => GetBaseType(i) == "PageviewData"))
			{
				batchReceived.TrySetResult();
			}
		});

		// Act
		await Page.GotoAsync(PlaywrightFixture.Factory.ServerAddress + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		await batchReceived.Task.WaitAsync(TimeSpan.FromMilliseconds(AppInsightsTimeout), TestContext.CancellationToken);

		// Assert
		Assert.Contains(i => GetBaseType(i) == "PageviewData", capturedItems, "Expected at least one PageviewData item.");
	}

	[TestMethod]
	public async Task EnableInitialPageViewTracking_False_DoesNotSendInitialPageView()
	{
		// Arrange
		var capturedItems = new ConcurrentBag<JsonElement>();

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
		});

		// Act
		await Page.GotoAsync(_factoryWithTrackingDisabled.ServerAddress + NavigationRoutes.PageViewTracking.InitialPageViewTrackingTest);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		// Wait for any late-arriving requests after flush
		await Task.Delay(1000, TestContext.CancellationToken);

		// Assert
		Assert.DoesNotContain(i => GetBaseType(i) == "PageviewData", capturedItems, "Expected no PageviewData items when EnableInitialPageViewTracking is false.");
	}

	private static string GetBaseType(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseType", out var baseType)
			? baseType.GetString()
			: null;
}
