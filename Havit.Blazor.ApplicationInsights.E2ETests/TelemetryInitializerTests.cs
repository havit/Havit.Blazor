using System.Collections.Concurrent;
using System.Text.Json;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class TelemetryInitializerTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task BlazorApplicationInsights_TelemetryInitializer_CloudRoleNameAppliedAfterRegistration()
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

			if (capturedItems.Count >= 2)
			{
				batchReceived.TrySetResult();
			}
		});

		// Act
		await Page.GotoAsync(NavigationRoutes.TelemetryInitializerTests);
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		await batchReceived.Task.WaitAsync(TimeSpan.FromMilliseconds(AppInsightsTimeout), TestContext.CancellationToken);

		// Assert
		JsonElement? beforeItem = capturedItems.Where(i => GetMetricName(i) == "before-initializer").Cast<JsonElement?>().FirstOrDefault();
		JsonElement? afterItem = capturedItems.Where(i => GetMetricName(i) == "after-initializer").Cast<JsonElement?>().FirstOrDefault();

		Assert.IsNotNull(beforeItem, "before-initializer metric not found in captured telemetry.");
		Assert.IsNotNull(afterItem, "after-initializer metric not found in captured telemetry.");

		Assert.IsNull(GetCloudRoleName(beforeItem.Value), "before-initializer should not have ai.cloud.role.");
		Assert.AreEqual("test-role", GetCloudRoleName(afterItem.Value), "after-initializer should have ai.cloud.role 'test-role'.");
	}

	private static string GetMetricName(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("metrics", out var metrics)
		&& metrics.GetArrayLength() > 0
		&& metrics[0].TryGetProperty("name", out var name)
			? name.GetString()
			: null;

	private static string GetCloudRoleName(JsonElement item) =>
		item.TryGetProperty("tags", out var tags)
		&& tags.TryGetProperty("ai.cloud.role", out var cloudRole)
			? cloudRole.GetString()
			: null;
}
