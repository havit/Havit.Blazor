using System.Globalization;
using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;

internal static class DateHelper
{
	internal static bool TryParseDateFromString<TValue>(string value, TimeProvider timeProvider, out TValue result)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			result = default;
			bool isNullable = (Nullable.GetUnderlyingType(typeof(TValue)) != null);
			return isNullable;
		}

		// it also works for "invalid dates" (with dots, commas, spaces...)
		// ie. 09,09,2020 --> 9.9.2020
		// ie. 09 09 2020 --> 9.9.2020
		// ie. 06,05, --> 6.5.ThisYear
		// ie. 06 05 --> 6.5.ThisYear
		if (DateTimeOffset.TryParse(value, formatProvider: null, DateTimeStyles.AllowWhiteSpaces, out DateTimeOffset parsedValue) && (parsedValue.TimeOfDay == TimeSpan.Zero))
		{
			result = GetValueFromDateTimeOffset<TValue>(parsedValue);
			return true;
		}

		// expecting date format with day, month, and year components
		int dayIndex = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.IndexOf("d");
		int monthIndex = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.IndexOf("M");
		int yearIndex = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.IndexOf("y");

		if ((dayIndex < 0) || (monthIndex < 0) || (yearIndex < 0))
		{
			result = default;
			return false;
		}

		Match match;

		// 010520 --> 01.05.2020
		match = Regex.Match(value, "^(\\d{2})(\\d{2})(\\d{2})$");
		if (match.Success)
		{
			if (int.TryParse(match.Groups[1].Value, out int part1)
				&& int.TryParse(match.Groups[2].Value, out int part2)
				&& int.TryParse(match.Groups[3].Value, out int part3))
			{
				// we need to assign the parts to day, month, and year based on the current culture's date format
				var indexedDateComponents = new List<(int, char)>
				{
					(dayIndex, 'd'),
					(monthIndex, 'M'),
					(yearIndex, 'y')
				};

				int day;
				int month;
				int year;

				string dateComponents = String.Join("", indexedDateComponents.OrderBy(item => item.Item1).Select(item => item.Item2));
				switch (dateComponents)
				{
					case "dMy":
						day = part1;
						month = part2;
						year = part3;
						break;

					case "Mdy":
						month = part1;
						day = part2;
						year = part3;
						break;

					case "yMd":
						year = part1;
						month = part2;
						day = part3;
						break;

					case "ydM":
						year = part1;
						day = part2;
						month = part3;
						break;

					default:
						result = default;
						return false;
				}

				year += year < 50 ? 2000 : 1900; // Adjust year to be in the correct century
				try
				{
					result = GetValueFromDateTimeOffset<TValue>(new DateTimeOffset(new DateTime(year, month, day)));
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

		// 0105 --> 01.05.ThisYear
		match = Regex.Match(value, "^(\\d{2})(\\d{2})$");
		if (match.Success)
		{

			if (int.TryParse(match.Groups[1].Value, out int part1)
				&& int.TryParse(match.Groups[2].Value, out int part2))
			{
				try
				{
					int day;
					int month;

					if (dayIndex > monthIndex)
					{
						month = part1;
						day = part2;
					}
					else
					{
						day = part1;
						month = part2;
					}

					result = GetValueFromDateTimeOffset<TValue>(new DateTimeOffset(new DateTime(timeProvider.GetLocalNow().Year, month, day)));
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

		// 01 --> 01.ThisMonth.ThisYear
		match = Regex.Match(value, "^(\\d{2})$");
		if (match.Success)
		{
			if (int.TryParse(match.Groups[1].Value, out int day))
			{
				try
				{
					var localNow = timeProvider.GetLocalNow();
					result = GetValueFromDateTimeOffset<TValue>(new DateTimeOffset(new DateTime(localNow.Year, localNow.Month, day)));
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
			default:
				throw new InvalidOperationException("Unsupported type.");
		}
	}
}