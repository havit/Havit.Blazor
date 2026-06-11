namespace Havit.Blazor.E2ETests.HxNavbarTests;

public class HxNavbarTests : TestAppTestBase
{

	[Fact]
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

	[Fact]
	public async Task HxNavbar_Toggler_TogglesNavOnSmallViewport()
	{
		// Arrange - narrow viewport so the navbar is collapsed
		await Page.SetViewportSizeAsync(375, 800);
		await NavigateToTestAppAsync("/HxNavbar");

		var toggler = Page.Locator("[data-testid='navbar-toggler']");
		var navCollapse = Page.Locator("#test-navbar-collapse");

		// Assert - toggler is visible and the navbar drawer is closed (Bootstrap 6 renders the responsive navbar content as a drawer)
		await Expect(toggler).ToBeVisibleAsync();
		await Expect(navCollapse).Not.ToHaveAttributeAsync("open", "", new() { Timeout = 5_000 });

		// Act - click toggler to expand navigation
		await toggler.ClickAsync();

		// Assert - the navbar drawer is open (Bootstrap Drawer plugin opened the native dialog)
		await Expect(navCollapse).ToHaveAttributeAsync("open", "", new() { Timeout = 10_000 });

		// Assert - nav items are now visible
		var navItemHome = Page.Locator("[data-testid='nav-item-home']");
		await Expect(navItemHome).ToBeVisibleAsync();

		// Act - click toggler again to collapse navigation
		// Close via the drawer's close button - the open modal drawer intercepts pointer
		// events over the rest of the page, so the toggler underneath is not clickable
		// (Bootstrap 6 navbar UX: close via the close button, the backdrop, or Escape).
		await Page.Locator("#test-navbar-collapse .btn-close").ClickAsync();

		// Assert - the navbar drawer is closed again
		await Expect(navCollapse).Not.ToHaveAttributeAsync("open", "", new() { Timeout = 10_000 });
	}

	[Fact]
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
