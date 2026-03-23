namespace Havit.Blazor.E2ETests.HxInputDateTests;

[TestClass]
public class HxInputDateTests : TestAppTestBase
{
	private const int DefaultTimeout = 5_000;

	[TestMethod]
	public async Task HxInputDate_ClickCalendarIcon_OpensCalendarPopup()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateTests");

		// Act - Click the calendar icon to open the dropdown
		var calendarIcon = Page.Locator(".hx-input-date-icon");
		await calendarIcon.ClickAsync();

		// Assert - The calendar popup should be visible
		var calendar = Page.Locator(".hx-calendar");
		await Expect(calendar).ToBeVisibleAsync(new() { Timeout = DefaultTimeout });
	}

	[TestMethod]
	public async Task HxInputDate_CalendarPickDay_SetsValueAndCloses()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateTests");

		// Act - Click the calendar icon to open the dropdown
		var calendarIcon = Page.Locator(".hx-input-date-icon");
		await calendarIcon.ClickAsync();

		// Wait for the calendar to be visible
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		// Click on day 15
		var calendar = Page.Locator(".hx-calendar");
		await calendar.Locator("button:has-text('15')").ClickAsync();

		// Assert - The value should be set
		await Expect(Page.Locator("[data-testid='current-value']")).Not.ToBeEmptyAsync(new() { Timeout = DefaultTimeout });

		// Assert - The calendar popup should be closed
		await Expect(calendar).Not.ToBeVisibleAsync(new() { Timeout = DefaultTimeout });
	}
}
