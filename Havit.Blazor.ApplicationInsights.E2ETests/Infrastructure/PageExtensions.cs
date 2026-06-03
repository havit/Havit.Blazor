using System.Text.Json;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.Model;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public static class PageExtensions
{
	public static async Task RouteApplicationInsightsTrackAsync(this IPage page, ICollection<AiTelemetryItem> capturedTelemetryItems)
	{
		await page.RouteAsync("**/v2/track", async route =>
		{
			if (capturedTelemetryItems != null)
			{
				var items = JsonSerializer.Deserialize<AiTelemetryItem[]>(route.Request.PostData ?? "[]");
				lock (capturedTelemetryItems)
				{
					foreach (var item in items)
					{
						capturedTelemetryItems.Add(item);
					}
				}
			}

			await route.FulfillAsync(new RouteFulfillOptions
			{
				Status = 200,
				ContentType = "application/json",
				Body = "{}"
			});
		});
	}
}
