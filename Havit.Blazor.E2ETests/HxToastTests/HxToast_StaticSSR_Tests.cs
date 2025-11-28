namespace Havit.Blazor.E2ETests.HxToastTests;

[TestClass]
public class HxToast_StaticSSR_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxToast_StaticSSR_BasicToast_DisplaysAndAutoHidesAfter2Seconds()
	{
		// Arrange & Act - Navigate to the HxToast StaticSSR test page
		await NavigateToTestAppAsync("/HxToast_StaticSSR");

		// Wait for the page to be loaded
		await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);

		// Assert - Verify the toast is visible with correct content
		var toastHeader = Page.Locator(".toast-header:has-text('HeaderText')");
		await Expect(toastHeader).ToBeVisibleAsync();

		var toastBody = Page.Locator(".toast-body:has-text('ContentText')");
		await Expect(toastBody).ToBeVisibleAsync();

		// Wait for the toast to auto-hide (AutohideDelay is 2000ms, add buffer for animations)
		await Page.WaitForTimeoutAsync(2500);

		// Assert - Verify the toast is no longer visible
		await Expect(toastHeader).Not.ToBeVisibleAsync();
		await Expect(toastBody).Not.ToBeVisibleAsync();
	}
}
