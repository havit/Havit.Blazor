namespace Havit.Blazor.E2ETests.HxModalTests;

[TestClass]
public class HxModal_RapidHideShowHide_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxModal_RapidHideShowHide_ModalShouldCloseCompletely()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxModal_RapidHideShowHide");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act 1 - Open the modal
		await showButton.ClickAsync();

		// Wait for the modal to fully show (Bootstrap animates it)
		var hideShowHideButton = Page.Locator("[data-testid='hide-show-hide-button']");
		await Expect(hideShowHideButton).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act 2 - Click the button that rapidly calls HideAsync → ShowAsync → HideAsync
		await hideShowHideButton.ClickAsync();

		// Assert - The modal should be fully hidden after the rapid hide→show→hide sequence
		await Expect(hideShowHideButton).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Additional check: the backdrop should also be gone
		var backdrop = Page.Locator(".modal-backdrop");
		await Expect(backdrop).ToHaveCountAsync(0, new() { Timeout = 5_000 });
	}
}
