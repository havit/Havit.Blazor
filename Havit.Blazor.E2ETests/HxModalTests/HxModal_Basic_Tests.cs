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
		await openButton.ClickAsync();

		var modalDialog = Page.Locator(".modal-dialog");
		await Expect(modalDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - Click the backdrop (the .modal element outside the dialog)
		// Clicking near position (1, 1) of the modal overlay targets the backdrop area
		var modalOverlay = Page.Locator(".modal.show");
		await modalOverlay.ClickAsync(new() { Position = new() { X = 1, Y = 1 } });

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
