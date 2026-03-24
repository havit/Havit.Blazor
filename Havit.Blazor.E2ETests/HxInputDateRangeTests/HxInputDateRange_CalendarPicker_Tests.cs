namespace Havit.Blazor.E2ETests.HxInputDateRangeTests;

[TestClass]
public class HxInputDateRange_CalendarPicker_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputDateRange_CalendarPicker_SelectsDateRange()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_CalendarPicker");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Select "From" date using calendar picker
		await fromInput.ClickAsync(); // Open "From" dropdown

		// Wait for calendar to be visible
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		var fromCalendar = Page.Locator(".hx-calendar").First;

		// Navigate calendar to January 2025
		await fromCalendar.Locator("select[aria-label='Year']").SelectOptionAsync("2025");
		await fromCalendar.Locator("select[aria-label='Month']").SelectOptionAsync("0"); // January (0-based)

		// Click on day 10
		await fromCalendar.Locator("button.hx-calendar-day-in:has-text('10')").ClickAsync();

		// Wait for "From" dropdown to close and "To" dropdown to auto-open
		await Page.WaitForTimeoutAsync(500);

		// The "To" calendar should now be visible (auto-opened after "From" selection)
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		var toCalendar = Page.Locator(".hx-calendar").Last;

		// Click on day 20 (after "From" date - valid range)
		await toCalendar.Locator("button.hx-calendar-day-in:has-text('20')").ClickAsync();

		// Wait for dropdown to close
		await Page.WaitForTimeoutAsync(500);

		// Assert - Verify the exact dates were selected
		var expectedFromDate = new DateTime(2025, 1, 10).ToShortDateString();
		var expectedToDate = new DateTime(2025, 1, 20).ToShortDateString();

		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(expectedFromDate);
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(expectedToDate);

		// Verify inputs display the selected dates
		await Expect(fromInput).ToHaveValueAsync(expectedFromDate);
		await Expect(toInput).ToHaveValueAsync(expectedToDate);
	}
}
