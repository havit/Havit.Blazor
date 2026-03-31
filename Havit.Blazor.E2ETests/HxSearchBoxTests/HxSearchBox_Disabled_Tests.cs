namespace Havit.Blazor.E2ETests.HxSearchBoxTests;

[TestClass]
public class HxSearchBox_Disabled_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxSearchBox_DisabledTrue_InputIsDisabled()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxSearchBox_Disabled");

		// Act & Assert — the search input should be present but disabled
		var input = Page.Locator("input[inputmode='search']");
		await Expect(input).ToBeDisabledAsync();
	}

	[TestMethod]
	public async Task HxSearchBox_DisabledTrue_CannotType()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxSearchBox_Disabled");

		var input = Page.Locator("input[inputmode='search']");

		// Act — try to type text (force to bypass Playwright's own disabled check)
		await input.FillAsync("Test", new() { Force = true });

		// Assert — the input should still be empty (disabled inputs don't accept fill in browser)
		await Expect(input).ToHaveValueAsync("");
	}
}
