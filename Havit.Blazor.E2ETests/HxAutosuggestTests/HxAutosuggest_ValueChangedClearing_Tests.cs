namespace Havit.Blazor.E2ETests.HxAutosuggestTests;

[TestClass]
public class HxAutosuggest_ValueChangedClearing_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxAutosuggest_ValueChangedClearing_InputIsClearedOnSecondSelection()
	{
		// Arrange - navigate to the test page
		await NavigateToTestAppAsync("/HxAutosuggest_ValueChangedClearing");

		var input = Page.Locator("input[type='text']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");
		var valueChangedCountDisplay = Page.Locator("[data-testid='value-changed-count']");

		// Act 1 - Type to trigger suggestions and select the first item
		await input.ClickAsync();
		await input.FillAsync("Alpha");
		await Page.Locator(".dropdown-item:has-text('Alpha')").ClickAsync();

		// Assert 1 - The input should be cleared because the ValueChanged handler sets Value to null
		await Expect(selectedValueDisplay).ToHaveTextAsync("");
		await Expect(valueChangedCountDisplay).ToHaveTextAsync("1");
		await Expect(input).ToHaveValueAsync("");

		// Act 2 - Select a second item (this is the scenario that was broken)
		await input.ClickAsync();
		await input.FillAsync("Beta");
		await Page.Locator(".dropdown-item:has-text('Beta')").ClickAsync();

		// Assert 2 - The input should ALSO be cleared because the ValueChanged handler sets Value to null
		await Expect(selectedValueDisplay).ToHaveTextAsync("");
		await Expect(valueChangedCountDisplay).ToHaveTextAsync("2");
		await Expect(input).ToHaveValueAsync("");
	}
}
