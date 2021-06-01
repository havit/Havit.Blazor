using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCalendar
	{
		[Parameter] public DateTime? Value { get; set; }
		[Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }

		[Parameter] public DateTime DisplayMonth { get; set; }
		[Parameter] public EventCallback<DateTime> DisplayMonthChanged { get; set; }

		[Parameter] public int MinYear { get; set; } = 1900;
		[Parameter] public int MaxYear { get; set; } = 2099;

		private CultureInfo Culture => CultureInfo.CurrentUICulture;
		private DayOfWeek FirstDayOfWeek => Culture.DateTimeFormat.FirstDayOfWeek;
		public DateTime DisplayMonthFirstDay => new DateTime(DisplayMonth.Year, DisplayMonth.Month, 1);
		public DateTime FirstDayToDisplay
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

		}

		private RenderData GetRenderData()
		{
			RenderData result = new RenderData();
			result.DaysOfWeek = new List<string>(7);

			string[] dayNames = Culture.DateTimeFormat.ShortestDayNames;
			DayOfWeek firstDayOfWeek = this.FirstDayOfWeek;

			result.Years = Enumerable.Range(MinYear, MaxYear - MinYear + 1).Reverse().ToList();

			for (int i = 0; i < 7; i++)
			{
				result.DaysOfWeek.Add(dayNames[((int)firstDayOfWeek + i) % 7]);
			}

			result.Months = Culture.DateTimeFormat.MonthNames.Take(12).ToList(); // returns 13 items, see https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo.monthnames?view=net-5.0

			result.Weeks = new List<WeekData>(6);

			DateTime currentDay = FirstDayToDisplay;
			DateTime valueDay = Value?.Date ?? default;
			DateTime today = DateTime.Today;

			for (var week = 0; week < 6; week++)
			{
				WeekData weekData = new WeekData();
				weekData.Days = new List<DayData>(7);

				for (int day = 0; day < 7; day++)
				{
					bool clickEnabled = (currentDay.Year >= MinYear) // can click only days starting MinYear
							&& (currentDay.Year <= MaxYear); // can click only days ending MaxYear
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
				result.Weeks.Add(weekData);
			}

			return result;

		}

		private async Task SetDisplayMonthAsync(DateTime newDisplayMonth)
		{
			DisplayMonth = newDisplayMonth;
			await DisplayMonthChanged.InvokeAsync(newDisplayMonth);
		}

		private async Task HandlePreviousMonthClickAsync()
		{
			var previousMonth = DisplayMonth.AddMonths(-1);
			if (previousMonth.Year >= MinYear)
			{
				await SetDisplayMonthAsync(previousMonth);
			}
		}

		private async Task HandleNextMonthClickAsync()
		{
			var nextMonth = DisplayMonth.AddMonths(1);
			if (nextMonth.Year <= MaxYear)
			{
				await SetDisplayMonthAsync(nextMonth);
			}
		}

		private async Task HandleYearChangeAsync(ChangeEventArgs changeEventArgs)
		{
			int year = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(year, DisplayMonth.Month, 1));
		}

		private async Task HandleMonthChangeAsync(ChangeEventArgs changeEventArgs)
		{
			int monthIndex = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(DisplayMonth.Year, monthIndex + 1, 1));
		}

		private async Task HandleDayClickAsync(DayData day)
		{
			Value = day.Date;
			await ValueChanged.InvokeAsync(day.Date);
		}

		private class RenderData
		{
			public List<string> DaysOfWeek { get; set; }
			public List<string> Months { get; set; }
			public List<int> Years { get; set; }
			public List<WeekData> Weeks { get; set; }
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
	}
}
