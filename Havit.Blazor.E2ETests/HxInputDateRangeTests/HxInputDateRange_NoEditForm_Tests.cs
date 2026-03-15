using System.Text.RegularExpressions;

namespace Havit.Blazor.E2ETests.HxInputDateRangeTests;

[TestClass]
public class HxInputDateRange_NoEditForm_Tests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_BlocksInvalidFromDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set valid initial range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 6, 30);
		var invalidFromDate = new DateTime(2024, 12, 31);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Try to set "from" date after "to" date
		await fromInput.ClickAsync();
		await fromInput.FillAsync(invalidFromDate.ToShortDateString());
		await fromInput.PressAsync("Tab"); // Trigger validation

		// Assert

		// Value should not have changed to invalid value
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should have "is-invalid" class
		await Expect(fromInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// Validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_BlocksInvalidToDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set valid initial range
		var dateFrom = new DateTime(2024, 6, 1);
		var dateTo = new DateTime(2024, 12, 31);
		var invalidToDate = new DateTime(2024, 1, 1);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Try to set "to" date before "from" date
		await toInput.ClickAsync();
		await toInput.FillAsync(invalidToDate.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger validation

		// Assert

		// Value should not have changed to invalid value
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should have "is-invalid" class
		await Expect(fromInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// Validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsValidFromDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set valid initial range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 12, 31);
		var newFromDate = new DateTime(2024, 6, 1);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Set "from" date to a valid date (before "to" date)
		await fromInput.ClickAsync();
		await fromInput.FillAsync(newFromDate.ToShortDateString());
		await fromInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(newFromDate.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsValidToDate_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Set valid initial range
		var dateFrom = new DateTime(2024, 1, 1);
		var dateTo = new DateTime(2024, 6, 30);
		var newToDate = new DateTime(2024, 12, 31);

		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab"); // Trigger processing

		// Act - Set "to" date to a valid date (after "from" date)
		await toInput.ClickAsync();
		await toInput.FillAsync(newToDate.ToShortDateString());
		await toInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(newToDate.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsEqualDates_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

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

		// Act - Set "from" date equal to "to" date
		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateTo.ToShortDateString());
		await fromInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateTo.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsWhenOnlyFromDateSet_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Set only "from" date
		var dateFrom = new DateTime(2024, 6, 1);
		await fromInput.ClickAsync();
		await fromInput.FillAsync(dateFrom.ToShortDateString());
		await fromInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync(dateFrom.ToShortDateString());
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync("");

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsWhenOnlyToDateSet_WhenTyping()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Set only "to" date
		var dateTo = new DateTime(2024, 6, 30);
		await toInput.ClickAsync();
		await toInput.FillAsync(dateTo.ToShortDateString());
		await toInput.PressAsync("Tab");

		// Assert

		// Value should have updated
		await Expect(Page.Locator("[data-testid='value-from']")).ToHaveTextAsync("");
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync(dateTo.ToShortDateString());

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_BlocksInvalidFromDate_UsingCalendar()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Start with empty inputs and select invalid date range from calendar
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

		// Click on day 15
		await toCalendar.Locator("button:has-text('15')").ClickAsync();

		// Wait for dropdown to close
		await Page.WaitForTimeoutAsync(500);


		// Assert

		// From value should have the valid selected date (16th)
		await Expect(Page.Locator("[data-testid='value-from']")).Not.ToBeEmptyAsync();

		// To value should remain empty (invalid date was rejected)
		await Expect(Page.Locator("[data-testid='value-to']")).ToHaveTextAsync("");

		// Both inputs should have "is-invalid" class
		await Expect(fromInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// Validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).ToBeVisibleAsync();
	}

	[TestMethod]
	public async Task HxInputDateRange_NoEditForm_AllowsValidToDate_UsingCalendar()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputDateRange_NoEditForm");

		var inputs = await Page.Locator("input[type='text']").AllAsync();
		var fromInput = inputs[0];
		var toInput = inputs[1];

		// Act - Start with empty inputs and select valid date range from calendar
		// Select From date: 15th of current month
		await fromInput.ClickAsync(); // Open dropdown

		// Wait for calendar to be visible
		await Page.WaitForSelectorAsync(".hx-calendar", new() { State = WaitForSelectorState.Visible });

		var fromCalendar = Page.Locator(".hx-calendar").First;

		// Click on day 15
		await fromCalendar.Locator("button:has-text('15')").ClickAsync();

		// Wait for From-dropdown to close and To-dropdown to open (opens automatically after From selection)
		await Page.WaitForTimeoutAsync(500);

		var toCalendar = Page.Locator(".hx-calendar").Last;

		// Click on day 16 (after From date - valid)
		await toCalendar.Locator("button:has-text('16')").ClickAsync();

		// Wait for dropdown to close
		await Page.WaitForTimeoutAsync(500);

		// Assert

		// Both values should be set
		await Expect(Page.Locator("[data-testid='value-from']")).Not.ToBeEmptyAsync();
		await Expect(Page.Locator("[data-testid='value-to']")).Not.ToBeEmptyAsync();

		// Both inputs should not have "is-invalid" class
		await Expect(fromInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));
		await Expect(toInput).Not.ToHaveAttributeAsync("class", new Regex(".*\\bis-invalid\\b.*"));

		// No validation error should be visible
		await Expect(Page.Locator("text=TestDateOrderErrorMessage")).Not.ToBeVisibleAsync();
	}
}
