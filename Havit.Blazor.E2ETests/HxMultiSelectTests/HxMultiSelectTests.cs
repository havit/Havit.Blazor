namespace Havit.Blazor.E2ETests.HxMultiSelectTests;

public class HxMultiSelectTests : TestAppTestBase
{
	[Fact]
	public async Task HxMultiSelect_ClickToggle_OpensDropdown()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelect");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Act
		await toggle.ClickAsync();

		// Assert - the menu should be visible
		await Expect(Page.Locator(".menu.show")).ToBeVisibleAsync();
	}

	[Fact]
	public async Task HxMultiSelect_CheckMultipleOptions_AllSelected()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelect");

		var toggle = Page.Locator("[data-testid='toggle']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		// Act - open menu and select Alpha and Beta
		await toggle.ClickAsync();
		await Page.Locator(".menu-item:has-text('Alpha')").ClickAsync();
		await Page.Locator(".menu-item:has-text('Beta')").ClickAsync();

		// Assert - both items are reflected in the selected value display
		await Expect(selectedValueDisplay).ToHaveTextAsync("Alpha, Beta");
	}

	[Fact]
	public async Task HxMultiSelect_UncheckOption_RemovedFromValue()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelect");

		var toggle = Page.Locator("[data-testid='toggle']");
		var selectedValueDisplay = Page.Locator("[data-testid='selected-value']");

		// Act - open menu and select Alpha and Beta
		await toggle.ClickAsync();
		await Page.Locator(".menu-item:has-text('Alpha')").ClickAsync();
		await Page.Locator(".menu-item:has-text('Beta')").ClickAsync();

		// Verify both are selected
		await Expect(selectedValueDisplay).ToHaveTextAsync("Alpha, Beta");

		// Uncheck Alpha
		await Page.Locator(".menu-item:has-text('Alpha')").ClickAsync();

		// Assert - only Beta remains selected
		await Expect(selectedValueDisplay).ToHaveTextAsync("Beta");
	}

	[Fact]
	public async Task HxMultiSelect_SelectedItems_ShownAsSummaryText()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelect");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Assert initial empty state shows EmptyText
		await Expect(toggle).ToHaveTextAsync("-select items-");

		// Act - open menu and select Gamma
		await toggle.ClickAsync();
		await Page.Locator(".menu-item:has-text('Gamma')").ClickAsync();

		// Close by clicking outside
		await Page.Locator("[data-testid='outside-click-target']").ClickAsync();

		// Assert - the toggle reflects the selected item
		await Expect(toggle).ToHaveTextAsync("Gamma");
	}

	[Fact]
	public async Task HxMultiSelect_ClickOutside_ClosesDropdown()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxMultiSelect");

		var toggle = Page.Locator("[data-testid='toggle']");

		// Open menu
		await toggle.ClickAsync();
		await Expect(Page.Locator(".menu.show")).ToBeVisibleAsync();

		// Act - click outside the menu
		await Page.Locator("[data-testid='outside-click-target']").ClickAsync();

		// Assert - menu is no longer shown
		await Expect(Page.Locator(".menu.show")).ToHaveCountAsync(0);
	}
}
