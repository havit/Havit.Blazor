using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date input.
	/// Uses a <see href="https://github.com/jdtcn/BlazorDateRangePicker">DateRangePicker</see>, follow the Get Started guide!
	/// </summary>
	/// <typeparam name="TValue">Supports DateTime and DateTimeOffset.</typeparam>
	public class HxInputDate<TValue> : HxInputDateBase<TValue>
	{
		// DO NOT FORGET TO MAINTAIN DOCUMENTATION!
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(DateTime), typeof(DateTimeOffset) };

		/// <summary>
		/// When true, uses default dates (today).
		/// </summary>
		[Parameter] public bool UseDefaultDates { get; set; } = true;

		/// <summary>
		/// Custom dates. When <see cref="UseDefaultDates"/> is true, these items are used with default items.
		/// </summary>
		[Parameter] public IEnumerable<DateItem> Dates { get; set; }

		[Inject] private IStringLocalizer<HxInputDate> StringLocalizer { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxInputDate()
		{
			Type undelyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
			if (!supportedTypes.Contains(undelyingType))
			{
				throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
			}
		}

		private protected override void BuildRenderInput_DateRangePickerAttributes(RenderTreeBuilder builder)
		{
			DateTimeOffset? startDate;
			if (EqualityComparer<TValue>.Default.Equals(Value, default))
			{
				startDate = null;
			}
			else
			{
				switch (Value)
				{
					case DateTime dateTimeValue:
						startDate = new DateTimeOffset(dateTimeValue);
						break;

					case DateTimeOffset dateTimeOffsetValue:
						startDate = dateTimeOffsetValue;
						break;

					default:
						throw new InvalidOperationException("Unsupported type.");
				}
			}

			builder.AddAttribute(2001, nameof(BlazorDateRangePicker.DateRangePicker.StartDate), startDate);
			builder.AddAttribute(2002, nameof(BlazorDateRangePicker.DateRangePicker.EndDate), (DateTimeOffset?)null);
			builder.AddAttribute(2003, nameof(BlazorDateRangePicker.DateRangePicker.SingleDatePicker), true);
			builder.AddAttribute(2004, nameof(BlazorDateRangePicker.DateRangePicker.StartDateChanged), EventCallback.Factory.Create<DateTimeOffset?>(this, HandleStartDateChanged));

			Dictionary<string, BlazorDateRangePicker.DateRange> dateRanges = GetDateRanges();
			if ((dateRanges != null) && dateRanges.Any())
			{
				builder.AddAttribute(2005, nameof(BlazorDateRangePicker.DateRangePicker.Ranges), dateRanges);
				// no DateRangeChanged event, just StartDateChanged above
			}
		}

		/// <inheritdoc />
		protected override string FormatValueAsString(TValue value)
		{
			// nenabízíme hodnotu 1.1.0001, atp.
			if (EqualityComparer<TValue>.Default.Equals(value, default))
			{
				return null;
			}

			switch (value)
			{
				case DateTime dateTimeValue:
					return dateTimeValue.ToShortDateString();
				case DateTimeOffset dateTimeOffsetValue:
					return dateTimeOffsetValue.DateTime.ToShortDateString();
				default:
					throw new InvalidOperationException("Unsupported type.");

			}
		}

		private void HandleStartDateChanged(DateTimeOffset? startDate)
		{
			CurrentValue = GetValueFromDateTimeOffset(startDate); // setter includes ValueChanged + NotifyFieldChanged
		}

		internal static TValue GetValueFromDateTimeOffset(DateTimeOffset? value)
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

		/// <inheritdoc />
		protected override bool TryParseValueFromStringCore(string value, out TValue result, out string validationErrorMessage)
		{
			bool success = TryParseDateTimeOffsetFromString(value, CultureInfo.CurrentCulture, out DateTimeOffset? parsedValue);

			if (success)
			{
				result = GetValueFromDateTimeOffset(parsedValue);
				validationErrorMessage = null;
				return true;
			}
			else
			{
				result = default;
				validationErrorMessage = GetParsingErrorMessage(StringLocalizer);
				return false;
			}
		}

		// for easy testability
		internal static bool TryParseDateTimeOffsetFromString(string value, CultureInfo culture, out DateTimeOffset? result)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				result = null;
				return true;
			}

			// it also works for "invalid dates" (with dots, commas, spaces...)
			// ie. 09,09,2020 --> 9.9.2020
			// ie. 09 09 2020 --> 9.9.2020
			// ie. 06,05, --> 6.5.ThisYear
			// ie. 06 05 --> 6.5.ThisYear
			bool success = DateTimeOffset.TryParse(value, culture, DateTimeStyles.AllowWhiteSpaces, out DateTimeOffset parsedValue) && (parsedValue.TimeOfDay == TimeSpan.Zero);
			if (success)
			{
				result = parsedValue;
				return true;
			}

			Match match;

			// 0105 --> 01.05.ThisYear
			match = Regex.Match(value, "^(\\d{2})(\\d{2})$");
			if (match.Success)
			{
				if (int.TryParse(match.Groups[1].Value, out int day)
					&& int.TryParse(match.Groups[2].Value, out int month))
				{
					try
					{
						result = new DateTimeOffset(new DateTime(DateTime.Now.Year, month, day));
						return true;
					}
					catch (ArgumentOutOfRangeException)
					{
						// NOOP
					}
				}
				result = null;
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
						result = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day));
						return true;
					}
					catch (ArgumentOutOfRangeException)
					{
						// NOOP
					}
				}
				result = null;
				return false;
			}

			result = null;
			return false;
		}

		private Dictionary<string, BlazorDateRangePicker.DateRange> GetDateRanges()
		{
			Dictionary<string, BlazorDateRangePicker.DateRange> result = null;

			if (Dates != null)
			{
				result = new Dictionary<string, BlazorDateRangePicker.DateRange>();

				foreach (DateItem dateItem in Dates)
				{
					result[dateItem.Label] = new BlazorDateRangePicker.DateRange
					{
						Start = new DateTimeOffset(dateItem.Date),
						End = default
					};
				}
			}

			if (UseDefaultDates)
			{
				result ??= new Dictionary<string, BlazorDateRangePicker.DateRange>();

				DateTimeOffset today = DateTimeOffset.Now.Date;

				result[StringLocalizer["Today"]] = new BlazorDateRangePicker.DateRange { Start = today };
			}

			return result;
		}

	}
}