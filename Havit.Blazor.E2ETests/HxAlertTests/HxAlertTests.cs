using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxAlertTests;

public class HxAlertTests : TestAppTestBase
{
	[Fact]
	public async Task HxAlert_Render_DisplaysContentAndColor()
	{
		// Arrange & Act
		await NavigateToTestAppAsync("/HxAlert_Render");

		// Assert - primary alert is visible with correct content and color class
		var alert = Page.Locator("[data-testid='alert-primary'] .alert");
		await Expect(alert).ToBeVisibleAsync();
		await Expect(alert).ToHaveClassAsync(new Regex("theme-primary"));
		await Expect(alert).ToContainTextAsync("Primary alert content");
	}

	[Fact]
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

	[Fact]
	public async Task HxAlert_DismissWithOnClosed_RemovedFromDomByBlazor()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxAlert_OnClosed");

		var alert = Page.Locator("[data-testid='alert-onclosed-container'] .alert");
		var closedCount = Page.GetByTestId("closed-count");
		await Expect(alert).ToBeVisibleAsync();

		// Act - dismiss the alert, OnClosed stops rendering it
		await alert.Locator(".btn-close").ClickAsync();

		// Assert - OnClosed was raised and Blazor removed the alert
		// (Bootstrap removes the element itself, HxAlert returns it hidden so that Blazor can remove it without failing)
		await Expect(closedCount).ToHaveTextAsync("1");
		await Expect(alert).ToHaveCountAsync(0);

		// Act & Assert - the renderer still works, the alert can be shown and dismissed again
		await Page.GetByTestId("show-alert").ClickAsync();
		await Expect(alert).ToBeVisibleAsync();
		await alert.Locator(".btn-close").ClickAsync();
		await Expect(closedCount).ToHaveTextAsync("2");
		await Expect(alert).ToHaveCountAsync(0);
	}
}
