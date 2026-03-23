namespace Havit.Blazor.E2ETests.HxCollapseTests;

[TestClass]
public class HxCollapse_BasicToggle_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxCollapse_BasicToggle_InitiallyHidden_ShouldShowAfterToggle()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse_BasicToggle");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var collapseContent = Page.Locator("[data-testid='collapse-content']");

		// Assert - initially hidden
		await Expect(collapseContent).Not.ToBeVisibleAsync();

		// Act - click the toggle button to show the content
		await toggleButton.ClickAsync();

		// Assert - content is now visible
		await Expect(collapseContent).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxCollapse_BasicToggle_ShownContent_ShouldHideAfterSecondToggle()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse_BasicToggle");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var collapseContent = Page.Locator("[data-testid='collapse-content']");

		// Act 1 - show the content
		await toggleButton.ClickAsync();
		await Expect(collapseContent).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act 2 - hide the content
		await toggleButton.ClickAsync();

		// Assert - content is hidden again
		await Expect(collapseContent).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}
}
