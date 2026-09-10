using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using Xunit;

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public class BlazorApplicationInsightsPageTestBase : PageTest
{
	public const float DefaultTimeoutMilliseconds = 30_000;

	protected virtual bool AllowConsoleErrors => false;
	private List<IConsoleMessage> _consoleMessages;

	public override BrowserNewContextOptions ContextOptions() => new BrowserNewContextOptions()
	{
		BaseURL = PlaywrightFixture.Factory.GetServerAddress()
	};

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();

		Page.SetDefaultTimeout(DefaultTimeoutMilliseconds);

		_consoleMessages = new List<IConsoleMessage>();
		Page.Console += (_, msg) =>
		{
			_consoleMessages.Add(msg);
		};
	}

	public override async ValueTask DisposeAsync()
	{
		if (!AllowConsoleErrors)
		{
			var errors = _consoleMessages.Where(m => m.Type == "error").ToList();
			if (errors.Count > 0)
			{
				Assert.Fail($"There were {errors.Count} console errors: {string.Join(Environment.NewLine, errors.Select(e => e.Text))}");
			}
		}

		await base.DisposeAsync();
	}
}
