namespace Havit.Blazor.E2ETests.HxOffcanvasTests;

[TestClass]
public class HxOffcanvas_BasicTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxOffcanvas_Open_ShowsPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxOffcanvas_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - offcanvas panel content should be visible
		var offcanvasContent = Page.Locator("[data-testid='offcanvas-content']");
		await Expect(offcanvasContent).ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxOffcanvas_ClickCloseButton_ClosesPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxOffcanvas_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		var offcanvasContent = Page.Locator("[data-testid='offcanvas-content']");
		await Expect(offcanvasContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the close button inside the offcanvas header
		var closeButton = Page.Locator(".hx-offcanvas-close-button");
		await closeButton.ClickAsync();

		// Assert - offcanvas content should no longer be visible
		await Expect(offcanvasContent).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxOffcanvas_ClickBackdrop_ClosesPanel()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxOffcanvas_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");
		await showButton.ClickAsync();

		var offcanvasContent = Page.Locator("[data-testid='offcanvas-content']");
		await Expect(offcanvasContent).ToBeVisibleAsync(new() { Timeout = 10_000 });

		// Act - click the backdrop
		var backdrop = Page.Locator(".offcanvas-backdrop");
		await backdrop.ClickAsync();

		// Assert - offcanvas content should no longer be visible
		await Expect(offcanvasContent).Not.ToBeVisibleAsync(new() { Timeout = 10_000 });
	}

	[TestMethod]
	public async Task HxOffcanvas_Content_RendersCorrectly()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxOffcanvas_BasicTests");

		var showButton = Page.Locator("[data-testid='show-button']");

		// Act
		await showButton.ClickAsync();

		// Assert - offcanvas content text renders correctly
		var offcanvasContent = Page.Locator("[data-testid='offcanvas-content']");
		await Expect(offcanvasContent).ToBeVisibleAsync(new() { Timeout = 10_000 });
		await Expect(offcanvasContent).ToHaveTextAsync("Offcanvas body content");

		// Assert - offcanvas title renders correctly
		var offcanvasTitle = Page.Locator(".offcanvas-title");
		await Expect(offcanvasTitle).ToBeVisibleAsync();
		await Expect(offcanvasTitle).ToHaveTextAsync("Test Offcanvas");
	}
}
