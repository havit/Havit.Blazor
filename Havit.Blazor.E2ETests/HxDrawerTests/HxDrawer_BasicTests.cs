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
		await showButton.ClickAsync();

		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Move focus into the open drawer before pressing Escape. Playwright delivers key presses
		// to the focused element; right after opening, focus is still on the trigger button in the
		// now-inert page, so a bare Escape never reaches the dialog. (The previous implementation
		// instead awaited a 'shown.bs.drawer' event that is never raised in a headless/CI
		// environment - transitions emit no transitionend - which hung the whole E2E run forever.)
		await drawerContent.ClickAsync();

		// Press Escape. Bootstrap 6 (alpha) silently swallows dismissals while the entry transition
		// is still running, so the first keystroke can be ignored; a second press after a short
		// settle reliably dismisses the drawer.
		await Page.Keyboard.PressAsync("Escape");
		await Page.WaitForTimeoutAsync(500);
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
