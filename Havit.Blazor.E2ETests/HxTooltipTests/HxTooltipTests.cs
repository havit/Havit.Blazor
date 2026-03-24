namespace Havit.Blazor.E2ETests.HxTooltipTests;

[TestClass]
public class HxTooltipTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxTooltip_Hover_ShowsTooltip()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxTooltip_HoverShowHide");

		// Act - hover over the tooltip trigger element
		var trigger = Page.Locator("[data-testid='tooltip-trigger']");
		await trigger.HoverAsync();

		// Assert - tooltip should be visible
		await Expect(Page.Locator(".tooltip")).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxTooltip_Content_ShowsExpectedText()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxTooltip_HoverShowHide");

		// Act - hover over the tooltip trigger element
		var trigger = Page.Locator("[data-testid='tooltip-trigger']");
		await trigger.HoverAsync();

		// Assert - tooltip should show the expected text
		await Expect(Page.Locator(".tooltip-inner")).ToHaveTextAsync("Hello, Tooltip!", new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxTooltip_MouseLeave_HidesTooltip()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxTooltip_HoverShowHide");

		var trigger = Page.Locator("[data-testid='tooltip-trigger']");
		await trigger.HoverAsync();

		// Wait for tooltip to appear
		await Expect(Page.Locator(".tooltip")).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act - move mouse away from the trigger (far enough to leave both trigger and tooltip)
		var box = await trigger.BoundingBoxAsync();
		await Page.Mouse.MoveAsync(box.X + box.Width + 200, box.Y + box.Height + 200);

		// Assert - tooltip should be hidden
		await Expect(Page.Locator(".tooltip")).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxTooltip_EmptyText_DoesNotThrow()
	{
		// Arrange - collect any page-level JS errors
		var pageErrors = new List<string>();
		Page.PageError += (_, error) => pageErrors.Add(error);

		await NavigateToTestAppAsync("/HxTooltip_EmptyText");

		// Act - hover over the empty-text tooltip trigger
		var trigger = Page.Locator("[data-testid='empty-tooltip-trigger']");
		await trigger.HoverAsync();

		// Assert - no tooltip should appear for empty text
		await Expect(Page.Locator(".tooltip")).Not.ToBeVisibleAsync(new() { Timeout = 2_000 });

		// Assert - no JavaScript errors should have occurred
		Assert.IsEmpty(pageErrors, $"Unexpected page errors: {string.Join(", ", pageErrors)}");
	}
}
