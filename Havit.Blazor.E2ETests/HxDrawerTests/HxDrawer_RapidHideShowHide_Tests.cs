namespace Havit.Blazor.E2ETests.HxDrawerTests;

public class HxDrawer_RapidHideShowHide_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxDrawer_RapidHideShowHide_DrawerShouldCloseCompletely()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDrawer_RapidHideShowHide");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act 1 - Open the drawer
		await showButton.ClickAsync();

		// Wait for the drawer to fully show (button inside becomes visible)
		var hideShowHideButton = Page.Locator("[data-testid='hide-show-hide-button']");
		await Expect(hideShowHideButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act 2 - Click the button that rapidly calls HideAsync → ShowAsync → HideAsync
		await hideShowHideButton.ClickAsync();

		// Assert - The drawer should be fully hidden after the rapid hide→show→hide sequence
		await Expect(hideShowHideButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: no open dialog should remain (the native ::backdrop disappears with it)
		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}
}
