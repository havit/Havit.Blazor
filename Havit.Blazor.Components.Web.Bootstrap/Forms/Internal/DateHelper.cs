using System.Globalization;
using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;

internal static partial class DateHelper
{
	internal static bool TryParseDateFromString<TValue>(string value, TimeProvider timeProvider, out TValue result)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			result = default;
			bool isNullable = (Nullable.GetUnderlyingType(typeof(TValue)) != null);
			return isNullable;
		}

		// expecting date format with day, month, and year components
		int dayIndex = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.IndexOf("d");
		int monthIndex = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.IndexOf("M");
		int yearIndex = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.IndexOf("y");

		if ((dayIndex < 0) || (monthIndex < 0) || (yearIndex < 0))
		{
			result = default;
			return false;
		}

		string dateComponents = String.Join("", ((IEnumerable<(int, char)>)[(dayIndex, 'd'), (monthIndex, 'M'), (yearIndex, 'y')])
			.OrderBy(item => item.Item1) // dayIndex, monthIndex, yearIndex
			.Select(item => item.Item2)); // d, M, y

		Regex regex;

		// 010520 --> 01.05.2020
		switch (dateComponents)
		{
			case "dMy":
				regex = GetRegex_DayMonthYear_Strict();
				break;

			case "Mdy":
				regex = GetRegex_MonthDayYear_Strict();
				break;

			case "yMd":
				regex = GetRegex_YearMonthDay_Strict();
				break;

			case "ydM":
				regex = GetRegex_YearDayMonth_Strict();
				break;

			default:
				// unsupported date format
				regex = null;
				break;
		}

		if (TryGetValue<TValue>(regex, value, timeProvider, out result))
		{
			return true;
		}

		// 01. 05. 20 --> 01.05.2020
		switch (dateComponents)
		{
			case "dMy":
				regex = GetRegex_DayMonthYear_Relaxed();
				break;

			case "Mdy":
				regex = GetRegex_MonthDayYear_Relaxed();
				break;

			case "yMd":
				regex = GetRegex_YearMonthDay_Relaxed();
				break;

			case "ydM":
				regex = GetRegex_YearDayMonth_Relaxed();
				break;

			default:
				// unsupported date format
				regex = null;
				break;
		}

		if (TryGetValue<TValue>(regex, value, timeProvider, out result))
		{
			return true;
		}

		// 0105 --> 01.05.ThisYear
		switch (dateComponents)
		{
			case "dMy":
			case "ydM":
				regex = GetRegex_DayMonth_Strict();
				break;

			case "Mdy":
			case "yMd":
				regex = GetRegex_MonthDay_Strict();
				break;

			default:
				// unsupported date format
				regex = null;
				break;
		}

		if (TryGetValue<TValue>(regex, value, timeProvider, out result))
		{
			return true;
		}

		//01.05, 01.05. --> 01.05.ThisYear
		switch (dateComponents)
		{
			case "dMy":
			case "ydM":
				regex = GetRegex_DayMonth_Relaxed();
				break;

			case "Mdy":
			case "yMd":
				regex = GetRegex_MonthDay_Relaxed();
				break;

			default:
				regex = null;
				break;
		}

		if (TryGetValue<TValue>(regex, value, timeProvider, out result))
		{
			return true;
		}

		// "1", "01" --> 01.ThisMonth.ThisYear
		if (TryGetValue(GetRegex_Day_Strict(), value, timeProvider, out result))
		{
			return true;
		}

		// "-01.",  --> 01.ThisMonth.ThisYear
		if (TryGetValue(GetRegex_Day_Relaxed(), value, timeProvider, out result))
		{
			return true;
		}

		result = default;
		return false;
	}

	#region Regex patterns
	[GeneratedRegex("^(?<day>\\d{2})(?<month>\\d{2})(?<year>\\d{2})$")]
	private static partial Regex GetRegex_DayMonthYear_Strict();

	[GeneratedRegex("^(?<month>\\d{2})(?<day>\\d{2})(?<year>\\d{2})$")]
	private static partial Regex GetRegex_MonthDayYear_Strict();

	[GeneratedRegex("^(?<year>\\d{2})(?<month>\\d{2})(?<day>\\d{2})$")]
	private static partial Regex GetRegex_YearMonthDay_Strict();

	[GeneratedRegex("^(?<year>\\d{2})(?<day>\\d{2})(?<month>\\d{2})$")]
	private static partial Regex GetRegex_YearDayMonth_Strict();

	[GeneratedRegex("^\\W*(?<day>\\d{1,2})\\W+(?<month>\\d{1,2})\\W+(?<year>\\d{2}|\\d{4})\\W*$")]
	private static partial Regex GetRegex_DayMonthYear_Relaxed();

	[GeneratedRegex("^\\W*(?<month>\\d{1,2})\\W+(?<day>\\d{1,2})\\W+(?<year>\\d{2}|\\d{4})\\W*$")]
	private static partial Regex GetRegex_MonthDayYear_Relaxed();

	[GeneratedRegex("^\\W*(?<year>\\d{2}|\\d{4})\\W+(?<month>\\d{1,2})\\W+(?<day>\\d{1,2})\\W*$")]
	private static partial Regex GetRegex_YearMonthDay_Relaxed();

	[GeneratedRegex("^\\W*(?<year>\\d{2}|\\d{4})\\W+(?<day>\\d{1,2})\\W+(?<month>\\d{1,2})\\W*$")]
	private static partial Regex GetRegex_YearDayMonth_Relaxed();

	[GeneratedRegex("^(?<day>\\d{2})(?<month>\\d{2})$")]
	private static partial Regex GetRegex_DayMonth_Strict();

	[GeneratedRegex("^(?<month>\\d{2})(?<day>\\d{2})$")]
	private static partial Regex GetRegex_MonthDay_Strict();

	[GeneratedRegex("^\\W*(?<day>\\d{1,2})\\W+(?<month>\\d{1,2})\\W*?$")]
	private static partial Regex GetRegex_DayMonth_Relaxed();

	[GeneratedRegex("^\\W*(?<month>\\d{1,2})\\W+(?<day>\\d{1,2})\\W*?$")]
	private static partial Regex GetRegex_MonthDay_Relaxed();

	[GeneratedRegex("^(?<day>\\d{1,2})$")]
	private static partial Regex GetRegex_Day_Strict();

	[GeneratedRegex("^\\W*(?<day>\\d{1,2})\\W+$")]
	private static partial Regex GetRegex_Day_Relaxed();
	#endregion

	internal static bool TryGetValue<TValue>(Regex regex, string input, TimeProvider timeProvider, out TValue result)
	{
		if (regex == null)
		{
			result = default;
			return false;
		}

		Match match = regex.Match(input);

		if (match.Success)
		{
			int.TryParse(match.Groups["day"].Value, out int day);
			bool monthParsed = int.TryParse(match.Groups["month"].Value, out int month);
			bool yearParsed = int.TryParse(match.Groups["year"].Value, out int year);

			if (yearParsed && year < 100)
			{
				year += year < 50 ? 2000 : 1900; // Adjust year to be in the correct century
			}

			var localNow = timeProvider.GetLocalNow();

			try
			{
				var dateTimeOffset = new DateTimeOffset(new DateTime(
					year: yearParsed ? year : localNow.Year,
					month: monthParsed ? month : localNow.Month,
					day));
				result = GetValueFromDateTimeOffset<TValue>(dateTimeOffset);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				// NOOP
			}
		}

		result = default;
		return false;
	}

	internal static TValue GetValueFromDateTimeOffset<TValue>(DateTimeOffset? value)
	{
		if (value == null)
		{
			return default;
		}

		var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

		if (targetType == typeof(DateTime))
		{
			return (TValue)(object)value.Value.DateTime;
		}
		else if (targetType == typeof(DateTimeOffset))
		{
			return (TValue)(object)value.Value;
		}
		else if (targetType == typeof(DateOnly))
		{
			return (TValue)(object)DateOnly.FromDateTime(value.Value.DateTime);
		}
		else
		{
			throw new InvalidOperationException("Unsupported type.");
		}
	}

	internal static DateTime? GetDateTimeFromValue<TValue>(TValue value)
	{
		if (EqualityComparer<TValue>.Default.Equals(value, default))
		{
			return null;
		}

		switch (value)
		{
			case DateTime dateTimeValue:
				return dateTimeValue;
			case DateTimeOffset dateTimeOffsetValue:
				return dateTimeOffsetValue.DateTime;
			case DateOnly dateOnlyValue:
				return dateOnlyValue.ToDateTime(TimeOnly.MinValue);
			default:
				throw new InvalidOperationException("Unsupported type.");
		}
	}
}