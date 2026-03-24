namespace Havit.Blazor.E2ETests.HxNavbarTests;

[TestClass]
public class HxNavbar_Tests : TestAppTestBase
{
	private static readonly System.Text.RegularExpressions.Regex CollapseShowClassRegex = new System.Text.RegularExpressions.Regex("\\bshow\\b");

	[TestMethod]
	public async Task HxNavbar_Render_ShowsBrandAndItems()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);
		await NavigateToTestAppAsync("/HxNavbar");

		// Assert - brand is visible
		var brand = Page.Locator("[data-testid='navbar-brand']");
		await Expect(brand).ToBeVisibleAsync();

		// Assert - nav items are visible on wide viewport
		var navItemHome = Page.Locator("[data-testid='nav-item-home']");
		var navItemAbout = Page.Locator("[data-testid='nav-item-about']");
		await Expect(navItemHome).ToBeVisibleAsync();
		await Expect(navItemAbout).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxNavbar_Toggler_TogglesNavOnSmallViewport()
	{
		// Arrange - narrow viewport so the navbar is collapsed
		await Page.SetViewportSizeAsync(375, 800);
		await NavigateToTestAppAsync("/HxNavbar");

		var toggler = Page.Locator("[data-testid='navbar-toggler']");
		var navCollapse = Page.Locator("#test-navbar-collapse");

		// Assert - toggler is visible and nav is collapsed (no .show class)
		await Expect(toggler).ToBeVisibleAsync();
		await Expect(navCollapse).Not.ToHaveClassAsync(CollapseShowClassRegex, new() { Timeout = 5_000 });

		// Act - click toggler to expand navigation
		await toggler.ClickAsync();

		// Assert - nav collapse now has .show class (Bootstrap JS added it)
		await Expect(navCollapse).ToHaveClassAsync(CollapseShowClassRegex, new() { Timeout = 10_000 });

		// Assert - nav items are now visible
		var navItemHome = Page.Locator("[data-testid='nav-item-home']");
		await Expect(navItemHome).ToBeVisibleAsync();

		// Act - click toggler again to collapse navigation
		await toggler.ClickAsync();

		// Assert - nav collapse no longer has .show class
		await Expect(navCollapse).Not.ToHaveClassAsync(CollapseShowClassRegex, new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxNavbar_Brand_RendersCorrectly()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);
		await NavigateToTestAppAsync("/HxNavbar");

		// Assert - brand renders with correct text
		var brand = Page.Locator("[data-testid='navbar-brand']");
		await Expect(brand).ToBeVisibleAsync();
		await Expect(brand).ToHaveTextAsync("MyBrand");

		// Assert - brand link has correct href
		var brandLink = Page.Locator("#test-navbar .navbar-brand");
		await Expect(brandLink).ToHaveAttributeAsync("href", "/");
	}
}
