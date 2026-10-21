namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public class BlazorApplicationInsightsPageTestBase : PageTest
{
	public const float DefaultTimeoutMilliseconds = 30_000;

	protected virtual bool AllowConsoleErrors => false;
	private List<IConsoleMessage> _consoleMessages;

	/// <summary>
	/// Browser console messages captured since the page was created.
	/// For tests which allow console errors in general but still want to assert on specific ones.
	/// </summary>
	protected IReadOnlyList<IConsoleMessage> ConsoleMessages => _consoleMessages;

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
		string consoleErrorsFailureMessage = null;

		if (!AllowConsoleErrors)
		{
			var errors = _consoleMessages.Where(m => m.Type == "error").ToList();
			if (errors.Count > 0)
			{
				consoleErrorsFailureMessage = $"There were {errors.Count} console errors: {string.Join(Environment.NewLine, errors.Select(e => e.Text))}";
			}
		}

		// the Playwright teardown (closing the browser context, recycling the worker) must always run,
		// otherwise a test failing on console errors leaks a browser process until the test run ends
		await base.DisposeAsync();

		if (consoleErrorsFailureMessage != null)
		{
			Assert.Fail(consoleErrorsFailureMessage);
		}
	}
}
