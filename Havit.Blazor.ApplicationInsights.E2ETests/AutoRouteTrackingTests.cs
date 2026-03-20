using System.Collections.Concurrent;
using System.Text.Json;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class AutoRouteTrackingTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	private static BlazorWebApplicationFactory _factoryAutoRouteTrackingEnabled;
	private static BlazorWebApplicationFactory _factoryAutoRouteTrackingDisabled;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[ClassInitialize]
	public static void ClassInitialize(TestContext _)
	{
		_factoryAutoRouteTrackingEnabled = new BlazorWebApplicationFactory(options =>
		{
			options.JsSdkOptions.EnableAutoRouteTracking = true;
			options.EnableInitialPageViewTracking = false; // Avoid timing-sensitive false positive: initial trackPageView({}) from the snippet could be dequeued by the CDN SDK after navigation (using the new URL).
		});
		_factoryAutoRouteTrackingEnabled.CreateClient();

		_factoryAutoRouteTrackingDisabled = new BlazorWebApplicationFactory(options =>
		{
			options.JsSdkOptions.EnableAutoRouteTracking = false;
			options.EnableInitialPageViewTracking = false; // Avoid timing-sensitive false positive: initial trackPageView({}) from the snippet could be dequeued by the CDN SDK after navigation (using the new URL).
		});
		_factoryAutoRouteTrackingDisabled.CreateClient();
	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		_factoryAutoRouteTrackingEnabled?.Dispose();
		_factoryAutoRouteTrackingDisabled?.Dispose();
	}

	[TestMethod]
	public async Task EnableAutoRouteTracking_True_SendsPageViewOnNavigation()
	{
		// Arrange
		var capturedItems = new ConcurrentBag<JsonElement>();
		var page2PageViewReceived = new TaskCompletionSource();

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

			if (capturedItems.Any(i => GetPageViewUrl(i)?.Contains("auto-route-tracking-2") == true))
			{
				page2PageViewReceived.TrySetResult();
			}
		});

		// Act
		await Page.GotoAsync(_factoryAutoRouteTrackingEnabled.ServerAddress + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core",
			options: new PageWaitForFunctionOptions { Timeout = AppInsightsTimeout }); // Wait for the real CDN SDK (not just the stub) so the history.pushState hook is in place before navigating.
		await Page.ClickAsync("#goto-page2");
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		await page2PageViewReceived.Task.WaitAsync(TimeSpan.FromMilliseconds(AppInsightsTimeout), TestContext.CancellationToken);

		// Assert
		Assert.Contains(i => GetPageViewUrl(i)?.Contains("auto-route-tracking-2") == true, capturedItems, "Expected a PageviewData item for Page 2 URL.");
	}

	[TestMethod]
	public async Task EnableAutoRouteTracking_False_DoesNotSendPageViewOnNavigation()
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
		await Page.GotoAsync(_factoryAutoRouteTrackingDisabled.ServerAddress + NavigationRoutes.PageViewTracking.AutoRouteTrackingPage1);
		await Page.WaitForFunctionAsync("window.appInsights && window.appInsights.core",
			options: new PageWaitForFunctionOptions { Timeout = AppInsightsTimeout }); // Wait for the real CDN SDK (not just the stub) so the test reflects actual SDK behavior.
		await Page.ClickAsync("#goto-page2");
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		// Wait for any late-arriving requests after flush
		await Task.Delay(1000, TestContext.CancellationToken);

		// Assert
		Assert.DoesNotContain(i => GetPageViewUrl(i)?.Contains("auto-route-tracking-2") == true, capturedItems, "Expected no PageviewData for Page 2 URL when EnableAutoRouteTracking is false.");
	}

	private static string GetBaseType(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseType", out var baseType)
			? baseType.GetString()
			: null;

	private static string GetPageViewUrl(JsonElement item) =>
		GetBaseType(item) == "PageviewData"
		&& item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("url", out var url)
			? url.GetString()
			: null;
}
