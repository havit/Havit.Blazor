using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class TrackMetricTests : PageTest
{
	private const float AppInsightsTimeout = 2_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task TrackMetric_InteractiveServer_MetricIsTracked()
	{
		// Arrange — spy obalí trackMetric před inicializací SDK;
		// setter se spustí ve chvíli, kdy ApplicationInsightsScript nastaví window.appInsights
		await Page.AddInitScriptAsync("""
			window._trackedMetrics = [];
			Object.defineProperty(window, 'appInsights', {
				configurable: true,
				set(value) {
					Object.defineProperty(window, 'appInsights', {
						configurable: true, writable: true, value
					});
					const orig = value.trackMetric.bind(value);
					value.trackMetric = function(metric, customProps) {
						window._trackedMetrics.push(metric);
						return orig(metric, customProps);
					};
				}
			});
			""");

		// Act
		await Page.GotoAsync(NavigationRoutes.AppInsightsTestTrackMetric);

		// Assert
		await Page.WaitForFunctionAsync(
			"window._trackedMetrics && window._trackedMetrics.length > 0",
			options: new PageWaitForFunctionOptions { Timeout = AppInsightsTimeout });

		Assert.AreEqual("TestMetric", await Page.EvaluateAsync<string>("window._trackedMetrics[0].name"));
		Assert.AreEqual(42.0, await Page.EvaluateAsync<double>("window._trackedMetrics[0].average"));
	}
}
