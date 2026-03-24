using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxScrollspyTests;

[TestClass]
public class HxScrollspy_ScrollAndClick_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxScrollspy_Scroll_HighlightsCorrespondingNavLink()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxScrollspy_Basic");

		var navLink1 = Page.Locator("[data-testid='nav-link-1']");
		var navLink2 = Page.Locator("[data-testid='nav-link-2']");

		// Assert initial state: first section's nav link should be active
		await Expect(navLink1).ToHaveClassAsync(new Regex(@"\bactive\b"), new() { Timeout = 5_000 });

		// Act - scroll the scrollspy container down to bring section 2 into view
		await Page.EvaluateAsync("(() => { const container = document.querySelector('.test-scrollspy'); const section2 = document.querySelector('#section2'); if (container && section2) { container.scrollTop = section2.offsetTop; } })();");

		// Assert - second nav link should now be active
		await Expect(navLink2).ToHaveClassAsync(new Regex(@"\bactive\b"), new() { Timeout = 5_000 });
		await Expect(navLink1).Not.ToHaveClassAsync(new Regex(@"\bactive\b"), new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxScrollspy_ClickNavLink_ScrollsToSection()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxScrollspy_Basic");

		var scrollTopBefore = await Page.EvaluateAsync<double>("document.querySelector('.test-scrollspy').scrollTop");

		// Act - click on the third nav link
		var navLink3 = Page.Locator("[data-testid='nav-link-3']");
		await navLink3.ClickAsync();

		// Wait for the scrollspy to activate the third nav link (implies the container scrolled to section 3)
		await Expect(navLink3).ToHaveClassAsync(new Regex(@"\bactive\b"), new() { Timeout = 5_000 });

		// Assert - the scrollspy container should have scrolled down
		var scrollTopAfter = await Page.EvaluateAsync<double>("document.querySelector('.test-scrollspy').scrollTop");
		Assert.IsGreaterThan(scrollTopAfter, scrollTopBefore, $"Expected scrollTop to increase after clicking nav link 3 (was {scrollTopBefore}, now {scrollTopAfter})");
	}
}
