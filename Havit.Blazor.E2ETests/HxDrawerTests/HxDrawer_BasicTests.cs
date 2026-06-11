namespace Havit.Blazor.E2ETests.HxDrawerTests;

public class HxDrawer_BasicTests : TestAppTestBase
{
	[Fact]
	public async Task HxDrawer_Open_ShowsPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDrawer_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - drawer panel content should be visible
		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[Fact]
	public async Task HxDrawer_ClickCloseButton_ClosesPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDrawer_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the close button inside the drawer header
		var closeButton = Page.Locator(".hx-drawer-close-button");
		await closeButton.ClickAsync();

		// Assert - drawer content should no longer be visible
		await Expect(drawerContent).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[Fact]
	public async Task HxDrawer_PressEscape_ClosesPanel()
	{
		// Light-dismiss coverage. Backdrop-click dismissal cannot be asserted in E2E:
		// clicks on a native <dialog>'s ::backdrop region do not dispatch DOM events
		// when the point lies outside the dialog's own box (the page beneath is inert),
		// so neither the Bootstrap Drawer plugin nor any script can observe them -
		// a platform constraint shared with upstream Bootstrap 6. Escape exercises
		// the same dismissal flow deterministically.

		// Arrange
		await NavigateToTestAppAsync("/HxDrawer_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");
		await Page.EvaluateAsync("window.__hxDrawerShown = new Promise(resolve => document.addEventListener('shown.bs.drawer', resolve, { once: true }))");
		await showButton.ClickAsync();

		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Wait for the entry transition to finish (shown.bs.drawer): Bootstrap 6 (alpha)
		// silently swallows dismissals (incl. native Escape) while _isTransitioning is true.
		await Page.EvaluateAsync("window.__hxDrawerShown");

		// Act - press Escape
		await Page.Keyboard.PressAsync("Escape");

		// Assert - drawer content should no longer be visible
		await Expect(drawerContent).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[Fact]
	public async Task HxDrawer_Content_RendersCorrectly()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDrawer_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - drawer content text renders correctly
		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(drawerContent).ToHaveTextAsync("Drawer body content");

		// Assert - drawer title renders correctly
		var drawerTitle = Page.Locator(".drawer-title");
		await Expect(drawerTitle).ToBeVisibleAsync();
		await Expect(drawerTitle).ToHaveTextAsync("Test Drawer");
	}
}
