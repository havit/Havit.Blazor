namespace Havit.Blazor.E2ETests.HxCollapseTests;

public class HxCollapse_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxCollapse_InitialState_ContentHidden()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		// Assert - collapse content should not be visible by default
		var collapseContent = Page.Locator("[data-testid='collapse-inner-content']");
		await Expect(collapseContent).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[Fact]
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

	[Fact]
	public async Task HxCollapse_ClickToggleAgain_HidesContent()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxCollapse");

		var toggleButton = Page.Locator("[data-testid='toggle-button']");
		var collapseContent = Page.Locator("[data-testid='collapse-inner-content']");
		var collapseElement = Page.Locator("[data-testid='collapse-content']");

		// Bootstrap 6 adds the "show" class when the show transition starts and ignores toggling until it ends,
		// so the second click has to wait for the shown event (subscribed before the first click so it cannot be missed).
		await collapseElement.EvaluateAsync("element => element.addEventListener('shown.bs.collapse', () => element.dataset.e2eShown = 'true', { once: true })");

		// Act 1 - click toggle to show content
		await toggleButton.ClickAsync();
		await Expect(collapseContent).ToBeVisibleAsync(new() { Timeout = 5_000 });
		await Expect(collapseElement).ToHaveAttributeAsync("data-e2e-shown", "true", new() { Timeout = 5_000 });

		// Act 2 - click toggle again to hide content
		await toggleButton.ClickAsync();

		// Assert - collapse content should be hidden again
		await Expect(collapseContent).Not.ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[Fact]
	public async Task HxCollapse_ShowAsyncBeforeInitialization_ShowsContent()
	{
		// Arrange & Act - the test page calls ShowAsync() before HxCollapse finishes its JS initialization
		await NavigateToTestAppAsync("/HxCollapse_ShowBeforeInit");

		// Assert - the deferred request is applied once the collapse is initialized
		var content = Page.Locator("[data-testid='show-before-init-content']");
		await Expect(content).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[Fact]
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
