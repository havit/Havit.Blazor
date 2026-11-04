namespace Havit.Blazor.E2ETests.HxDialogTests;

public class HxDialog_Basic_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxDialog_Open_ShowsDialog()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDialog_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Act - Open the dialog programmatically
		await openButton.ClickAsync();

		// Assert - The dialog should be visible
		var dialog = Page.Locator("dialog.dialog");
		await Expect(dialog).ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[Fact]
	public async Task HxDialog_ClickCloseButton_ClosesDialog()
	{
		// Arrange - navigate to the test page and open the dialog
		await NavigateToTestAppAsync("/HxDialog_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");
		await openButton.ClickAsync();

		var dialog = Page.Locator("dialog.dialog");
		await Expect(dialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - Click the close button (×) in the dialog header
		var closeButton = Page.Locator(".dialog-header .btn-close");
		await closeButton.ClickAsync();

		// Assert - The dialog should be hidden
		await Expect(dialog).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: no dialog should remain open (the native backdrop disappears with it)
		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxDialog_ClickBackdrop_ClosesDialog()
	{
		// Arrange - navigate to the test page and open the dialog
		await NavigateToTestAppAsync("/HxDialog_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Set up a promise that resolves when Bootstrap's shown.bs.dialog fires.
		// Must be registered before opening the dialog to avoid missing the event.
		var shownTask = Page.EvaluateHandleAsync(@"() => new Promise(resolve => {
			document.querySelector('.hx-dialog').addEventListener('shown.bs.dialog', resolve, { once: true });
		})");

		await openButton.ClickAsync();

		var dialog = Page.Locator("dialog.dialog");
		await Expect(dialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Wait for the Bootstrap show-transition to fully complete (shown.bs.dialog event).
		// During the fade-in transition, Bootstrap sets _isTransitioning = true
		// and ignores any hide() calls, so clicking the backdrop too early has no effect.
		await shownTask;

		// Act - Click the backdrop (the native ::backdrop outside the dialog;
		// clicks on it are delivered to the dialog element with coordinates outside its bounds)
		await Page.Mouse.ClickAsync(10, 10);

		// Assert - The dialog should be hidden
		await Expect(dialog).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: no dialog should remain open (the native backdrop disappears with it)
		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxDialog_TitleBodyFooter_RenderCorrectly()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxDialog_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Act - Open the dialog
		await openButton.ClickAsync();

		var dialog = Page.Locator("dialog.dialog");
		await Expect(dialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Assert - Title renders correctly
		var dialogTitle = Page.Locator(".dialog-title");
		await Expect(dialogTitle).ToBeVisibleAsync();
		await Expect(dialogTitle).ToHaveTextAsync("Test Dialog Title");

		// Assert - Body content renders correctly
		var bodyContent = Page.Locator("[data-testid='dialog-body-content']");
		await Expect(bodyContent).ToBeVisibleAsync();
		await Expect(bodyContent).ToHaveTextAsync("This is the dialog body content.");

		// Assert - Footer content renders correctly
		var footerContent = Page.Locator("[data-testid='dialog-footer-content']");
		await Expect(footerContent).ToBeVisibleAsync();
		await Expect(footerContent).ToHaveTextAsync("This is the dialog footer content.");
	}
}
