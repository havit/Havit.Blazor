using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Basic calendar building block. Used for <see cref="HxInputDate{TValue}"/> and <see cref="HxInputDateRange"/> implementation.
	/// </summary>
	public partial class HxCalendar
	{
		public static CalendarDefaults Defaults { get; set; } = new CalendarDefaults();

		/// <summary>
		/// Date selected.
		/// </summary>
		[Parameter] public DateTime? Value { get; set; }

		/// <summary>
		/// Raised when selected date changes.
		/// </summary>
		[Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }

		/// <summary>
		/// Month to display.
		/// </summary>
		[Parameter] public DateTime DisplayMonth { get; set; }

		/// <summary>
		/// Raised when month selection changes.
		/// </summary>
		[Parameter] public EventCallback<DateTime> DisplayMonthChanged { get; set; }

		/// <summary>
		/// First date selectable from the calendar.<br/>
		/// Default is <c>1.1.1900</c> (configurable from <see cref="HxCalendar.Defaults"/>).
		/// </summary>
		[Parameter] public DateTime? MinDate { get; set; }

		/// <summary>
		/// Last date selectable from the calendar.<br />
		/// Default is <c>31.12.2099</c> (configurable from <see cref="HxCalendar.Defaults"/>).
		/// </summary>
		[Parameter] public DateTime? MaxDate { get; set; }

		private DateTime MinDateEffective => MinDate ?? GetDefaults().MinDate;
		private DateTime MaxDateEffective => MaxDate ?? GetDefaults().MaxDate;

		/// <summary>
		/// Returns <see cref="HxCalendar"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual CalendarDefaults GetDefaults() => Defaults;

		private CultureInfo Culture => CultureInfo.CurrentUICulture;
		private DayOfWeek FirstDayOfWeek => Culture.DateTimeFormat.FirstDayOfWeek;
		protected DateTime DisplayMonthFirstDay => new DateTime(DisplayMonth.Year, DisplayMonth.Month, 1);
		protected DateTime FirstDayToDisplay
		{
			get
			{
				DateTime firstDayToDisplay = DisplayMonthFirstDay;
				firstDayToDisplay = firstDayToDisplay.AddDays(-1 * (((int)firstDayToDisplay.DayOfWeek - (int)FirstDayOfWeek + 7) % 7));

				// We display 6 rows of days.
				// When the month has 28 days and the first day is also first day of week, the month fits to 4 rows.
				// We add one row for the week before the display month and row for the week after the month.
				if ((DateTime.DaysInMonth(DisplayMonth.Year, DisplayMonth.Month) == 28) && (DisplayMonthFirstDay.DayOfWeek == FirstDayOfWeek))
				{
					firstDayToDisplay = firstDayToDisplay.AddDays(-7);
				}
				return firstDayToDisplay;
			}
		}

		private RenderData renderData;

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			if (DisplayMonth == default)
			{
				await SetDisplayMonthAsync(Value ?? DateTime.Today);
			}
			else if ((Value != null) && ((DisplayMonth.Year != Value.Value.Year) || (DisplayMonth.Month != Value.Value.Month)))
			{
				await SetDisplayMonthAsync(Value.Value);
			}

			UpdateRenderData();
		}

		private void UpdateRenderData()
		{
			renderData = new RenderData();
			renderData.DaysOfWeek = new List<string>(7);

			string[] dayNames = Culture.DateTimeFormat.AbbreviatedDayNames;
			DayOfWeek firstDayOfWeek = this.FirstDayOfWeek;

			DateTime minDateEffective = MinDateEffective;
			DateTime maxDateEffective = MaxDateEffective;
			int minYear = minDateEffective.Year;
			int maxYear = maxDateEffective.Year;

			renderData.Years = Enumerable.Range(minYear, maxYear - minYear + 1).Reverse().ToList();

			for (int i = 0; i < 7; i++)
			{
				renderData.DaysOfWeek.Add(dayNames[((int)firstDayOfWeek + i) % 7]);
			}

			renderData.Months = Culture.DateTimeFormat.MonthNames.Take(12) // returns 13 items, see https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo.monthnames?view=net-5.0
				.Select((name, index) => new MonthData
				{
					Index = index,
					Name = name,
					Enabled = (new DateTime(DisplayMonth.Year, index + 1, DateTime.DaysInMonth(DisplayMonth.Year, index + 1)) >= MinDate) && (new DateTime(DisplayMonth.Year, index + 1, 1) <= MaxDateEffective)
				})
				.ToList();

			renderData.Weeks = new List<WeekData>(6);

			DateTime currentDay = FirstDayToDisplay;
			DateTime valueDay = Value?.Date ?? default;
			DateTime today = DateTime.Today;

			for (var week = 0; week < 6; week++)
			{
				WeekData weekData = new WeekData();
				weekData.Days = new List<DayData>(7);

				for (int day = 0; day < 7; day++)
				{
					bool clickEnabled = (currentDay >= minDateEffective) // can click only days starting MinDate
							&& (currentDay <= maxDateEffective); // can click only days ending MaxDate
					string cssClass = CssClassHelper.Combine(
						clickEnabled ? "active" : "disabled",
						(currentDay == valueDay) ? "selected" : null,  // currently selected day has "selected" class
						((currentDay.Month == DisplayMonth.Month) && (currentDay.Year == DisplayMonth.Year)) ? "in" : "out",
						(currentDay == today) ? "today" : null,
						((currentDay.DayOfWeek == DayOfWeek.Saturday) || (currentDay.DayOfWeek == DayOfWeek.Sunday)) ? "weekend" : null
					);

					DayData dayData = new DayData
					{
						Date = currentDay,
						DayInMonth = currentDay.Day.ToString("d", Culture.DateTimeFormat),
						CssClass = cssClass,
						ClickEnabled = clickEnabled
					};
					weekData.Days.Add(dayData);
					currentDay = currentDay.AddDays(1);
				}
				renderData.Weeks.Add(weekData);
			}

			renderData.PreviousMonthEnabled = minDateEffective < DisplayMonth; // DisplayMonth is always the first fay of month
			renderData.NextMonthEnabled = maxDateEffective > new DateTime(DisplayMonth.Year, DisplayMonth.Month, DateTime.DaysInMonth(DisplayMonth.Year, DisplayMonth.Month));
		}

		private async Task SetDisplayMonthAsync(DateTime newDisplayMonth)
		{
			newDisplayMonth = new[] { newDisplayMonth, new DateTime(MinDateEffective.Year, MinDateEffective.Month, 1) }.Max();
			newDisplayMonth = new[] { newDisplayMonth, new DateTime(MaxDateEffective.Year, MaxDateEffective.Month, 1) }.Min();

			DisplayMonth = newDisplayMonth;
			await DisplayMonthChanged.InvokeAsync(newDisplayMonth);
		}

		private async Task HandlePreviousMonthClickAsync()
		{
			var previousMonth = DisplayMonth.AddMonths(-1);
			await SetDisplayMonthAsync(previousMonth);
			UpdateRenderData();
		}

		private async Task HandleNextMonthClickAsync()
		{
			var nextMonth = DisplayMonth.AddMonths(1);
			await SetDisplayMonthAsync(nextMonth);
			UpdateRenderData();
		}

		private async Task HandleYearChangeAsync(ChangeEventArgs changeEventArgs)
		{
			int year = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(year, DisplayMonth.Month, 1));
			UpdateRenderData();
		}

		private async Task HandleMonthChangeAsync(ChangeEventArgs changeEventArgs)
		{
			int monthIndex = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(DisplayMonth.Year, monthIndex + 1, 1));
			UpdateRenderData();
		}

		private async Task HandleDayClickAsync(DayData day)
		{
			Value = day.Date;
			await ValueChanged.InvokeAsync(day.Date);
			UpdateRenderData();
		}

		private class RenderData
		{
			public List<string> DaysOfWeek { get; set; }
			public List<MonthData> Months { get; set; }
			public List<int> Years { get; set; }
			public List<WeekData> Weeks { get; set; }
			public bool PreviousMonthEnabled { get; set; }
			public bool NextMonthEnabled { get; set; }
		}

		private class WeekData
		{
			public List<DayData> Days { get; set; }
		}

		private class DayData
		{
			public string CssClass { get; set; }
			public string DayInMonth { get; set; }
			public DateTime Date { get; set; }
			public bool ClickEnabled { get; set; }
		}

		private class MonthData
		{
			public int Index { get; set; }
			public string Name { get; set; }
			public bool Enabled { get; set; }
		}

	}
}
