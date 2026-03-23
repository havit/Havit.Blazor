namespace Havit.Blazor.E2ETests.HxSidebarTests;

[TestClass]
public class HxSidebarTests : TestAppTestBase
{
	/// <summary>
	/// Verifies that the HxSidebar correctly renders the brand, items, and footer.
	/// </summary>
	[TestMethod]
	public async Task HxSidebar_Render_ShowsItemsAndBrand()
	{
		await NavigateToTestAppAsync("/HxSidebarTests");

		// Brand is visible
		await Expect(Page.Locator(".hx-sidebar-brand")).ToBeVisibleAsync();
		await Expect(Page.GetByText("Test Brand")).ToBeVisibleAsync();

		// Items are visible
		await Expect(Page.GetByText("Dashboard")).ToBeVisibleAsync();
		await Expect(Page.GetByText("Reports")).ToBeVisibleAsync();

		// Footer is visible
		await Expect(Page.Locator(".hx-sidebar-footer")).ToBeVisibleAsync();
		await Expect(Page.GetByText("GitHub")).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that clicking a sidebar item navigates to the item's href.
	/// </summary>
	[TestMethod]
	public async Task HxSidebar_ClickItem_Navigates()
	{
		await NavigateToTestAppAsync("/HxSidebarTests");

		await Page.GetByText("Dashboard").ClickAsync();

		await Page.WaitForURLAsync("**/HxSidebarTests/navigate-target");
	}

	/// <summary>
	/// Verifies that clicking the collapse toggler collapses and expands the sidebar.
	/// </summary>
	[TestMethod]
	public async Task HxSidebar_CollapseToggle_CollapsesAndExpands()
	{
		await NavigateToTestAppAsync("/HxSidebarTests");

		var toggler = Page.Locator(".hx-sidebar-toggler");
		var collapsedState = Page.Locator("[data-testid='collapsed-state']");

		// Initially expanded
		await Expect(collapsedState).ToHaveTextAsync("expanded");

		// Collapse the sidebar
		await toggler.ClickAsync();
		await Expect(collapsedState).ToHaveTextAsync("collapsed");
		await Expect(Page.Locator("[data-testid='sidebar-wrapper'] .hx-sidebar.collapsed")).ToHaveCountAsync(1);

		// Expand the sidebar again
		await toggler.ClickAsync();
		await Expect(collapsedState).ToHaveTextAsync("expanded");
		await Expect(Page.Locator("[data-testid='sidebar-wrapper'] .hx-sidebar.collapsed")).ToHaveCountAsync(0);
	}

	/// <summary>
	/// Verifies that the sidebar item matching the current route has the active highlight styling.
	/// </summary>
	[TestMethod]
	public async Task HxSidebar_ActiveItem_HasHighlightStyling()
	{
		await NavigateToTestAppAsync("/HxSidebarTests/navigate-target");

		await Expect(Page.Locator("[data-testid='sidebar-wrapper'] a.hx-sidebar-item.active").First).ToBeVisibleAsync();
	}
}
