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
	public async Task HxDrawer_ClickBackdrop_ClosesPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDrawer_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		var drawerContent = Page.Locator("[data-testid='drawer-content']");
		await Expect(drawerContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the native ::backdrop (outside the drawer, which is placed at the end/right)
		await Page.Mouse.ClickAsync(10, 10);

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
