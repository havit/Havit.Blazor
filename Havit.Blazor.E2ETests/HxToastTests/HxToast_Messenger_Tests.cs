namespace Havit.Blazor.E2ETests.HxToastTests;

[TestClass]
public class HxToast_Messenger_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxToast_ShowViaMessenger_DisplaysToast()
	{
		// Arrange - Navigate to the messenger test page
		await NavigateToTestAppAsync("/HxToast_Messenger");

		// Act - Click button to show a single toast via messenger
		await Page.Locator("[data-testid='show-single-toast']").ClickAsync();

		// Assert - Verify the toast is visible with correct content
		var toastHeader = Page.Locator(".toast-header strong:has-text('Messenger Toast')");
		await Expect(toastHeader).ToBeVisibleAsync(new() { Timeout = 5_000 });

		var toastBody = Page.Locator(".toast-body:has-text('This toast was shown via messenger.')");
		await Expect(toastBody).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxToast_AutoHide_DisappearsAfterDelay()
	{
		// Arrange - Navigate to the messenger test page
		await NavigateToTestAppAsync("/HxToast_Messenger");

		// Act - Show a toast with 2-second autohide
		await Page.Locator("[data-testid='show-autohide-toast']").ClickAsync();

		// Assert - Verify the toast appears
		var toastBody = Page.Locator(".toast-body:has-text('This toast will auto-hide.')");
		await Expect(toastBody).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Assert - Verify the toast auto-hides (2000ms delay + animation buffer)
		await Expect(toastBody).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxToast_ClickCloseButton_DismissesImmediately()
	{
		// Arrange - Navigate to the messenger test page
		await NavigateToTestAppAsync("/HxToast_Messenger");

		// Act - Show a toast without autohide
		await Page.Locator("[data-testid='show-no-autohide-toast']").ClickAsync();

		// Assert - Verify the toast appears
		var toast = Page.Locator(".hx-toast:has(.toast-body:has-text('Close me with the button.'))");
		await Expect(toast).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act - Click the close button
		var closeButton = toast.Locator(".btn-close");
		await closeButton.ClickAsync();

		// Assert - Verify the toast is dismissed
		await Expect(toast).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxToast_ShowMultiple_AllDisplayCorrectly()
	{
		// Arrange - Navigate to the messenger test page
		await NavigateToTestAppAsync("/HxToast_Messenger");

		// Act - Click button to show three toasts at once
		await Page.Locator("[data-testid='show-multiple-toasts']").ClickAsync();

		// Assert - Verify all three toasts are visible
		var toastOne = Page.Locator(".toast-body:has-text('First toast message.')");
		var toastTwo = Page.Locator(".toast-body:has-text('Second toast message.')");
		var toastThree = Page.Locator(".toast-body:has-text('Third toast message.')");

		await Expect(toastOne).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(toastTwo).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(toastThree).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Assert - Verify three toast elements exist in the container
		var toasts = Page.Locator(".hx-messenger .hx-toast");
		await Expect(toasts).ToHaveCountAsync(3, new() { Timeout = 5_000 });
	}
}
