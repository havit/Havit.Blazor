using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxNavOverflowTests;

public class HxNavOverflowTests : TestAppTestBase
{
	[Fact]
	public async Task HxNavOverflow_NarrowContainer_ShowsNavAndMovesItemsToMoreMenu()
	{
		// Arrange - 8 items in a 360px wide container
		await NavigateToTestAppAsync("/HxNavOverflow");

		// Assert - Bootstrap 6 hides the nav until the .nav-overflow wrapper is initialized (by our JS, once the items are distributed)
		var wrapper = Page.Locator("[data-testid='nav-overflow-container'] > .nav-overflow");
		await Expect(wrapper).ToHaveClassAsync(new Regex(@"\bnav-overflow-initialized\b"), new() { Timeout = 5_000 });
		var nav = wrapper.Locator("> nav[data-testid='nav-overflow-nav']");
		await Expect(nav).ToBeVisibleAsync();

		// Assert - some, but not all, items moved to the "More" menu
		var moreToggle = nav.Locator(".nav-overflow-toggle");
		await Expect(moreToggle).ToBeVisibleAsync();
		int visibleItemsCount = await nav.Locator("> a.nav-link").Filter(new() { Visible = true }).CountAsync();
		Assert.InRange(visibleItemsCount, 1, 7);

		// Act - open the "More" menu
		await moreToggle.ClickAsync();

		// Assert - the menu shows the items hidden in the nav (8 items in total)
		var menuItems = nav.Locator(".nav-overflow-menu > a.menu-item").Filter(new() { Visible = true });
		await Expect(menuItems).ToHaveCountAsync(8 - visibleItemsCount, new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxNavOverflow_InCollapsedNavbarDrawer_ShowsAllItemsWithoutMoreMenu()
	{
		// Arrange - narrow viewport so the navbar is collapsed (the nav is vertical .navbar-nav in the drawer)
		await Page.SetViewportSizeAsync(375, 800);
		await NavigateToTestAppAsync("/HxNavOverflow");

		var nav = Page.Locator("nav[data-testid='nav-overflow-navbar-nav']");
		await Expect(nav.Locator("xpath=..")).ToHaveClassAsync(new Regex(@"\bnav-overflow-initialized\b"), new() { Timeout = 5_000 });

		// Act - open the drawer
		await Page.Locator("[data-testid='nav-overflow-navbar-toggler']").ClickAsync();
		await Expect(Page.Locator("#nav-overflow-navbar-drawer")).ToHaveAttributeAsync("open", "", new() { Timeout = 10_000 });

		// Assert - all items are visible in the drawer, the "More" toggle is hidden
		await Expect(nav.Locator("> a.nav-link").Filter(new() { Visible = true })).ToHaveCountAsync(12, new() { Timeout = 5_000 });
		await Expect(nav.Locator(".nav-overflow-toggle")).ToBeHiddenAsync();
	}

	[Fact]
	public async Task HxNavOverflow_InExpandedNavbar_MovesItemsToMoreMenu()
	{
		// Arrange - expanded navbar (1100px wide container, lg), not enough space for all 12 items
		await Page.SetViewportSizeAsync(1600, 800);
		await NavigateToTestAppAsync("/HxNavOverflow");

		var nav = Page.Locator("nav[data-testid='nav-overflow-navbar-nav']");
		await Expect(nav.Locator("xpath=..")).ToHaveClassAsync(new Regex(@"\bnav-overflow-initialized\b"), new() { Timeout = 5_000 });

		// Assert - horizontal nav, some (but not all) items moved to the "More" menu
		await Expect(nav.Locator(".nav-overflow-toggle")).ToBeVisibleAsync(new() { Timeout = 5_000 });
		int visibleItemsCount = await nav.Locator("> a.nav-link").Filter(new() { Visible = true }).CountAsync();
		Assert.InRange(visibleItemsCount, 1, 11);
	}
}
