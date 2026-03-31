namespace Havit.Blazor.E2ETests.HxSearchBoxTests;

[TestClass]
public class HxSearchBox_E2E_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxSearchBox_TypeText_ShowsSuggestions()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxSearchBox_E2E");

		var input = Page.Locator("input[inputmode='search']");

		// Act - type text to trigger suggestions
		await input.ClickAsync();
		await input.FillAsync("App");

		// Assert - suggestions dropdown should be visible
		var suggestion = Page.Locator(".dropdown-item:has-text('Apple')");
		await Expect(suggestion).ToBeVisibleAsync(new() { Timeout = 5_000 });
	}

	[TestMethod]
	public async Task HxSearchBox_ClickSuggestion_TriggersAction()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxSearchBox_E2E");

		var input = Page.Locator("input[inputmode='search']");
		var selectedItemDisplay = Page.Locator("[data-testid='selected-item']");

		// Act - type to trigger suggestions and click one
		await input.ClickAsync();
		await input.FillAsync("Ban");
		await Page.Locator(".dropdown-item:has-text('Banana')").ClickAsync();

		// Assert - the OnItemSelected callback should have been called with the selected item
		await Expect(selectedItemDisplay).ToHaveTextAsync("Banana");
	}

	[TestMethod]
	public async Task HxSearchBox_PressEnter_TriggersFreeTextSearch()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxSearchBox_E2E");

		var input = Page.Locator("input[inputmode='search']");
		var textQueryTriggeredDisplay = Page.Locator("[data-testid='text-query-triggered']");

		// Act - type text and press Enter
		await input.ClickAsync();
		await input.FillAsync("Cherry search");
		await input.PressAsync("Enter");

		// Assert - the OnTextQueryTriggered callback should have been fired with the typed text
		await Expect(textQueryTriggeredDisplay).ToHaveTextAsync("Cherry search");
	}

	[TestMethod]
	public async Task HxSearchBox_ClickClear_ResetsInput()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxSearchBox_E2E");

		var input = Page.Locator("input[inputmode='search']");
		var currentTextQueryDisplay = Page.Locator("[data-testid='current-text-query']");

		// Act - type text so that the clear button appears, then click it
		await input.ClickAsync();
		await input.FillAsync("Apple");

		// Wait for the clear button to appear
		var clearButton = Page.Locator(".hx-search-box-input-icon-end[role='button']");
		await Expect(clearButton).ToBeVisibleAsync(new() { Timeout = 5_000 });

		await clearButton.ClickAsync();

		// Assert - the input should be cleared
		await Expect(input).ToHaveValueAsync("");
		await Expect(currentTextQueryDisplay).ToHaveTextAsync("");
	}
}
