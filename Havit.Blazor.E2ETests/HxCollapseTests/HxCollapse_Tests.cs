namespace Havit.Blazor.E2ETests.HxCollapseTests;

[TestClass]
public class HxCollapse_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxCollapse_InitialState_ContentHidden()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		// Assert - collapse content should not be visible by default
		var collapseContent = Page.Locator("[data-testid='collapse-inner-content']");
		await Expect(collapseContent).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxCollapse_ClickToggle_ShowsContent()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var collapseContent = Page.Locator("[data-testid='collapse-inner-content']");

		// Act - click toggle to show content
		await toggleButton.ClickAsync();

		// Assert - collapse content should be visible
		await Expect(collapseContent).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxCollapse_ClickToggleAgain_HidesContent()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var collapseContent = Page.Locator("[data-testid='collapse-inner-content']");

		// Act 1 - click toggle to show content
		await toggleButton.ClickAsync();
		await Expect(collapseContent).ToBeVisibleAsync(new() { Timeout = 5_000 });

		// Act 2 - click toggle again to hide content
		await toggleButton.ClickAsync();

		// Assert - collapse content should be hidden again
		await Expect(collapseContent).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxCollapse_MultipleShowCalls_DoesNotBreak()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		var multipleShowButton = Page.Locator("[data-testid='multiple-show-button']");
		var counterValue = Page.Locator("[data-testid='counter-value']");
		var incrementButton = Page.Locator("[data-testid='increment-button']");

		// Verify initial state - collapse is initially expanded, counter is 0
		await Expect(counterValue).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(counterValue).ToHaveTextAsync("0");

		// Act 1 - call ShowAsync() multiple times while already shown
		await multipleShowButton.ClickAsync();

		// Act 2 - verify ShouldRender is not broken by incrementing the counter
		await incrementButton.ClickAsync();

		// Assert - counter should have incremented, proving ShouldRender still works
		await Expect(counterValue).ToHaveTextAsync("1", new() { Timeout = 5_000 });
	}
}
