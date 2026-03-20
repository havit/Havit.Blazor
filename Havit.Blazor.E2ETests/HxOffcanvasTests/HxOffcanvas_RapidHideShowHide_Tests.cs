namespace Havit.Blazor.E2ETests.HxOffcanvasTests;

[TestClass]
public class HxOffcanvas_RapidHideShowHide_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxOffcanvas_RapidHideShowHide_OffcanvasShouldCloseCompletely()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxOffcanvas_RapidHideShowHide");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act 1 - Open the offcanvas
		await showButton.ClickAsync();

		// Wait for the offcanvas to fully show (button inside becomes visible)
		var hideShowHideButton = Page.Locator("[data-testid='hide-show-hide-button']");
		await Expect(hideShowHideButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act 2 - Click the button that rapidly calls HideAsync → ShowAsync → HideAsync
		await hideShowHideButton.ClickAsync();

		// Assert - The offcanvas should be fully hidden after the rapid hide→show→hide sequence
		await Expect(hideShowHideButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: the backdrop should also be gone
		var backdrop = Page.Locator(".offcanvas-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}
}
