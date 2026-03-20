using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public class BlazorApplicationInsightsPageTestBase : PageTest
{
	public const float DefaultTimeoutMilliseconds = 30_000;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.ServerAddress
	};

	[TestInitialize]
	public async Task PageConfigureAsync()
	{
		Page.SetDefaultTimeout(DefaultTimeoutMilliseconds);
	}

}
