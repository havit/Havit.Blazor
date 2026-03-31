using Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;
using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Microsoft.Playwright;

namespace Havit.Blazor.ApplicationInsights.E2ETests;

[TestClass]
public class CookieManagerTests : BlazorApplicationInsightsPageTestBase
{
	[TestMethod]
	public async Task BlazorApplicationInsightsCookieManager_SetEnabledAsync_DisablesCookieAccess()
	{
		// Act
		await Page.GotoAsync(NavigationRoutes.CookieManagerTests);
		var done = Page.Locator("#done");
		await done.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached });

		// Assert — IsEnabledAsync reflects the state change
		Assert.AreEqual("True", await done.GetAttributeAsync("data-enabled-before"), "Cookies should be enabled before SetEnabledAsync(false).");
		Assert.AreEqual("False", await done.GetAttributeAsync("data-enabled-after"), "Cookies should be disabled after SetEnabledAsync(false).");
	}
}
