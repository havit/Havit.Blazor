namespace Havit.Blazor.E2ETests.HxMessageBoxTests;

public class HxMessageBox_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxMessageBox_Show_DisplaysTitleAndMessage()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - verify dialog is visible with correct title and message
		var dialogTitle = Page.Locator(".dialog-title");
		await Expect(dialogTitle).ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(dialogTitle).ToHaveTextAsync("Test Title");

		var dialogBody = Page.Locator(".dialog-body");
		await Expect(dialogBody).ToContainTextAsync("Test Message");
	}

	[Fact]
	public async Task HxMessageBox_ClickConfirm_ClosesWithPositiveResult()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		// Wait for the dialog to fully show
		var confirmButton = Page.Locator(".dialog-footer .btn.theme-primary");
		await Expect(confirmButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the confirm (OK) button
		await confirmButton.ClickAsync();

		// Assert - dialog should close and result should be positive
		await Expect(confirmButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0);

		var result = Page.Locator("[data-testid='result']");
		await Expect(result).ToHaveTextAsync("Positive");
	}

	[Fact]
	public async Task HxMessageBox_ClickCancel_ClosesWithNegativeResult()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMessageBox");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		// Wait for the dialog to fully show
		var cancelButton = Page.Locator(".dialog-footer .btn.theme-secondary");
		await Expect(cancelButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the cancel button
		await cancelButton.ClickAsync();

		// Assert - dialog should close and result should be negative
		await Expect(cancelButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		var openDialog = Page.Locator("dialog[open]");
		await Expect(openDialog).ToHaveCountAsync(0);
		var result = Page.Locator("[data-testid='result']");
		await Expect(result).ToHaveTextAsync("Negative");
	}
}
