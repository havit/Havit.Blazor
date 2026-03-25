namespace Havit.Blazor.E2ETests.HxModalTests;

[TestClass]
public class HxModal_Basic_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxModal_Open_ShowsModalDialog()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxModal_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Act - Open the modal programmatically
		await openButton.ClickAsync();

		// Assert - The modal dialog should be visible
		var modalDialog = Page.Locator(".modal-dialog");
		await Expect(modalDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxModal_ClickCloseButton_ClosesModal()
	{
		// Arrange - navigate to the test page and open the modal
		await NavigateToTestAppAsync("/HxModal_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");
		await openButton.ClickAsync();

		var modalDialog = Page.Locator(".modal-dialog");
		await Expect(modalDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - Click the close button (×) in the modal header
		var closeButton = Page.Locator(".modal-header .btn-close");
		await closeButton.ClickAsync();

		// Assert - The modal should be hidden
		await Expect(modalDialog).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: the backdrop should also be gone
		var backdrop = Page.Locator(".modal-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxModal_ClickBackdrop_ClosesModal()
	{
		// Arrange - navigate to the test page and open the modal
		await NavigateToTestAppAsync("/HxModal_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Set up a promise that resolves when Bootstrap's shown.bs.modal fires.
		// Must be registered before opening the modal to avoid missing the event.
		var shownTask = Page.EvaluateHandleAsync(@"() => new Promise(resolve => {
			document.querySelector('.hx-modal').addEventListener('shown.bs.modal', resolve, { once: true });
		})");

		await openButton.ClickAsync();

		var modalDialog = Page.Locator(".modal-dialog");
		await Expect(modalDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Wait for the Bootstrap show-transition to fully complete (shown.bs.modal event).
		// During the fade-in transition, Bootstrap sets _isTransitioning = true
		// and ignores any hide() calls, so clicking the backdrop too early has no effect.
		await shownTask;

		// Act - Click the backdrop (the .modal element outside the dialog)
		// Clicking near position (10, 10) of the modal overlay targets the backdrop area
		var modalOverlay = Page.Locator(".modal.show");
		await modalOverlay.ClickAsync(new() { Position = new() { X = 10, Y = 10 } });

		// Assert - The modal should be hidden
		await Expect(modalDialog).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: the backdrop should also be gone
		var backdrop = Page.Locator(".modal-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxModal_TitleBodyFooter_RenderCorrectly()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxModal_Basic");

		var openButton = Page.Locator("[data-testid='open-button']");

		// Act - Open the modal
		await openButton.ClickAsync();

		var modalDialog = Page.Locator(".modal-dialog");
		await Expect(modalDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Assert - Title renders correctly
		var modalTitle = Page.Locator(".modal-title");
		await Expect(modalTitle).ToBeVisibleAsync();
		await Expect(modalTitle).ToHaveTextAsync("Test Modal Title");

		// Assert - Body content renders correctly
		var bodyContent = Page.Locator("[data-testid='modal-body-content']");
		await Expect(bodyContent).ToBeVisibleAsync();
		await Expect(bodyContent).ToHaveTextAsync("This is the modal body content.");

		// Assert - Footer content renders correctly
		var footerContent = Page.Locator("[data-testid='modal-footer-content']");
		await Expect(footerContent).ToBeVisibleAsync();
		await Expect(footerContent).ToHaveTextAsync("This is the modal footer content.");
	}
}
