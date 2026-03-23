namespace Havit.Blazor.E2ETests.HxMultiSelectTests;

[TestClass]
public class HxMultiSelectTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxMultiSelect_ClickToggle_OpensDropdown()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelectTests");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Act
		await toggle.ClickAsync();

		// Assert - the dropdown menu should be visible
		await Expect(Page.Locator(".dropdown-menu.show")).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxMultiSelect_CheckMultipleOptions_AllSelected()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelectTests");

		var toggle = Page.Locator("[data-testid='toggle']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		// Act - open dropdown and select Alpha and Beta
		await toggle.ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Alpha')").ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Beta')").ClickAsync();

		// Assert - both items are reflected in the selected value display
		await Expect(selectedValueDisplay).ToHaveTextAsync("Alpha, Beta");
	}

	[TestMethod]
	public async Task HxMultiSelect_UncheckOption_RemovedFromValue()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelectTests");

		var toggle = Page.Locator("[data-testid='toggle']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		// Act - open dropdown and select Alpha and Beta
		await toggle.ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Alpha')").ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Beta')").ClickAsync();

		// Verify both are selected
		await Expect(selectedValueDisplay).ToHaveTextAsync("Alpha, Beta");

		// Uncheck Alpha
		await Page.Locator(".dropdown-item:has-text('Alpha')").ClickAsync();

		// Assert - only Beta remains selected
		await Expect(selectedValueDisplay).ToHaveTextAsync("Beta");
	}

	[TestMethod]
	public async Task HxMultiSelect_SelectedItems_ShownAsSummaryText()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelectTests");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Assert initial empty state shows EmptyText
		await Expect(toggle).ToHaveValueAsync("-select items-");

		// Act - open dropdown and select Gamma
		await toggle.ClickAsync();
		await Page.Locator(".dropdown-item:has-text('Gamma')").ClickAsync();

		// Close by clicking outside
		await Page.Locator("[data-testid='outside-click-target']").ClickAsync();

		// Assert - the input reflects the selected item
		await Expect(toggle).ToHaveValueAsync("Gamma");
	}

	[TestMethod]
	public async Task HxMultiSelect_ClickOutside_ClosesDropdown()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelectTests");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Open dropdown
		await toggle.ClickAsync();
		await Expect(Page.Locator(".dropdown-menu.show")).ToBeVisibleAsync();

		// Act - click outside the dropdown
		await Page.Locator("[data-testid='outside-click-target']").ClickAsync();

		// Assert - dropdown is no longer shown
		await Expect(Page.Locator(".dropdown-menu.show")).ToHaveCountAsync(0);
	}
}
