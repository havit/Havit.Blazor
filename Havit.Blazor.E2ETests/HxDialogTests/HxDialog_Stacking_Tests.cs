namespace Havit.Blazor.E2ETests.HxDialogTests;

public class HxDialog_Stacking_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxDialog_ToastRaisedWhileDialogOpen_IsShownAboveTheDialog()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDialog_Stacking");
		await Page.Locator("[data-testid='open-button']").ClickAsync();
		var dialog = Page.Locator("dialog.dialog:has-text('Parent Dialog')");
		await Expect(dialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act
		await Page.Locator("[data-testid='toast-button']").ClickAsync();

		// Assert - the toast is visible and its container is in the top layer (shown after the dialog, i.e. above its ::backdrop)
		var toast = Page.Locator(".toast:has-text('This toast is shown while a modal dialog is open.')");
		await Expect(toast).ToBeVisibleAsync(new() { Timeout = 5_000 });

		var isTopmost = await toast.EvaluateAsync<bool>("toast => toast.closest('.hx-toast-container').matches(':popover-open')");
		Assert.True(isTopmost, "The toast container should be in the top layer, above the modal dialog.");
	}

	[Fact]
	public async Task HxDialog_MessageBoxFromDialog_StacksAndKeepsParentOpen()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxDialog_Stacking");
		await Page.Locator("[data-testid='open-button']").ClickAsync();
		var parentDialog = Page.Locator("dialog.dialog:has-text('Parent Dialog')");
		await Expect(parentDialog).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - open the confirmation from the dialog
		await Page.Locator("[data-testid='confirm-button']").ClickAsync();

		// Assert - both dialogs are open
		var messageBox = Page.Locator("dialog.dialog:has-text('Do you confirm?')");
		await Expect(messageBox).ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(Page.Locator("dialog[open]")).ToHaveCountAsync(2);

		// Act - cancel the confirmation
		await messageBox.Locator(".dialog-footer .btn.theme-secondary").ClickAsync();

		// Assert - the parent dialog stays open, the page stays scroll-locked
		await Expect(messageBox).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(Page.Locator("[data-testid='confirm-result']")).ToHaveTextAsync("canceled");
		await Expect(parentDialog).ToBeVisibleAsync();
		await Expect(Page.Locator("html.dialog-open")).ToHaveCountAsync(1);
	}
}
