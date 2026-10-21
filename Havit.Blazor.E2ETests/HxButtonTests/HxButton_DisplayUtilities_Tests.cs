namespace Havit.Blazor.E2ETests.HxButtonTests;

/// <summary>
/// Bootstrap 6 utilities are layered and have no !important, so any unlayered button CSS (e.g. display:inline-flex) would beat them.
/// </summary>
public class HxButton_DisplayUtilities_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxButton_DisplayNoneUtilities_HideButtonOnWideViewport()
	{
		// Arrange
		await Page.SetViewportSizeAsync(1200, 800);

		// Act
		await NavigateToTestAppAsync("/HxButton_DisplayUtilities");

		// Assert
		await Expect(Page.Locator("[data-testid='plain-button']")).ToBeVisibleAsync();
		await Expect(Page.Locator("[data-testid='d-none-button']")).ToBeHiddenAsync();
		await Expect(Page.Locator("[data-testid='neutral-d-none-button']")).ToBeHiddenAsync();
		await Expect(Page.Locator("[data-testid='lg-d-none-button']")).ToBeHiddenAsync();
	}

	[Fact]
	public async Task HxButton_ResponsiveDisplayNone_ShowsButtonBelowBreakpoint()
	{
		// Arrange
		await Page.SetViewportSizeAsync(800, 800);

		// Act
		await NavigateToTestAppAsync("/HxButton_DisplayUtilities");

		// Assert
		await Expect(Page.Locator("[data-testid='lg-d-none-button']")).ToBeVisibleAsync();
		await Expect(Page.Locator("[data-testid='d-none-button']")).ToBeHiddenAsync();
	}
}
