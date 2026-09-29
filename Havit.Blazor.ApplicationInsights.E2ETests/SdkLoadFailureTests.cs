using System.Diagnostics;
using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

/// <summary>
/// Covers the degradation path of the readiness gate in <c>HxApplicationInsights</c>: when the SDK script cannot be downloaded,
/// the calls queued behind the gate must be released (to the snippet's no-op stubs) instead of hanging or throwing,
/// and later calls must not wait again.
/// </summary>
public class SdkLoadFailureTests : BlazorApplicationInsightsPageTestBase
{
	// the gate opens on a 15 s fallback timer; the test drives the browser clock instead of sitting the timer out
	private const long GateFallbackTimeoutMilliseconds = 15_000;

	// every CDN fallback host the snippet tries is reported by the browser as a failed resource load
	protected override bool AllowConsoleErrors => true;

	[Fact]
	public async Task SdkScriptUnavailable_QueuedCallsAreReleasedWithoutException_LaterCallsDoNotWait()
	{
		// Arrange
		await Page.Clock.InstallAsync(); // fake timers, running in real time until fast-forwarded; has to be installed before the page loads
		var telemetry = await Page.RouteApplicationInsightsTrackAsync();
		await Page.RouteAsync("**/ai.3*.min.js", route => route.AbortAsync()); // the fallback hosts serve the same path

		// Act
		// the page issues a dozen tracking calls right away - all of them end up queued behind the gate
		await Page.GotoAsync(NavigationRoutes.TrackingMethodsTests);

		// the snippet gives up after its last fallback host and reports that as telemetry itself;
		// waiting for it keeps the snippet's own retry timers running at their real pace, only the gate timer gets skipped
		await telemetry.WaitForItemAsync(i => i.Data.BaseData.Exceptions?[0].TypeName == "SDKLoadFailed", "SDK load failure report from the snippet");
		await Page.Clock.FastForwardAsync(GateFallbackTimeoutMilliseconds);

		// Assert
		// the gate opened on the fallback, not because the SDK initialized (a hung gate would never resolve - hence the .NET-side bound)
		var gateOpenedBySdk = await Page.EvaluateAsync<bool>("() => window.havitBlazorAppInsights.ready")
			.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		Assert.False(gateOpenedBySdk);

		// a call made after the gate opened passes straight through (measured here, the browser clock is faked)
		var stopwatch = Stopwatch.StartNew();
		await Page.EvaluateAsync("() => window.havitBlazorAppInsights.trackEvent({ name: 'after-sdk-load-failure' })")
			.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		Assert.InRange(stopwatch.ElapsedMilliseconds, 0, 2_000);

		Assert.DoesNotContain(
			ConsoleMessages,
			m => (m.Type == "error") && (m.Text.Contains("TypeError") || m.Text.Contains("JSException") || m.Text.Contains("Circuit has been shut down")));
	}
}
