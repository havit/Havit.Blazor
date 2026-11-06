namespace Havit.Blazor.E2ETests.HxContextMenuTests;

public class HxContextMenuTests : TestAppTestBase
{
	[Fact]
	public async Task HxContextMenu_ClickTrigger_OpensMenu()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var menu = Page.Locator(".menu");

		// Assert - menu is not visible before clicking
		await Expect(menu).Not.ToBeVisibleAsync();

		// Act
		await trigger.ClickAsync();

		// Assert - menu is visible after clicking the trigger
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxContextMenu_ClickItem_ExecutesAction()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var clickCount = Page.Locator("[data-testid='click-count']");
		var lastClicked = Page.Locator("[data-testid='last-clicked']");

		// Act - open menu and click the first item
		await trigger.ClickAsync();
		await Page.Locator(".menu-item:has-text('Action 1')").ClickAsync();

		// Assert - callback was invoked
		await Expect(clickCount).ToHaveTextAsync("1");
		await Expect(lastClicked).ToHaveTextAsync("Action 1");
	}

	[Fact]
	public async Task HxContextMenu_ClickItem_ClosesMenu()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var menu = Page.Locator(".menu");

		// Act - open menu and click an item
		await trigger.ClickAsync();
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Page.Locator(".menu-item:has-text('Action 1')").ClickAsync();

		// Assert - menu is closed after clicking an item
		await Expect(menu).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxContextMenu_DisabledItem_NotClickable()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var disabledItem = Page.Locator(".menu-item:has-text('Disabled Action')");
		var disabledClickCount = Page.Locator("[data-testid='disabled-click-count']");

		// Act - open menu
		await trigger.ClickAsync();

		// Assert - item has disabled CSS class (Bootstrap Menu style)
		await Expect(disabledItem).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("\\bdisabled\\b"));
		await Expect(disabledItem).ToHaveCSSAsync("pointer-events", "none");

		// Act - attempt to click the disabled item (forced click to bypass actionability checks)
		await disabledItem.ClickAsync(new() { Force = true });

		// Assert - callback was NOT invoked for the disabled item
		await Expect(disabledClickCount).ToHaveTextAsync("0");
	}

	[Fact]
	public async Task HxContextMenu_Keyboard_OpensMenuAndActivatesItem()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var menu = Page.Locator(".menu");
		var firstItem = Page.Locator(".menu-item:has-text('Action 1')");
		var clickCount = Page.Locator("[data-testid='click-count']");

		// Act - open the menu from the keyboard
		await trigger.FocusAsync();
		await Page.Keyboard.PressAsync("ArrowDown");

		// Assert - the menu is open and the first item is focused
		await Expect(menu).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(firstItem).ToBeFocusedAsync();

		// Act - activate the item
		await Page.Keyboard.PressAsync("Enter");

		// Assert
		await Expect(clickCount).ToHaveTextAsync("1");
	}
}
