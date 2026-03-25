namespace Havit.Blazor.E2ETests.HxDropdownTests;

[TestClass]
public class HxDropdown_Tests : TestAppTestBase
{
	private const int DropdownAnimationTimeout = 5_000;

	[TestMethod]
	public async Task HxDropdown_ClickToggle_OpensMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDropdown");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var dropdownMenu = Page.Locator(".dropdown-menu");

		// Assert - menu is not visible before clicking
		await Expect(dropdownMenu).Not.ToBeVisibleAsync();

		// Act - click the toggle button
		await toggleButton.ClickAsync();

		// Assert - menu is visible after clicking the toggle
		await Expect(dropdownMenu).ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });
	}

	[TestMethod]
	public async Task HxDropdown_ClickOutside_ClosesMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDropdown");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var dropdownMenu = Page.Locator(".dropdown-menu");

		// Act 1 - open the dropdown
		await toggleButton.ClickAsync();
		await Expect(dropdownMenu).ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });

		// Act 2 - click outside the dropdown to close it
		await Page.Locator("[data-testid='outside-click-area']").ClickAsync();

		// Assert - menu is no longer visible
		await Expect(dropdownMenu).Not.ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });
	}

	[TestMethod]
	public async Task HxDropdown_ClickItem_TriggersActionAndCloses()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDropdown");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var actionItem = Page.Locator("[data-testid='action-item']");
		var dropdownMenu = Page.Locator(".dropdown-menu");
		var itemClickCount = Page.Locator("[data-testid='item-click-count']");

		// Act - open the dropdown and click the action item
		await toggleButton.ClickAsync();
		await Expect(dropdownMenu).ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });
		await actionItem.ClickAsync();

		// Assert - handler was called and dropdown closed
		await Expect(itemClickCount).ToHaveTextAsync("1");
		await Expect(dropdownMenu).Not.ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });
	}

	[TestMethod]
	public async Task HxDropdown_HeaderAndDivider_RenderInMenu()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDropdown");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");

		// Act - open the dropdown
		await toggleButton.ClickAsync();

		// Assert - header and divider are rendered in the dropdown menu
		var dropdownHeader = Page.Locator(".dropdown-header");
		await Expect(dropdownHeader).ToBeVisibleAsync(new() { Timeout = DropdownAnimationTimeout });
		await Expect(dropdownHeader).ToHaveTextAsync("Section Header");

		var dropdownDivider = Page.Locator(".dropdown-divider");
		await Expect(dropdownDivider).ToHaveCountAsync(1, new() { Timeout = DropdownAnimationTimeout });
	}
}
