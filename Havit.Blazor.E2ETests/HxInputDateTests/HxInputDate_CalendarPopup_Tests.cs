namespace Havit.Blazor.E2ETests.HxInputDateTests;

[TestClass]
public class HxInputDate_CalendarPopup_Tests : TestAppTestBase
{
	private const int DefaultTimeout = 5_000;

	[TestMethod]
	public async Task HxInputDate_ClickCalendarIcon_OpensCalendarPopup()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDate_CalendarPopup");

		var wrapper = Page.Locator("[data-testid='input-date-wrapper']");

		// Act - Click the calendar icon to open the dropdown
		await wrapper.Locator(".hx-input-date-icon").ClickAsync();

		// Assert - The calendar popup should be visible
		await Expect(wrapper.Locator(".hx-calendar")).ToBeVisibleAsync(new() { Timeout = DefaultTimeout });
	}

	[TestMethod]
	public async Task HxInputDate_CalendarPickDay_SetsValueAndCloses()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDate_CalendarPopup");

		var wrapper = Page.Locator("[data-testid='input-date-wrapper']");

		// Act - Click the calendar icon to open the dropdown
		await wrapper.Locator(".hx-input-date-icon").ClickAsync();

		// Wait for the calendar to be visible
		var calendar = wrapper.Locator(".hx-calendar");
		await Expect(calendar).ToBeVisibleAsync(new() { Timeout = DefaultTimeout });

		// Click on day 15 (15th in-month day; zero-based index 14)
		await calendar.Locator(".hx-calendar-day-in").Nth(14).ClickAsync();

		// Assert - The value should be set to the selected date
		var expectedDateText = new DateTime(2024, 6, 15).ToShortDateString();
		await Expect(Page.Locator("[data-testid='current-value']")).ToHaveTextAsync(expectedDateText, new() { Timeout = DefaultTimeout });

		// Assert - The calendar popup should be closed
		await Expect(calendar).Not.ToBeVisibleAsync(new() { Timeout = DefaultTimeout });
	}
}
