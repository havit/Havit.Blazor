using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
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
		private const string Separator = "-";

		private protected override bool UseSingleDatePicker => false;
		
		[Inject] private IStringLocalizer<HxInputDateRange> StringLocalizer { get; set; }

		private protected override void BuildRenderInput_DateRangeValue(RenderTreeBuilder builder)
		{
			builder.AddAttribute(2001, nameof(BlazorDateRangePicker.DateRangePicker.StartDate), Value.StartDate != null ? new DateTimeOffset(Value.StartDate.Value) : null);
			builder.AddAttribute(2002, nameof(BlazorDateRangePicker.DateRangePicker.EndDate), Value.EndDate != null ? new DateTimeOffset(Value.EndDate.Value) : null);
		}

		protected override string FormatValueAsString(DateTimeRange value)
		{
			if ((Value.StartDate != null) && (Value.EndDate != null))
			{
				return $"{Value.StartDate.Value.ToShortDateString()} {Separator} {Value.EndDate.Value.ToShortDateString()}";
			}

			if (Value.StartDate != null)
			{
				return $"{StringLocalizer["StartDate"]} {Value.StartDate.Value.ToShortDateString()}";
			}

			if (Value.EndDate != null)
			{
				return $"{StringLocalizer["EndDate"]} {Value.EndDate.Value.ToShortDateString()}";
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

			string startDateText = StringLocalizer["StartDate"];
			string endDateText = StringLocalizer["EndDate"];

			if (TryParseValueFromString_StartDateEndDatePattern(value, cultureInfo, startDateText, endDateText, out result)
				|| TryParseValueFromString_StartDatePattern(value, cultureInfo, startDateText, out result)
				|| TryParseValueFromString_EndDatePattern(value, cultureInfo, endDateText, out result)
				|| TryParseValueFromString_NoDatePattern(value, out result))
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

		internal static bool TryParseValueFromString_StartDateEndDatePattern(string value, CultureInfo culture, string startDateText, string endDateText, out DateTimeRange result)
		{
			StringPatternizer sp = new StringPatternizer();
			sp.Markers.Add("XXstartDateXX", typeof(string));
			sp.Markers.Add("XXendDateXX", typeof(string));

			sp.Patterns.Add("XXstartDateXX-XXendDateXX");
			sp.Patterns.Add($"{startDateText}XXstartDateXX{endDateText}XXendDateXX");

			var patterns = sp.Match(value);
			if (patterns.Exception == null) // fuj fuj API to má...
			{
				string startDateString = patterns.MarkerHasValue("XXstartDateXX") ? patterns.GetMarkerValue<string>("XXstartDateXX") : null;
				string endDateString = patterns.MarkerHasValue("XXendDateXX") ? patterns.GetMarkerValue<string>("XXendDateXX") : null;

				if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(startDateString, culture, out DateTimeOffset? startDate)
					&& HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(endDateString, culture, out DateTimeOffset? endDate))
				{
					result = new DateTimeRange
					{
						StartDate = ((startDate != null) && (endDate != null)) ? DateTimeExt.Min(startDate.Value.DateTime, endDate.Value.DateTime) : startDate?.DateTime,
						EndDate = ((startDate != null) && (endDate != null)) ? DateTimeExt.Max(startDate.Value.DateTime, endDate.Value.DateTime) : endDate?.DateTime
					};
					return true;
				}
			}

			result = default;
			return false;
		}

		internal static bool TryParseValueFromString_StartDatePattern(string value, CultureInfo culture, string startDateText, out DateTimeRange result)
		{
			StringPatternizer sp = new StringPatternizer();
			sp.Markers.Add("XXstartDateXX", typeof(string));

			sp.Patterns.Add("XXstartDateXX-");
			sp.Patterns.Add($"{startDateText}XXstartDateXX");

			var patterns = sp.Match(value);
			if (patterns.Exception == null) // fuj fuj API to má...
			{
				string startDateString = patterns.GetMarkerValue<string>("XXstartDateXX");

				if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(startDateString, culture, out DateTimeOffset? startDate))					
				{
					result = new DateTimeRange
					{
						StartDate = startDate?.DateTime,
						EndDate = null
					};
					return true;
				}
			}

			result = default;
			return false;
		}

		internal static bool TryParseValueFromString_EndDatePattern(string value, CultureInfo culture, string endDateText, out DateTimeRange result)
		{
			StringPatternizer sp = new StringPatternizer();
			sp.Markers.Add("XXendDateXX", typeof(string));

			sp.Patterns.Add("-XXendDateXX");
			sp.Patterns.Add($"{endDateText}XXendDateXX");

			var patterns = sp.Match(value);
			if (patterns.Exception == null) // fuj fuj API to má...
			{
				string endDateString = patterns.GetMarkerValue<string>("XXendDateXX");

				if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(endDateString, culture, out DateTimeOffset? endDate))
				{
					result = new DateTimeRange
					{
						StartDate = null,
						EndDate = endDate?.DateTime
					};
					return true;
				}
			}

			result = default;
			return false;
		}

		internal static bool TryParseValueFromString_NoDatePattern(string value, out DateTimeRange result)
		{
			if (String.IsNullOrWhiteSpace(value) || (value.Trim() == "-"))
			{
				result = new DateTimeRange
				{
					StartDate = null,
					EndDate = null
				};
				return true;
			}

			result = default;
			return false;
		}

	}
}