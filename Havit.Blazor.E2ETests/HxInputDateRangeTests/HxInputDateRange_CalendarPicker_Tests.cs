using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxInputDateRangeTests;

public class HxInputDateRange_CalendarPicker_Tests : TestAppTestBase
{
	[Fact]
	public async Task HxInputDateRange_CalendarPicker_SelectsDateRange()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_CalendarPicker");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// The menu becomes visible when its entry transition starts, but it re-renders its content once shown,
		// which would reset calendar navigation done before that. Wait for the shown event (subscribed before the click so it cannot be missed).
		await fromInput.EvaluateAsync("element => element.addEventListener('shown.bs.menu', () => element.dataset.e2eShown = 'true', { once: true })");

		// Act - Select "From" date using calendar picker
		await fromInput.ClickAsync(); // Open "From" dropdown

		// Wait for the calendar menu to be shown
		await Expect(fromInput).ToHaveAttributeAsync("data-e2e-shown", "true", new() { Timeout = 5_000 });

		var fromCalendar = Page.Locator(".hx-calendar").First;

		// Navigate calendar to January 2025
		await fromCalendar.Locator("select[aria-label='Year']").SelectOptionAsync("2025");
		await fromCalendar.Locator("select[aria-label='Month']").SelectOptionAsync("0"); // January (0-based)

		// Click on day 10
		await fromCalendar.Locator("button.hx-calendar-day-in:has-text('10')").ClickAsync();

		// Wait for "To" dropdown to auto-open (the "To" dropdown menu gets the "show" class)
		var toDropdownMenu = Page.Locator(".hx-input-date-range-menu").Last;
		await Expect(toDropdownMenu).ToHaveClassAsync(new Regex(".*\\bshow\\b.*"));

		var toCalendar = Page.Locator(".hx-calendar").Last;

		// Click on day 20 (after "From" date - valid range)
		await toCalendar.Locator("button.hx-calendar-day-in:has-text('20')").ClickAsync();

		// Wait for "To" dropdown to close
		await Expect(toDropdownMenu).Not.ToHaveClassAsync(new Regex(".*\\bshow\\b.*"));

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
