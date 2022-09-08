DateTime today = DateTime.Today;

DateTime thisMonthStart = new DateTime(today.Year, today.Month, 1);
DateTime thisMonthEnd = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
DateTime lastMonthStart = thisMonthStart.AddMonths(-1);
DateTime lastMonthEnd = new DateTime(lastMonthStart.Year, lastMonthStart.Month, DateTime.DaysInMonth(lastMonthStart.Year, lastMonthStart.Month));
DateTime thisYearStart = new DateTime(today.Year, 1, 1);
DateTime thisYearEnd = new DateTime(today.Year, 12, 31);
DateTime lastYearStart = thisYearStart.AddYears(-1);
DateTime lastYearEnd = thisYearEnd.AddYears(-1);

HxInputDateRange.Defaults.PredefinedDateRanges = new[]
	{
		new InputDateRangePredefinedRangesItem { Label = "This month", DateRange = new DateTimeRange(thisMonthStart, thisMonthEnd) },
		new InputDateRangePredefinedRangesItem { Label = "Last month", DateRange = new DateTimeRange(lastMonthStart, lastMonthEnd) },
		new InputDateRangePredefinedRangesItem { Label = "This year", DateRange = new DateTimeRange(thisYearStart, thisYearEnd) },
		new InputDateRangePredefinedRangesItem { Label = "Last year", DateRange = new DateTimeRange(lastYearStart, lastYearEnd) },
	};
