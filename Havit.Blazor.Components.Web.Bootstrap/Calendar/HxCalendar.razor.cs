using System.Globalization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Basic calendar building block. Used for <see cref="HxInputDate{TValue}"/> and <see cref="HxInputDateRange"/> implementation.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCalendar">https://havit.blazor.eu/components/HxCalendar</see>
/// </summary>
public partial class HxCalendar
{
	/// <summary>
	/// Internal HFW shared default.
	/// </summary>
	internal static DateTime DefaultMinDate => new DateTime(1900, 1, 1);

	/// <summary>
	/// Internal HFW shared default.
	/// </summary>
	internal static DateTime DefaultMaxDate => new DateTime(2099, 12, 31);

	/// <summary>
	/// Application-wide defaults for the <see cref="HxCalendar"/>.
	/// </summary>
	public static CalendarSettings Defaults { get; set; }

	static HxCalendar()
	{
		Defaults = new CalendarSettings()
		{
			MinDate = DefaultMinDate,
			MaxDate = DefaultMaxDate,
		};
	}

	/// <summary>
	/// Returns the component defaults.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual CalendarSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public CalendarSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual CalendarSettings GetSettings() => Settings;

	/// <summary>
	/// Date selected.
	/// </summary>
	[Parameter] public DateTime? Value { get; set; }

	/// <summary>
	/// Raised when the selected date changes.
	/// </summary>
	[Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="ValueChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeValueChangedAsync(DateTime? newValue) => ValueChanged.InvokeAsync(newValue);

	/// <summary>
	/// Month to display.
	/// </summary>
	[Parameter] public DateTime DisplayMonth { get; set; }

	/// <summary>
	/// Raised when the month selection changes.
	/// </summary>
	[Parameter] public EventCallback<DateTime> DisplayMonthChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="DisplayMonthChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeDisplayMonthChangedAsync(DateTime newValue) => DisplayMonthChanged.InvokeAsync(newValue);

	/// <summary>
	/// First date selectable from the calendar.<br/>
	/// Default is <c>1.1.1900</c> (configurable from <see cref="HxCalendar.Defaults"/>).
	/// </summary>
	[Parameter] public DateTime? MinDate { get; set; }
	protected DateTime MinDateEffective => MinDate ?? GetSettings()?.MinDate ?? GetDefaults().MinDate ?? throw new InvalidOperationException(nameof(MinDate) + " default for " + nameof(HxCalendar) + " has to be set.");

	/// <summary>
	/// Last date selectable from the calendar.<br />
	/// Default is <c>31.12.2099</c> (configurable from <see cref="HxCalendar.Defaults"/>).
	/// </summary>
	[Parameter] public DateTime? MaxDate { get; set; }
	protected DateTime MaxDateEffective => MaxDate ?? GetSettings()?.MaxDate ?? GetDefaults().MaxDate ?? throw new InvalidOperationException(nameof(MaxDate) + " default for " + nameof(HxCalendar) + " has to be set.");

	/// <summary>
	/// Allows customization of the dates in calendar.
	/// </summary>
	[Parameter] public CalendarDateCustomizationProviderDelegate DateCustomizationProvider { get; set; }
	protected CalendarDateCustomizationProviderDelegate DateCustomizationProviderEffective => DateCustomizationProvider ?? GetSettings()?.DateCustomizationProvider ?? GetDefaults().DateCustomizationProvider;

	/// <summary>
	/// Indicates whether the keyboard navigation is enabled. When disabled, the calendar renders <c>tabindex="-1"</c> on interactive elements.
	/// Default is <c>true</c> (tabindex attribute is not rendered).
	/// </summary>
	[Parameter] public bool KeyboardNavigation { get; set; } = true;

	// Set during SetParameterSetAsync to make it optional
	[Inject] protected TimeProvider TimeProviderFromServices { get; set; }

	/// <summary>
	/// TimeProvider is resolved in the following order:<br />
	///		1. TimeProvider from this parameter <br />
	///		2. Settings TimeProvider (configurable from <see cref="HxCalendar.Settings"/>)<br />
	///		3. Defaults TimeProvider (configurable from <see cref="HxCalendar.Defaults"/>)<br />
	///		4. TimeProvider from DependencyInjection<br />
	/// </summary>
	[Parameter] public TimeProvider TimeProvider { get; set; } = null;

	protected TimeProvider TimeProviderEffective => TimeProvider ?? GetSettings()?.TimeProvider ?? GetDefaults()?.TimeProvider ?? TimeProviderFromServices;

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

	private RenderData _renderData;
	private DateTime? _lastKnownValue;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		bool lastKnownValueChanged = _lastKnownValue != Value;
		_lastKnownValue = Value;

		// whenever the value is changed from outside, we set the display month to the value month
		if (DisplayMonth == default || lastKnownValueChanged)
		{
			if (Value != null)
			{
				await SetDisplayMonthAsync(Value.Value);
			}
			else
			{
				await SetDisplayMonthAsync(TimeProviderEffective.GetLocalNow().Date, limitDisplayMonthByMinMaxDateEffective: true);
			}
		}

		UpdateRenderData();
	}

	/// <summary>
	/// Refreshes the calendar.
	/// Useful when the customization needs to be updated.
	/// </summary>
	public Task RefreshAsync()
	{
		StateHasChanged();
		return Task.CompletedTask;
	}

	private void UpdateRenderData()
	{
		_renderData = new RenderData();
		_renderData.DaysOfWeek = new List<string>(7);

		string[] dayNames = Culture.DateTimeFormat.AbbreviatedDayNames;
		DayOfWeek firstDayOfWeek = FirstDayOfWeek;

		DateTime minDateEffective = MinDateEffective;
		DateTime maxDateEffective = MaxDateEffective;
		int minYear = minDateEffective.Year;
		int maxYear = maxDateEffective.Year;

		_renderData.Years = Enumerable.Range(minYear, maxYear - minYear + 1)
			.Union([DisplayMonth.Year])
			.OrderByDescending(year => year)
			.ToList();

		for (int i = 0; i < 7; i++)
		{
			_renderData.DaysOfWeek.Add(dayNames[((int)firstDayOfWeek + i) % 7]);
		}

		_renderData.Months = Culture.DateTimeFormat.MonthNames.Take(12) // returns 13 items, see https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo.monthnames
			.Select((name, index) => new MonthData
			{
				Index = index,
				Name = name,
				Enabled = (new DateTime(DisplayMonth.Year, index + 1, DateTime.DaysInMonth(DisplayMonth.Year, index + 1)) >= minDateEffective) && (new DateTime(DisplayMonth.Year, index + 1, 1) <= maxDateEffective)
			})
			.ToList();

		_renderData.Weeks = new List<WeekData>(6);

		DateTime currentDay = FirstDayToDisplay;
		DateTime valueDay = Value?.Date ?? default;
		DateTime today = TimeProviderEffective.GetLocalNow().Date;

		for (var week = 0; week < 6; week++)
		{
			var weekData = new WeekData();
			weekData.Key = week;
			weekData.Days = new List<DayData>(7);

			for (int day = 0; day < 7; day++)
			{
				CalendarDateCustomizationResult customization = GetDateCustomization(DateCustomizationProviderEffective, currentDay);

				bool clickEnabled = (currentDay >= minDateEffective) // can click only days starting MinDate
						&& (currentDay <= maxDateEffective) && (customization?.Enabled ?? true); // can click only days ending MaxDate
				string cssClass = CssClassHelper.Combine(
					clickEnabled ? "hx-calendar-day-active" : "hx-calendar-day-disabled",
					(currentDay == valueDay) ? "hx-calendar-day-selected" : null,  // currently selected day has "selected" class
					((currentDay.Month == DisplayMonth.Month) && (currentDay.Year == DisplayMonth.Year)) ? "hx-calendar-day-in" : "hx-calendar-day-out",
					(currentDay == today) ? "hx-calendar-day-today" : null,
					((currentDay.DayOfWeek == DayOfWeek.Saturday) || (currentDay.DayOfWeek == DayOfWeek.Sunday)) ? "hx-calendar-day-weekend" : null,
					customization?.CssClass
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
			_renderData.Weeks.Add(weekData);
		}

		_renderData.PreviousMonthEnabled = minDateEffective < DisplayMonth; // DisplayMonth is always the first fay of month
		_renderData.NextMonthEnabled = maxDateEffective > new DateTime(DisplayMonth.Year, DisplayMonth.Month, DateTime.DaysInMonth(DisplayMonth.Year, DisplayMonth.Month));
	}

	private CalendarDateCustomizationResult GetDateCustomization(CalendarDateCustomizationProviderDelegate dateCustomizationProvider, DateTime day)
	{
		if (dateCustomizationProvider == null)
		{
			return null;
		}

		return dateCustomizationProvider.Invoke(new CalendarDateCustomizationRequest
		{
			Target = CalendarDateCustomizationTarget.Calendar,
			Date = day
		});
	}

	private async Task SetDisplayMonthAsync(DateTime newDisplayMonth, bool limitDisplayMonthByMinMaxDateEffective = false)
	{
		if (limitDisplayMonthByMinMaxDateEffective)
		{
			newDisplayMonth = new[] { newDisplayMonth, new DateTime(MinDateEffective.Year, MinDateEffective.Month, 1) }.Max();
			newDisplayMonth = new[] { newDisplayMonth, new DateTime(MaxDateEffective.Year, MaxDateEffective.Month, 1) }.Min();
		}

		if (DisplayMonth != newDisplayMonth)
		{
			DisplayMonth = newDisplayMonth;
			await InvokeDisplayMonthChangedAsync(newDisplayMonth);
		}
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
		Contract.Requires<InvalidOperationException>(day.ClickEnabled, "The selected date is disabled."); // Just for case, the disabled date does not use event handler.

		Value = day.Date;
		_lastKnownValue = day.Date;
		await InvokeValueChangedAsync(day.Date);
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
		public int Key { get; set; }
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
