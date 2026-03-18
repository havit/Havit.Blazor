using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class AuthenticatedUserContextTests : PageTest
{
	private const float AppInsightsTimeout = 2_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestMethod]
	public async Task SetAuthenticatedUserContext_InteractiveServer_TelemetryContainsUserId()
	{
		// Arrange — telemetryInitializer spy zachytí auth user ID ze všech telemetrických položek;
		// je zaregistrován okamžitě při nastavení window.appInsights, dříve než C# kód odešle data
		await Page.AddInitScriptAsync(BuildTelemetrySpyScript());

		// Act
		await Page.GotoAsync(NavigationRoutes.AppInsightsTestAuthenticatedUserContext);

		// Stránka vykreslí <span id="done"> po dokončení celé sekvence Set/Track/Clear/Track
		await Page.WaitForSelectorAsync("#done",
			new PageWaitForSelectorOptions { Timeout = AppInsightsTimeout });

		// Assert — metrika odeslaná PO SetAuthenticatedUserContext musí obsahovat authUserId
		var authUserId = await Page.EvaluateAsync<string>(
			"window._telemetryItems.find(i => i.name === 'WithAuth')?.authUserId");
		Assert.AreEqual("test-user", authUserId);
	}

	[TestMethod]
	public async Task ClearAuthenticatedUserContext_InteractiveServer_TelemetryNoLongerContainsUserId()
	{
		// Arrange
		await Page.AddInitScriptAsync(BuildTelemetrySpyScript());

		// Act
		await Page.GotoAsync(NavigationRoutes.AppInsightsTestAuthenticatedUserContext);
		await Page.WaitForSelectorAsync("#done",
			new PageWaitForSelectorOptions { Timeout = AppInsightsTimeout });

		// Assert — metrika odeslaná PO ClearAuthenticatedUserContext nesmí obsahovat authUserId
		var authUserId = await Page.EvaluateAsync<string>(
			"window._telemetryItems.find(i => i.name === 'WithoutAuth')?.authUserId");
		Assert.IsNull(authUserId);
	}

	/// <summary>
	/// JS spy injektovaný do stránky přes Playwright AddInitScript.
	/// Přepisuje setter window.appInsights a okamžitě registruje telemetryInitializer,
	/// který pro každou odeslanou telemetrickou položku uloží její název a authUserId tag.
	/// </summary>
	private static string BuildTelemetrySpyScript() => """
		window._telemetryItems = [];
		Object.defineProperty(window, 'appInsights', {
			configurable: true,
			set(value) {
				Object.defineProperty(window, 'appInsights', {
					configurable: true, writable: true, value
				});
				value.addTelemetryInitializer(function(envelope) {
					window._telemetryItems.push({
						name: envelope.baseData?.name,
						authUserId: (envelope.tags && envelope.tags['ai.user.authUserId']) || null
					});
				});
			}
		});
		""";
}
