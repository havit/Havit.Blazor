namespace Havit.Blazor.E2ETests.HxTabPanelTests;

public class HxTabPanelTests : TestAppTestBase
{
	[Fact]
	public async Task HxTabPanel_ArrowHomeEndKeys_ActivateTabsAndSkipDisabled()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxTabPanel");

		var tabList = Page.GetByRole(AriaRole.Tablist);
		var firstTab = tabList.GetByRole(AriaRole.Tab, new() { Name = "First" });
		var secondTab = tabList.GetByRole(AriaRole.Tab, new() { Name = "Second" });
		var fourthTab = tabList.GetByRole(AriaRole.Tab, new() { Name = "Fourth" });
		var activeTabId = Page.GetByTestId("active-tab-id");

		await Expect(firstTab).ToHaveAttributeAsync("aria-selected", "true");
		await firstTab.FocusAsync();

		// Act & Assert - ArrowRight activates the next tab
		await Page.Keyboard.PressAsync("ArrowRight");
		await Expect(secondTab).ToBeFocusedAsync();
		await Expect(secondTab).ToHaveAttributeAsync("aria-selected", "true");
		await Expect(secondTab).ToHaveAttributeAsync("tabindex", "0");
		await Expect(activeTabId).ToHaveTextAsync("tab2");

		// Act & Assert - ArrowRight skips the disabled tab
		await Page.Keyboard.PressAsync("ArrowRight");
		await Expect(fourthTab).ToBeFocusedAsync();
		await Expect(activeTabId).ToHaveTextAsync("tab4");

		// Act & Assert - ArrowDown wraps around to the first tab
		await Page.Keyboard.PressAsync("ArrowDown");
		await Expect(firstTab).ToBeFocusedAsync();
		await Expect(activeTabId).ToHaveTextAsync("tab1");

		// Act & Assert - ArrowLeft wraps around to the last tab
		await Page.Keyboard.PressAsync("ArrowLeft");
		await Expect(fourthTab).ToBeFocusedAsync();
		await Expect(activeTabId).ToHaveTextAsync("tab4");

		// Act & Assert - Home / End activate the first / last tab
		await Page.Keyboard.PressAsync("Home");
		await Expect(firstTab).ToBeFocusedAsync();
		await Expect(activeTabId).ToHaveTextAsync("tab1");

		await Page.Keyboard.PressAsync("End");
		await Expect(fourthTab).ToBeFocusedAsync();
		await Expect(fourthTab).ToHaveAttributeAsync("aria-selected", "true");
		await Expect(activeTabId).ToHaveTextAsync("tab4");
	}
}
