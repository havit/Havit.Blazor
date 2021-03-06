﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date range input.
	/// </summary>
	public class HxInputDateRange : HxInputBase<DateTimeRange>
	{
		public static List<DateRangeItem> DefaultDateRanges { get; set; }

		/// <summary>
		/// When true, uses default date ranges (this month, last month, this year, last year).
		/// </summary>
		[Parameter] public bool UseDefaultDateRanges { get; set; } = true;

		/// <summary>
		/// Custom date ranges. When <see cref="UseDefaultDateRanges"/> is true, these items are used with default items.
		/// </summary>
		[Parameter] public IEnumerable<DateRangeItem> CustomDateRanges { get; set; }

		/// <summary>
		/// Gets or sets the error message used when displaying an a &quot;from&quot; parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string FromParsingErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the error message used when displaying an a &quot;to&quot; parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ToParsingErrorMessage { get; set; }

		[Inject] private IStringLocalizer<HxInputDateRange> StringLocalizer { get; set; }

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenComponent(1, typeof(HxInputDateRangeInternal));

			builder.AddAttribute(100, nameof(HxInputDateRangeInternal.Value), Value);
			builder.AddAttribute(101, nameof(HxInputDateRangeInternal.ValueChanged), ValueChanged);
			builder.AddAttribute(102, nameof(HxInputDateRangeInternal.ValueExpression), ValueExpression);

			builder.AddAttribute(200, nameof(HxInputDateRangeInternal.FromInputId), InputId);
			builder.AddAttribute(201, nameof(HxInputDateRangeInternal.InputCssClass), InputCssClass);
			builder.AddAttribute(202, nameof(HxInputDateRangeInternal.EnabledEffective), EnabledEffective);
			builder.AddAttribute(203, nameof(HxInputDateRangeInternal.FromParsingErrorMessageEffective), GetFromParsingErrorMessage());
			builder.AddAttribute(204, nameof(HxInputDateRangeInternal.ToParsingErrorMessageEffective), GetToParsingErrorMessage());
			builder.AddAttribute(205, nameof(HxInputDateRangeInternal.ShowValidationMessage), ShowValidationMessage);
			builder.AddAttribute(206, nameof(HxInputDateRangeInternal.CustomDateRanges), GetCustomDateRanges().ToList());

			builder.CloseComponent();
		}

		protected override void BuildRenderValidationMessage(RenderTreeBuilder builder)
		{
			// NOOP
		}

		private protected override void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
		{
			throw new NotSupportedException();
		}

		// For generating chips
		/// <inheritdocs />
		protected override string FormatValueAsString(DateTimeRange value)
		{
			string from = null;
			string to = null;

			if (value.StartDate != null)
			{
				from = StringLocalizer["From"] + " " + value.StartDate.Value.ToShortDateString();
			}

			if (value.EndDate != null)
			{
				to = StringLocalizer["To"] + " " + value.EndDate.Value.ToShortDateString();
			}

			return String.Join(" ", from, to);
		}

		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out DateTimeRange result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private IEnumerable<DateRangeItem> GetCustomDateRanges()
		{
			if (CustomDateRanges != null)
			{
				foreach (DateRangeItem dateRangeItem in CustomDateRanges)
				{
					yield return dateRangeItem;
				}
			}

			if (UseDefaultDateRanges)
			{
				if (DefaultDateRanges != null)
				{
					foreach (DateRangeItem defaultDateRangeItem in DefaultDateRanges)
					{
						yield return defaultDateRangeItem;
					}
				}
				else
				{
					DateTime today = DateTime.Today;

					DateTime thisMonthStart = new DateTime(today.Year, today.Month, 1);
					DateTime thisMonthEnd = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
					DateTime lastMonthStart = thisMonthStart.AddMonths(-1);
					DateTime lastMonthEnd = new DateTime(lastMonthStart.Year, lastMonthStart.Month, DateTime.DaysInMonth(lastMonthStart.Year, lastMonthStart.Month));
					DateTime thisYearStart = new DateTime(today.Year, 1, 1);
					DateTime thisYearEnd = new DateTime(today.Year, 12, 31);
					DateTime lastYearStart = thisYearStart.AddYears(-1);
					DateTime lastYearEnd = thisYearEnd.AddYears(-1);

					yield return new DateRangeItem { Label = StringLocalizer["ThisMonth"], DateRange = new DateTimeRange { StartDate = thisMonthStart, EndDate = thisMonthEnd } };
					yield return new DateRangeItem { Label = StringLocalizer["LastMonth"], DateRange = new DateTimeRange { StartDate = lastMonthStart, EndDate = lastMonthEnd } };
					yield return new DateRangeItem { Label = StringLocalizer["ThisYear"], DateRange = new DateTimeRange { StartDate = thisYearStart, EndDate = thisYearEnd } };
					yield return new DateRangeItem { Label = StringLocalizer["LastYear"], DateRange = new DateTimeRange { StartDate = lastYearStart, EndDate = lastYearEnd } };
				}
			}
		}

		/// <summary>
		/// Returns message for &quot;from&quot; parsing error.
		/// </summary>
		protected virtual string GetFromParsingErrorMessage()
		{
			var message = !String.IsNullOrEmpty(FromParsingErrorMessage)
				? FromParsingErrorMessage
				: StringLocalizer["FromParsingErrorMessage"];
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}

		/// <summary>
		/// Returns message for &quot;to&quot;  parsing error.
		/// </summary>
		protected virtual string GetToParsingErrorMessage()
		{
			var message = !String.IsNullOrEmpty(ToParsingErrorMessage)
				? ToParsingErrorMessage
				: StringLocalizer["ToParsingErrorMessage"];
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}
	}
}