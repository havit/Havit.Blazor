namespace Havit.Blazor.E2ETests.HxMessageBoxTests;

[TestClass]
public class HxMessageBox_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxMessageBox_Show_DisplaysTitleAndMessage()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - verify modal is visible with correct title and message
		var modalTitle = Page.Locator(".modal-title");
		await Expect(modalTitle).ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(modalTitle).ToHaveTextAsync("Test Title");

		var modalBody = Page.Locator(".modal-body");
		await Expect(modalBody).ToContainTextAsync("Test Message");
	}

	[TestMethod]
	public async Task HxMessageBox_ClickConfirm_ClosesWithPositiveResult()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		// Wait for the modal to fully show
		var confirmButton = Page.Locator(".modal-footer .btn-primary");
		await Expect(confirmButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the confirm (OK) button
		await confirmButton.ClickAsync();

		// Assert - modal should close and result should be positive
		await Expect(confirmButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		var backdrop = Page.Locator(".modal-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0);

		var result = Page.Locator("[data-testid='result']");
		await Expect(result).ToHaveTextAsync("Positive");
	}

	[TestMethod]
	public async Task HxMessageBox_ClickCancel_ClosesWithNegativeResult()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		// Wait for the modal to fully show
		var cancelButton = Page.Locator(".modal-footer .btn-secondary");
		await Expect(cancelButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the cancel button
		await cancelButton.ClickAsync();

		// Assert - modal should close and result should be negative
		await Expect(cancelButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		var backdrop = Page.Locator(".modal-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0);
		var result = Page.Locator("[data-testid='result']");
		await Expect(result).ToHaveTextAsync("Negative");
	}
}
