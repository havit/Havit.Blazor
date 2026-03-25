using System.Globalization;
using Havit;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxCalendarTests : BunitTestBase
{
	[TestMethod]
	public void HxCalendar_Render_SetsMonthAndYearFromDisplayMonth()
	{
		// Arrange
		var displayMonth = new DateTime(2025, 6, 1);

		// Act
		var cut = RenderComponent<HxCalendar>(parameters => parameters
			.Add(p => p.DisplayMonth, displayMonth)
			.Add(p => p.DisplayMonthChanged, (DateTime _) => { })
		);

		// Assert — verify the month select has June selected (index 5, zero-based)
		var monthSelect = cut.Find("select[aria-label='Month']");
		Assert.AreEqual("5", monthSelect.GetAttribute("value"));

		// Assert — verify the year select has 2025 selected
		var yearSelect = cut.Find("select[aria-label='Year']");
		Assert.AreEqual("2025", yearSelect.GetAttribute("value"));
	}

	[TestMethod]
	public async Task HxCalendar_NavigateNext_FiresDisplayMonthChangedWithNextMonth()
	{
		// Arrange
		var displayMonth = new DateTime(2025, 3, 1);
		DateTime? reportedMonth = null;

		var cut = RenderComponent<HxCalendar>(parameters => parameters
			.Add(p => p.DisplayMonth, displayMonth)
			.Add(p => p.DisplayMonthChanged, (DateTime newMonth) => { reportedMonth = newMonth; })
		);

		// Act — click the next-month navigation button (second .hx-calendar-navigation-button)
		var navButtons = cut.FindAll("button.hx-calendar-navigation-button");
		Assert.HasCount(2, navButtons, "Expected exactly 2 navigation buttons (previous and next)");
		await cut.InvokeAsync(() => navButtons[1].Click());

		// Assert
		Assert.IsNotNull(reportedMonth);
		Assert.AreEqual(new DateTime(2025, 4, 1), reportedMonth.Value);
	}

	[TestMethod]
	public async Task HxCalendar_NavigatePrevious_FiresDisplayMonthChangedWithPreviousMonth()
	{
		// Arrange
		var displayMonth = new DateTime(2025, 3, 1);
		DateTime? reportedMonth = null;

		var cut = RenderComponent<HxCalendar>(parameters => parameters
			.Add(p => p.DisplayMonth, displayMonth)
			.Add(p => p.DisplayMonthChanged, (DateTime newMonth) => { reportedMonth = newMonth; })
		);

		// Act — click the previous-month navigation button (first .hx-calendar-navigation-button)
		var navButtons = cut.FindAll("button.hx-calendar-navigation-button");
		Assert.HasCount(2, navButtons, "Expected exactly 2 navigation buttons (previous and next)");
		await cut.InvokeAsync(() => navButtons[0].Click());

		// Assert
		Assert.IsNotNull(reportedMonth);
		Assert.AreEqual(new DateTime(2025, 2, 1), reportedMonth.Value);
	}

	[TestMethod]
	public async Task HxCalendar_SelectDate_UpdatesBoundValue()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var displayMonth = new DateTime(2025, 3, 1);
			DateTime? selectedValue = null;

			var cut = RenderComponent<HxCalendar>(parameters => parameters
				.Add(p => p.DisplayMonth, displayMonth)
				.Add(p => p.DisplayMonthChanged, (DateTime _) => { })
				.Add(p => p.ValueChanged, (DateTime? newValue) => { selectedValue = newValue; })
			);

			// Act — find and click a day button showing "15" that is in the current month
			var dayButtons = cut.FindAll("button.hx-calendar-day.hx-calendar-day-in");
			var day15Button = dayButtons.Single(b => b.TextContent.Trim() == "15");
			await cut.InvokeAsync(() => day15Button.Click());

			// Assert
			Assert.IsNotNull(selectedValue);
			Assert.AreEqual(new DateTime(2025, 3, 15), selectedValue.Value);
		}
	}

	[TestMethod]
	public void HxCalendar_SelectedDate_HasActiveClass()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var selectedDate = new DateTime(2025, 3, 20);

			// Act
			var cut = RenderComponent<HxCalendar>(parameters => parameters
				.Add(p => p.Value, selectedDate)
				.Add(p => p.ValueChanged, (DateTime? _) => { })
				.Add(p => p.DisplayMonth, new DateTime(2025, 3, 1))
				.Add(p => p.DisplayMonthChanged, (DateTime _) => { })
			);

			// Assert — the element for day 20 should have the "hx-calendar-day-selected" CSS class
			var selectedElements = cut.FindAll(".hx-calendar-day-selected");
			Assert.HasCount(1, selectedElements, "Exactly one day should have the selected class");
			Assert.AreEqual("20", selectedElements[0].TextContent.Trim());
		}
	}
}
