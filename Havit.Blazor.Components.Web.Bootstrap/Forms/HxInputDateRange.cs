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
	/// Date range input.
	/// Uses a <see href="https://github.com/jdtcn/BlazorDateRangePicker">DateRangePicker</see>, follow the Get Started guide!
	/// </summary>
	public class HxInputDateRange : HxInputDateBase<DateTimeRange>
	{
		private protected override bool UseSingleDatePicker => false;
		
		[Inject] private IStringLocalizer<HxInputDateRange> StringLocalizer { get; set; }

		protected override string FormatValueAsString(DateTimeRange value)
		{
			if ((Value.StartDate != null) && (Value.EndDate != null))
			{
				return $"{Value.StartDate.Value.ToShortDateString()} - {Value.StartDate.Value.ToShortDateString()}";
			}

			if (Value.StartDate != null)
			{
				return $"{StringLocalizer["od"]} {Value.StartDate.Value.ToShortDateString()}";
			}

			if (Value.EndDate != null)
			{
				return $"{StringLocalizer["do"]} {Value.EndDate.Value.ToShortDateString()}";
			}

			return String.Empty;
		}

		private protected override async Task HandleStartDateChanged(DateTimeOffset? startDate)
		{
			Value = new DateTimeRange
			{
				StartDate = startDate?.DateTime,
				EndDate = Value.EndDate
			};
			await ValueChanged.InvokeAsync(Value);
			// notify change!
		}

		private protected override async Task HandleEndDateChanged(DateTimeOffset? endDate)
		{
			Value = new DateTimeRange
			{
				StartDate = Value.StartDate,
				EndDate = endDate?.DateTime
			};
			await ValueChanged.InvokeAsync(Value);
			// notify change!
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out DateTimeRange result, out string validationErrorMessage)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				result = new DateTimeRange();
				validationErrorMessage = null;
				return true;
			}

			CultureInfo cultureInfo = CultureInfo.CurrentCulture;

			if (TryParseValueFromString_SeparatorPattern(value, cultureInfo, out result)
				|| TryParseValueFromString_FromToPattern(value, cultureInfo, out result)
				|| TryParseValueFromString_FromPattern(value, cultureInfo, out result)
				|| TryParseValueFromString_ToPattern(value, cultureInfo, out result)
				|| TryParseValueFromString_SingleDatePattern(value, cultureInfo, out result))
			{
				validationErrorMessage = null;
				return true;
			}
			else
			{
				result = new DateTimeRange();
				validationErrorMessage = GetParsingErrorMessage(StringLocalizer);
				return false;
			}
		}

		// TODO: Unit test
		internal static bool TryParseValueFromString_SeparatorPattern(string value, CultureInfo cultureInfo, out DateTimeRange result)
		{
			result = default;
			return false;
		}

		// TODO: Unit test
		internal static bool TryParseValueFromString_FromToPattern(string value, CultureInfo cultureInfo, out DateTimeRange result)
		{
			result = default;
			return false;
		}

		// TODO: Unit test
		internal static bool TryParseValueFromString_FromPattern(string value, CultureInfo cultureInfo, out DateTimeRange result)
		{
			result = default;
			return false;
		}

		// TODO: Unit test
		internal static bool TryParseValueFromString_ToPattern(string value, CultureInfo cultureInfo, out DateTimeRange result)
		{
			result = default;
			return false;
		}

		// TODO: Unit test
		internal static bool TryParseValueFromString_SingleDatePattern(string value, CultureInfo cultureInfo, out DateTimeRange result)
		{
			result = default;
			return false;
		}

	}
}