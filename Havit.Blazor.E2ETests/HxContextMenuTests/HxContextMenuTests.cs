namespace Havit.Blazor.E2ETests.HxContextMenuTests;

[TestClass]
public class HxContextMenuTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxContextMenu_ClickTrigger_OpensMenu()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var dropdownMenu = Page.Locator(".dropdown-menu");

		// Assert - menu is not visible before clicking
		await Expect(dropdownMenu).Not.ToBeVisibleAsync();

		// Act
		await trigger.ClickAsync();

		// Assert - menu is visible after clicking the trigger
		await Expect(dropdownMenu).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxContextMenu_ClickItem_ExecutesAction()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var clickCount = Page.Locator("[data-testid='click-count']");
		var lastClicked = Page.Locator("[data-testid='last-clicked']");

		// Act - open menu and click the first item
		await trigger.ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Action 1')").ClickAsync();

		// Assert - callback was invoked
		await Expect(clickCount).ToHaveTextAsync("1");
		await Expect(lastClicked).ToHaveTextAsync("Action 1");
	}

	[TestMethod]
	public async Task HxContextMenu_ClickItem_ClosesMenu()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var dropdownMenu = Page.Locator(".dropdown-menu");

		// Act - open menu and click an item
		await trigger.ClickAsync();
		await Expect(dropdownMenu).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Page.Locator(".dropdown-item:has-text('Action 1')").ClickAsync();

		// Assert - menu is closed after clicking an item
		await Expect(dropdownMenu).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxContextMenu_DisabledItem_NotClickable()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxContextMenuTests");

		var trigger = Page.Locator(".hx-context-menu-btn");
		var disabledItem = Page.Locator(".dropdown-item:has-text('Disabled Action')");
		var disabledClickCount = Page.Locator("[data-testid='disabled-click-count']");

		// Act - open menu
		await trigger.ClickAsync();

		// Assert - item has disabled CSS class (Bootstrap Dropdown style)
		await Expect(disabledItem).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("\\bdisabled\\b"));
		await Expect(disabledItem).ToHaveCSSAsync("pointer-events", "none");

		// Act - attempt to click the disabled item (forced click to bypass actionability checks)
		await disabledItem.ClickAsync(new() { Force = true });

		// Assert - callback was NOT invoked for the disabled item
		await Expect(disabledClickCount).ToHaveTextAsync("0");
	}
}
