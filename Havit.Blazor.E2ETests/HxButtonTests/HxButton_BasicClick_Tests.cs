namespace Havit.Blazor.E2ETests.HxButtonTests;

[TestClass]
public class HxButton_BasicClick_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxButton_BasicClick_ShouldIncrementCounter()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxButton_BasicClick");

		var button = Page.Locator("[data-testid='click-button']");
		var clickCount = Page.Locator("[data-testid='click-count']");

		// Assert initial state
		await Expect(clickCount).ToHaveTextAsync("0");

		// Act - click the button
		await button.ClickAsync();

		// Assert - click count increased
		await Expect(clickCount).ToHaveTextAsync("1");
	}

	[TestMethod]
	public async Task HxButton_BasicClick_MultipleClicks_ShouldIncrementCounterEachTime()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxButton_BasicClick");

		var button = Page.Locator("[data-testid='click-button']");
		var clickCount = Page.Locator("[data-testid='click-count']");

		// Act - click the button multiple times
		await button.ClickAsync();
		await button.ClickAsync();
		await button.ClickAsync();

		// Assert - click count reflects all clicks
		await Expect(clickCount).ToHaveTextAsync("3");
	}
}
