using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxAlertTests;

[TestClass]
public class HxAlertTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxAlert_Render_DisplaysContentAndColor()
	{
		// Arrange & Act
		await NavigateToTestAppAsync("/HxAlert_Render");

		// Assert - primary alert is visible with correct content and color class
		var alert = Page.Locator("[data-testid='alert-primary'] .alert");
		await Expect(alert).ToBeVisibleAsync();
		await Expect(alert).ToHaveClassAsync(new Regex("alert-primary"));
		await Expect(alert).ToContainTextAsync("Primary alert content");
	}

	[TestMethod]
	public async Task HxAlert_ClickDismiss_HidesAlert()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxAlert_Dismiss");

		var alert = Page.Locator("[data-testid='alert-dismissible-container'] .alert");
		await Expect(alert).ToBeVisibleAsync();

		// Act - click the close button (Bootstrap JS dismisses the alert)
		var closeButton = alert.Locator(".btn-close");
		await closeButton.ClickAsync();

		// Assert - alert is no longer visible
		await Expect(alert).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxAlert_AfterDismiss_RemovedFromDOM()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxAlert_Dismiss");

		var alert = Page.Locator("[data-testid='alert-dismissible-container'] .alert");
		await Expect(alert).ToBeVisibleAsync();

		// Act - click the close button
		var closeButton = alert.Locator(".btn-close");
		await closeButton.ClickAsync();

		// Assert - alert element is removed from the DOM by Bootstrap JS
		await Expect(alert).ToHaveCountAsync(0);
	}
}
