namespace Havit.Blazor.E2ETests.HxMenuTests;

public class HxMenu_Tests : TestAppTestBase
{
	private const int MenuAnimationTimeout = 5_000;

	[Fact]
	public async Task HxMenu_ClickToggle_OpensMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxMenu");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var menu = Page.Locator(".menu");

		// Assert - menu is not visible before clicking
		await Expect(menu).Not.ToBeVisibleAsync();

		// Act - click the toggle button
		await toggleButton.ClickAsync();

		// Assert - menu is visible after clicking the toggle
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
	}

	[Fact]
	public async Task HxMenu_ClickOutside_ClosesMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxMenu");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var menu = Page.Locator(".menu");

		// Act 1 - open the menu
		await toggleButton.ClickAsync();
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });

		// Act 2 - click outside the menu to close it
		await Page.Locator("[data-testid='outside-click-area']").ClickAsync();

		// Assert - menu is no longer visible
		await Expect(menu).Not.ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
	}

	[Fact]
	public async Task HxMenu_ClickItem_TriggersActionAndCloses()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxMenu");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var actionItem = Page.Locator("[data-testid='action-item']");
		var menu = Page.Locator(".menu");
		var itemClickCount = Page.Locator("[data-testid='item-click-count']");

		// Act - open the menu and click the action item
		await toggleButton.ClickAsync();
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
		await actionItem.ClickAsync();

		// Assert - handler was called and menu closed
		await Expect(itemClickCount).ToHaveTextAsync("1");
		await Expect(menu).Not.ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
	}

	[Fact]
	public async Task HxMenu_HeaderAndDivider_RenderInMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxMenu");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");

		// Act - open the menu
		await toggleButton.ClickAsync();

		// Assert - header and divider are rendered in the menu
		var menuHeader = Page.Locator(".menu-header");
		await Expect(menuHeader).ToBeVisibleAsync(new() { Timeout = MenuAnimationTimeout });
		await Expect(menuHeader).ToHaveTextAsync("Section Header");

		var menuDivider = Page.Locator(".menu-divider");
		await Expect(menuDivider).ToHaveCountAsync(1, new() { Timeout = MenuAnimationTimeout });
	}
}
