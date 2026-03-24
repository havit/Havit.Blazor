namespace Havit.Blazor.E2ETests.HxAccordionTests;

[TestClass]
public class HxAccordionTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxAccordion_Render_AllItemsCollapsed()
	{
		// Arrange & Act - Navigate to the HxAccordion test page
		await NavigateToTestAppAsync("/HxAccordion");

		// Assert - All accordion bodies should be hidden (not visible) by default
		var body1 = Page.Locator("[data-testid='body-1']");
		var body2 = Page.Locator("[data-testid='body-2']");
		var body3 = Page.Locator("[data-testid='body-3']");

		await Expect(body1).Not.ToBeVisibleAsync();
		await Expect(body2).Not.ToBeVisibleAsync();
		await Expect(body3).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxAccordion_ClickHeader_ExpandsContent()
	{
		// Arrange - Navigate to the HxAccordion test page
		await NavigateToTestAppAsync("/HxAccordion");

		var header1 = Page.Locator("[data-testid='header-1']");
		var body1 = Page.Locator("[data-testid='body-1']");

		// Assert - Initially collapsed
		await Expect(body1).Not.ToBeVisibleAsync();

		// Act - Click the header to expand
		await header1.ClickAsync();

		// Assert - Content should now be visible
		await Expect(body1).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxAccordion_ClickExpandedHeader_CollapsesContent()
	{
		// Arrange - Navigate to the HxAccordion test page
		await NavigateToTestAppAsync("/HxAccordion");

		var header1 = Page.Locator("[data-testid='header-1']");
		var body1 = Page.Locator("[data-testid='body-1']");

		// Act 1 - Expand the item
		await header1.ClickAsync();
		await Expect(body1).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act 2 - Click the header again to collapse
		await header1.ClickAsync();

		// Assert - Content should be hidden again
		await Expect(body1).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxAccordion_StayOpenFalse_ClosesOtherItem()
	{
		// Arrange - Navigate to the HxAccordion test page
		await NavigateToTestAppAsync("/HxAccordion");

		var header1 = Page.Locator("[data-testid='header-1']");
		var body1 = Page.Locator("[data-testid='body-1']");
		var header2 = Page.Locator("[data-testid='header-2']");
		var body2 = Page.Locator("[data-testid='body-2']");

		// Act 1 - Expand item 1
		await header1.ClickAsync();
		await Expect(body1).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act 2 - Expand item 2 (should collapse item 1)
		await header2.ClickAsync();
		await Expect(body2).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Assert - Item 1 should now be collapsed
		await Expect(body1).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxAccordion_StayOpenTrue_AllowsMultipleOpen()
	{
		// Arrange - Navigate to the HxAccordion StayOpen test page
		await NavigateToTestAppAsync("/HxAccordion_StayOpen");

		var stayOpenHeader1 = Page.Locator("[data-testid='stayopen-header-1']");
		var stayOpenBody1 = Page.Locator("[data-testid='stayopen-body-1']");
		var stayOpenHeader2 = Page.Locator("[data-testid='stayopen-header-2']");
		var stayOpenBody2 = Page.Locator("[data-testid='stayopen-body-2']");

		// Act 1 - Expand item 1
		await stayOpenHeader1.ClickAsync();
		await Expect(stayOpenBody1).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act 2 - Expand item 2
		await stayOpenHeader2.ClickAsync();
		await Expect(stayOpenBody2).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Assert - Both items should remain open simultaneously
		await Expect(stayOpenBody1).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(stayOpenBody2).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}
}
