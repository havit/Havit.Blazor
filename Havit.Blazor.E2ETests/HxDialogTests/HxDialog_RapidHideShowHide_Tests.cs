namespace Havit.Blazor.E2ETests.HxDialogTests;

public class HxDialog_RapidHideShowHide_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxDialog_RapidHideShowHide_DialogShouldCloseCompletely()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDialog_RapidHideShowHide");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act 1 - Open the dialog
		await showButton.ClickAsync();

		// Wait for the dialog to fully show (Bootstrap animates it)
		var hideShowHideButton = Page.Locator("[data-testid='hide-show-hide-button']");
		await Expect(hideShowHideButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act 2 - Click the button that rapidly calls HideAsync → ShowAsync → HideAsync
		await hideShowHideButton.ClickAsync();

		// Assert - The dialog should be fully hidden after the rapid hide→show→hide sequence
		await Expect(hideShowHideButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: no dialog should remain open (the native backdrop disappears with it)
		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}
}
