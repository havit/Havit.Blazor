namespace Havit.Blazor.E2ETests.HxGridTests;

[TestClass]
public class HxGrid_DefaultSortingAfterStateReset_Tests : TestAppTestBase
{
	// Repro for #1413: HxGrid applies the default sorting (IsDefaultSortColumn) only on the first render.
	// After the bound CurrentUserState is reset to a state with no sorting, the grid used to issue a
	// DataProvider request with empty sorting instead of falling back to the default sort column.
	[TestMethod]
	public async Task HxGrid_CurrentUserStateReset_KeepsDefaultSortingInDataProviderRequest()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxGrid_DefaultSortingAfterStateReset");

		var sortingLog = Page.Locator("[data-testid='sorting-log']");

		// Assert — the initial load sorts by the default sort column ("Phone")
		await Expect(sortingLog).ToHaveTextAsync("Phone", new() { Timeout = 10_000 });

		// Act — reset the grid's CurrentUserState to "no sorting"
		await Page.Locator("[data-testid='reset-userstate-button']").ClickAsync();

		// Assert — the reload triggered by the reset must STILL sort by the default sort column,
		// i.e. the second DataProvider request must be "Phone" and not "(empty)".
		await Expect(sortingLog).ToHaveTextAsync("Phone | Phone", new() { Timeout = 10_000 });
	}
}
