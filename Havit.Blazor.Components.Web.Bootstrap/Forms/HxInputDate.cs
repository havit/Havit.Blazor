using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Predefined date ranges
	// TODO: DisableDates
	// TODO: HighlightDates

	/// <summary>
	/// Date input.
	/// Uses a <see href="https://github.com/jdtcn/BlazorDateRangePicker">DateRangePicker</see>, follow the Get Started guide!
	/// </summary>
	/// <typeparam name="TValue">Supports DateTime and DateTimeOffset.</typeparam>
	public class HxInputDate<TValue> : HxInputDateBase<TValue>
	{
		// DO NOT FORGET TO MAINTAIN DOCUMENTATION!
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(DateTime), typeof(DateTimeOffset) };

		[Inject] private IStringLocalizer<HxInputDate> StringLocalizer { get; set; }

		private protected override bool UseSingleDatePicker => true;

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

		private protected override async Task HandleStartDateChanged(DateTimeOffset? startDate)
		{
			Value = GetValueFromDateTimeOffset(startDate);
			await ValueChanged.InvokeAsync(Value);
			// TODO: notify change!
		}

		private protected override Task HandleEndDateChanged(DateTimeOffset? startDate)
		{
			throw new NotSupportedException();
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
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
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

			bool success = DateTimeOffset.TryParse(value, culture, DateTimeStyles.AllowWhiteSpaces, out DateTimeOffset parsedValue) && (parsedValue.TimeOfDay == TimeSpan.Zero);
			if (success)
			{
				result = parsedValue;
				return true;
			}

			result = null;
			return false;
		}
	}
}