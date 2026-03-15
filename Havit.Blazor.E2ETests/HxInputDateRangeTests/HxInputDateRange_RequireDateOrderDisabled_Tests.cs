using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxInputDateRangeTests;

[TestClass]
public class HxInputDateRange_RequireDateOrderDisabled_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputDateRange_RequireDateOrderDisabled_AllowsInvalidFromDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_RequireDateOrderDisabled");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set initial range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 6, 30);
		var invalidFromDate = new DateTime(2024, 12, 31);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Set "from" date after "to" date (should be allowed when validation is disabled)
		await fromInput.ClickAsync();
		await fromInput.FillAsync(invalidFromDate.ToShortDateString());
		await fromInput.PressAsync("Tab");

		// Assert

		// Value should have updated even though it's "invalid"
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(invalidFromDate.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class (validation is disabled)
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_RequireDateOrderDisabled_AllowsInvalidToDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_RequireDateOrderDisabled");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set initial range
		var dateFrom = new DateTime(2024, 6, 1);
		var dateTo = new DateTime(2024, 12, 31);
		var invalidToDate = new DateTime(2024, 1, 1);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Set "to" date before "from" date (should be allowed when validation is disabled)
		await toInput.ClickAsync();
		await toInput.FillAsync(invalidToDate.ToShortDateString());
		await toInput.PressAsync("Tab");

		// Assert

		// Value should have updated even though it's "invalid"
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(invalidToDate.ToShortDateString());

		// Both inputs should not have "is-invalid" class (validation is disabled)
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_RequireDateOrderDisabled_AllowsValidRange_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_RequireDateOrderDisabled");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Set valid range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 12, 31);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_RequireDateOrderDisabled_AllowsDateRangeOutOfOrder_UsingCalendar()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_RequireDateOrderDisabled");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Start with empty inputs and select invalid date range from calendar (should be allowed)
		// Select From date: 16th of current month
		await fromInput.ClickAsync(); // Open dropdown

		// Wait for calendar to be visible
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		var fromCalendar = Page.Locator(".hx-calendar").First;

		// Click on day 16
		await fromCalendar.Locator("button:has-text('16')").ClickAsync();

		// Wait for From-dropdown to close and To-dropdown to open (opens automatically after From selection)
		await Page.WaitForTimeoutAsync(500);

		var toCalendar = Page.Locator(".hx-calendar").Last;

		// Click on day 15 (before From date - invalid but should be allowed when validation is disabled)
		await toCalendar.Locator("button:has-text('15')").ClickAsync();

		// Wait for dropdown to close
		await Page.WaitForTimeoutAsync(500);

		// Assert

		// Both values should be set even though order is "invalid"
		await Expect(Page.Locator("[data-testid='value-from']")).Not.ToBeEmptyAsync();
		await Expect(Page.Locator("[data-testid='value-to']")).Not.ToBeEmptyAsync();

		// Both inputs should not have "is-invalid" class (validation is disabled)
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_RequireDateOrderDisabled_ClearButton_Works()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_RequireDateOrderDisabled");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set initial range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 12, 31);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Open calendar dropdown for "from" input and click Clear button
		await fromInput.ClickAsync(); // Open dropdown

		// Wait for calendar to be visible
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		// Click Clear button
		var clearButton = Page.Locator("button:has-text('Clear')").First;
		await clearButton.ClickAsync();

		// Wait for dropdown to close
		await Page.WaitForTimeoutAsync(500);

		// Assert

		// From date should be cleared
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync("");
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}
}
