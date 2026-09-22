using System.Text.Json;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public static class PageExtensions
{
	/// <summary>
	/// Intercepts the Application Insights track endpoint so that no telemetry leaves the machine
	/// and returns a collector the test can wait on for specific items.
	/// </summary>
	public static async Task<AiTelemetryCollector> RouteApplicationInsightsTrackAsync(this IPage page)
	{
		var collector = new AiTelemetryCollector();

		await page.RouteAsync("**/v2/track", async route =>
		{
			var items = JsonSerializer.Deserialize<AiTelemetryItem[]>(route.Request.PostData ?? "[]");
			collector.Add(items);

			await route.FulfillAsync(new RouteFulfillOptions
			{
				Status = 200,
				ContentType = "application/json",
				Body = "{}"
			});
		});

		return collector;
	}

	/// <summary>
	/// Waits until the Application Insights JS SDK is fully initialized.
	/// The wrapper itself is installed asynchronously by interactive render modes, so its presence is awaited first.
	/// </summary>
	public static async Task WaitForApplicationInsightsReadyAsync(this IPage page)
	{
		await page.WaitForFunctionAsync("() => !!window.havitBlazorAppInsights");
		await page.EvaluateAsync("() => window.havitBlazorAppInsights.ready");
	}
}
