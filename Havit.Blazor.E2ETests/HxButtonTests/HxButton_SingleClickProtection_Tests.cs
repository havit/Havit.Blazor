namespace Havit.Blazor.E2ETests.HxButtonTests;

[TestClass]
public class HxButton_SingleClickProtection_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxButton_AsyncHandler_DisablesButtonDuringExecution()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxButton_SingleClickProtection");

		var button = Page.Locator("[data-testid='async-button']");
		var clickCount = Page.Locator("[data-testid='click-count']");

		// Verify initial state
		await Expect(clickCount).ToHaveTextAsync("0");
		await Expect(button).ToBeEnabledAsync();

		// Act — click the button to start the async handler
		await button.ClickAsync();

		// Assert — button should become disabled while the async handler is running
		await Expect(button).ToBeDisabledAsync(new() { Timeout = 5_000 });

		// The handler was invoked exactly once
		await Expect(clickCount).ToHaveTextAsync("1");
	}

	[TestMethod]
	public async Task HxButton_DoubleClick_InvokesHandlerOnlyOnce()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxButton_SingleClickProtection");

		var button = Page.Locator("[data-testid='async-button']");
		var clickCount = Page.Locator("[data-testid='click-count']");

		// Act — click the button twice in quick succession
		await button.ClickAsync();
		await button.ClickAsync(new() { Force = true }); // Force bypasses disabled check on the Playwright side

		// Assert — after the handler completes, the click count should still be 1
		// Wait for the async handler to finish (2s delay + some margin)
		await Expect(button).ToBeEnabledAsync(new() { Timeout = 10_000 });
		await Expect(clickCount).ToHaveTextAsync("1");
	}

	[TestMethod]
	public async Task HxButton_AfterAsyncComplete_ButtonReenabled()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxButton_SingleClickProtection");

		var button = Page.Locator("[data-testid='async-button']");

		// Act — click and wait for the async handler to complete
		await button.ClickAsync();
		await Expect(button).ToBeDisabledAsync(new() { Timeout = 5_000 });

		// Assert — after async handler completes, button is re-enabled
		await Expect(button).ToBeEnabledAsync(new() { Timeout = 10_000 });
	}
}
