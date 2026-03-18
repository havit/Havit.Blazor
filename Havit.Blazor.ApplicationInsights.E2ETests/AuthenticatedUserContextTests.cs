using System.Collections.Concurrent;
using System.Text.Json;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class AuthenticatedUserContextTests : PageTest
{
	private const float AppInsightsTimeout = 10_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task BlazorApplicationInsights_AuhenticationUserContext_Test()
	{
		// Arrange — intercept HTTP batches sent to the AI ingestion endpoint.
		// The final envelope already has tags correctly populated (incl. ai.user.authUserId),
		// because the SDK applies them before sending — unlike addTelemetryInitializer,
		// which fires before the SDK has set the tags.
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

			if (capturedItems.Count >= 3)
			{
				batchReceived.TrySetResult();
			}
		});

		// Act
		await Page.GotoAsync(NavigationRoutes.AuthenticationUserContextTests);

		// The page renders <span id="done"> after the entire sequence completes — flush() is called
		// just before that, so the HTTP request to the AI endpoint is already in-flight or completed.
		await Page.WaitForSelectorAsync("#done", new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = AppInsightsTimeout });
		await batchReceived.Task.WaitAsync(TimeSpan.FromMilliseconds(AppInsightsTimeout), TestContext.CancellationToken);

		// Assert
		// JsonElement is a struct — FirstOrDefault returns default(JsonElement), not null.
		// Casting to JsonElement? gives a truly nullable value so Assert.IsNotNull is meaningful.
		JsonElement? message1 = capturedItems.Where(i => GetMetricName(i) == "Message1-WithoutAuth").Cast<JsonElement?>().FirstOrDefault();
		JsonElement? message2 = capturedItems.Where(i => GetMetricName(i) == "Message2-WithAuth").Cast<JsonElement?>().FirstOrDefault();
		JsonElement? message3 = capturedItems.Where(i => GetMetricName(i) == "Message3-WithoutAuth").Cast<JsonElement?>().FirstOrDefault();

		Assert.IsNotNull(message1, "Message1-WithoutAuth not found in captured telemetry.");
		Assert.IsNotNull(message2, "Message2-WithAuth not found in captured telemetry.");
		Assert.IsNotNull(message3, "Message3-WithoutAuth not found in captured telemetry.");

		Assert.IsNull(GetAuthUserId(message1.Value), "Message1 should not have authUserId.");
		Assert.AreEqual("test-user", GetAuthUserId(message2.Value), "Message2 should have authUserId 'test-user'.");
		Assert.IsNull(GetAuthUserId(message3.Value), "Message3 should not have authUserId after Clear.");
	}

	private static string GetMetricName(JsonElement item) =>
		item.TryGetProperty("data", out var data)
		&& data.TryGetProperty("baseData", out var baseData)
		&& baseData.TryGetProperty("metrics", out var metrics)
		&& metrics.GetArrayLength() > 0
		&& metrics[0].TryGetProperty("name", out var name)
			? name.GetString()
			: null;

	private static string GetAuthUserId(JsonElement item) =>
		item.TryGetProperty("tags", out var tags)
		&& tags.TryGetProperty("ai.user.authUserId", out var authUserId)
			? authUserId.GetString()
			: null;
}
