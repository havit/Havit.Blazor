using System.Text;
using Playwright.Axe;

namespace Havit.Blazor.E2ETests;

/// <summary>
/// Base class for E2E tests that provides Playwright context and helper methods.
/// </summary>
public abstract class TestAppTestBase : PageTest
{
	/// <summary>
	/// Gets the base URL of the running TestApp instance.
	/// </summary>
	protected static string BaseUrl => TestAppAssemblyInitializer.BaseUrl;

	/// <summary>
	/// The TestCleanup hook is intentionally used instead of a separate method invocation so that:
	/// - A11y checks are automatically executed after the main test flow completes
	/// - Existing tests do not need modification or duplication
	/// - The execution order remains consistent across all tests
	///
	/// This approach allows the same functional tests to be reused as an "accessibility overlay suite"
	/// by enabling the ACCESSIBILITYTESTS runtime toggle.
	/// Using a runtime toggle enables:
	/// - Clear separation between functional and accessibility test runs
	/// - Simpler and more predictable CI pipeline behavior without relying on DefineConstants
	/// - Zero A11y execution unless explicitly enabled
	/// </summary>
	private static bool AccessibilityTestsEnabled =>
#if ACCESSIBILITYTESTS
		true;
#else
		false;
#endif

	[TestCleanup]
	public async Task Cleanup()
	{
		try
		{
			if (AccessibilityTestsEnabled)
			{
				await RunAxe();
			}
		}
		finally
		{
			await Context.CloseAsync();
		}
	}

	// Temporary storage for the axe Result
	internal static readonly StringBuilder AxeReport = new();
	internal static readonly object Lock = new();

	/// <summary>
	/// Navigates to a relative path within the TestApp.
	/// </summary>
	/// <param name="relativePath">Relative path (e.g., "/counter" or "counter")</param>
	protected async Task NavigateToTestAppAsync(string relativePath)
	{
		if (!relativePath.StartsWith("/"))
		{
			relativePath = "/" + relativePath;
		}

		string fullUrl = BaseUrl + relativePath;
		await Page.GotoAsync(fullUrl);

		await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

		// see Havit.Blazor.Tests.TestApp.lib.module.js
		await Page.WaitForSelectorAsync("#blazor-ready-for-tests", new() { State = WaitForSelectorState.Attached });
	}

	protected async Task RunAxe()
	{
		AxeRunOptions options = new AxeRunOptions(
			runOnly: new AxeRunOnly(AxeRunOnlyType.Tag, new List<string>
			{
				"wcag2a",
				"wcag2aa",
				"wcag21a",
				"wcag21aa",
				"EN-301-549"
			}),

			// Specify rules.
			rules: new Dictionary<string, AxeRuleObjectValue>()
			{
				// ignore problematic rules, currently none applicable
				{ "document-title", new AxeRuleObjectValue(false) }
			},

			// Limit result types to Violations.
			resultTypes: new List<AxeResultGroup>()
			{
				AxeResultGroup.Violations
			},

			// Return CSS selectors in results.
			selectors: true,

			// Include all ancestor elements in the returned CSS selectors.
			ancestry: true,

			// Don't return xpath selectors for elements.
			xpath: false,

			// Don't run axe on iframes inside the document.
			iframes: false
		);

		var axeResults = await Page.RunAxe(options);
		var relevantViolations = axeResults.Violations
			.Where(v => v.Impact is AxeImpactValue.Serious or AxeImpactValue.Critical).ToList();
		if (relevantViolations.Any())
		{
			var message = new StringBuilder();

			foreach (var violation in relevantViolations)
			{
				var description = violation.Description?.Replace("\n", " ").Replace("|", "\\|");
				var impact = violation.Impact.ToString();

				foreach (var node in violation.Nodes)
				{
					var target = string.Join(", ", node.Target ?? new List<string>());
					target = target.Replace("|", "\\|");

					message.AppendLine($"| {TestContext.TestName} | {violation.Id} | {description} | {impact} | {target} |");
				}
			}

			lock (Lock)
			{
				if (message.Length > 0)
				{
					AxeReport.Append(message.ToString());
					Assert.Fail(message.ToString());
				}
			}
		}
	}
}