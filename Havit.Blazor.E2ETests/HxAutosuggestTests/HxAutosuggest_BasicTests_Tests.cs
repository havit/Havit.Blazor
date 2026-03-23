namespace Havit.Blazor.E2ETests.HxAutosuggestTests;

[TestClass]
public class HxAutosuggest_BasicTests_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxAutosuggest_TypeText_ShowsSuggestionDropdown()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxAutosuggest_BasicTests");

		var input = Page.Locator("input[type='text']");

		// Act - type text to trigger suggestions
		await input.ClickAsync();
		await input.FillAsync("Al");

		// Assert - the dropdown with a matching suggestion is visible
		var suggestionItem = Page.Locator(".dropdown-item:has-text('Alpha')");
		await Expect(suggestionItem).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxAutosuggest_ClickSuggestion_SetsValue()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxAutosuggest_BasicTests");

		var input = Page.Locator("input[type='text']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		// Act - type to trigger suggestions and click the first matching item
		await input.ClickAsync();
		await input.FillAsync("Alpha");
		await Page.Locator(".dropdown-item:has-text('Alpha')").ClickAsync();

		// Assert - the bound value is set
		await Expect(selectedValueDisplay).ToHaveTextAsync("Alpha");
	}

	[TestMethod]
	public async Task HxAutosuggest_ClearInput_ClearsValue()
	{
		// Arrange - navigate to the test page and select a value
		await NavigateToTestAppAsync("/HxAutosuggest_BasicTests");

		var input = Page.Locator("input[type='text']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		await input.ClickAsync();
		await input.FillAsync("Beta");
		await Page.Locator(".dropdown-item:has-text('Beta')").ClickAsync();
		await Expect(selectedValueDisplay).ToHaveTextAsync("Beta");

		// Act - click the clear button
		var clearButton = Page.Locator(".hx-autosuggest-input-button");
		await clearButton.ClickAsync();

		// Assert - the bound value is cleared
		await Expect(selectedValueDisplay).ToHaveTextAsync("");
		await Expect(input).ToHaveValueAsync("");
	}

	[TestMethod]
	public async Task HxAutosuggest_NoMatch_ShowsNoResultsMessage()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxAutosuggest_BasicTests");

		var input = Page.Locator("input[type='text']");

		// Act - type text that matches no suggestions
		await input.ClickAsync();
		await input.FillAsync("XYZ_NO_MATCH");

		// Assert - the no results message is shown
		var noResultsMessage = Page.Locator("[data-testid='no-results-message']");
		await Expect(noResultsMessage).ToBeVisibleAsync();
	}
}
