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
			for (var week = 0; week < 6; week++)
			{
				WeekData weekData = new WeekData();
				weekData.Days = new List<DayData>(7);

				for (int day = 0; day < 7; day++)
				{
					DayData dayData = new DayData
					{
						Date = currentDay,
						DayInMonth = currentDay.Day.ToString("d", Culture.DateTimeFormat),
						CssClass = GetCssClass(currentDay),
						Enabled = (currentDay.Year >= MinYear) && (currentDay.Year <= MaxYear)
					};
					weekData.Days.Add(dayData);
					currentDay = currentDay.AddDays(1);
				}
				result.Weeks.Add(weekData);
			}

			return result;

		}

		private string GetCssClass(DateTime currentDay)
		{
			List<string> values = new List<string>(5);
			return null; // TODO!
		}

		private async Task SetDisplayMonthAsync(DateTime newDisplayMonth)
		{
			DisplayMonth = newDisplayMonth;
			await DisplayMonthChanged.InvokeAsync(newDisplayMonth);
		}

		private async Task HandlePreviousMonthClick()
		{
			await SetDisplayMonthAsync(DisplayMonth.AddMonths(-1));
		}

		private async Task HandleNextMonthClick()
		{
			await SetDisplayMonthAsync(DisplayMonth.AddMonths(1));
		}

		private async Task HandleYearChange(ChangeEventArgs changeEventArgs)
		{
			int year = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(year, DisplayMonth.Month, 1));
		}

		private async Task HandleMonthChange(ChangeEventArgs changeEventArgs)
		{
			int monthIndex = int.Parse((string)changeEventArgs.Value);
			await SetDisplayMonthAsync(new DateTime(DisplayMonth.Year, monthIndex + 1, 1));
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
			public bool Enabled { get; set; }
		}
	}
}
