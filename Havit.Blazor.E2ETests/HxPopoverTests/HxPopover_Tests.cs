namespace Havit.Blazor.E2ETests.HxPopoverTests;

[TestClass]
public class HxPopover_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxPopover_ClickTrigger_ShowsPopover()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPopover");

		var trigger = Page.Locator("[data-testid='popover-trigger']");
		var popover = Page.Locator(".popover");

		// Act - Click the trigger to show the popover
		await trigger.ClickAsync();

		// Assert - Popover should be visible
		await Expect(popover).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxPopover_Content_DisplaysTitleAndBody()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPopover");

		var trigger = Page.Locator("[data-testid='popover-trigger']");

		// Act - Click the trigger to show the popover
		await trigger.ClickAsync();

		// Assert - Popover title and body content are visible
		var popoverTitle = Page.Locator(".popover-header");
		await Expect(popoverTitle).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(popoverTitle).ToContainTextAsync("Test Popover Title");

		var popoverBody = Page.Locator(".popover-body");
		await Expect(popoverBody).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(popoverBody).ToContainTextAsync("Test popover body content");
	}

	[TestMethod]
	public async Task HxPopover_ClickOutside_HidesPopover()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxPopover");

		var trigger = Page.Locator("[data-testid='popover-trigger']");
		var popover = Page.Locator(".popover");

		// Act - Click the trigger to show the popover
		await trigger.ClickAsync();
		await Expect(popover).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act - Click outside to hide the popover
		var outsideArea = Page.Locator("[data-testid='outside-area']");
		await outsideArea.ClickAsync();

		// Assert - Popover should no longer be visible
		await Expect(popover).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}
}
