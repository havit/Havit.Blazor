﻿using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date range picker. Form input component for entering start date and end date.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputDateRange">https://havit.blazor.eu/components/HxInputDateRange</see>
	/// </summary>
	public class HxInputDateRange : HxInputBase<DateTimeRange>, IInputWithSize
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputDateRange"/>.
		/// </summary>
		public static InputDateRangeSettings Defaults { get; set; }

		static HxInputDateRange()
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

			Defaults = new InputDateRangeSettings()
			{
				InputSize = Bootstrap.InputSize.Regular,
				MinDate = HxCalendar.DefaultMinDate,
				MaxDate = HxCalendar.DefaultMaxDate,
				ShowCalendarButtons = true,
				ShowPredefinedDateRanges = true,
				PredefinedDateRanges = new[]
				{
					new InputDateRangePredefinedRangesItem { Label = "ThisMonth", DateRange = new DateTimeRange(thisMonthStart, thisMonthEnd), ResourceType = typeof(HxInputDateRange) },
					new InputDateRangePredefinedRangesItem { Label = "LastMonth", DateRange = new DateTimeRange(lastMonthStart, lastMonthEnd), ResourceType = typeof(HxInputDateRange) },
					new InputDateRangePredefinedRangesItem { Label = "ThisYear", DateRange = new DateTimeRange(thisYearStart, thisYearEnd), ResourceType = typeof(HxInputDateRange) },
					new InputDateRangePredefinedRangesItem { Label = "LastYear", DateRange = new DateTimeRange(lastYearStart, lastYearEnd), ResourceType = typeof(HxInputDateRange) },
				}
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual InputDateRangeSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public InputDateRangeSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual InputDateRangeSettings GetSettings() => this.Settings;


		/// <summary>
		/// When enabled (default is <c>true</c>), shows predefined days (from <see cref="PredefinedDateRanges"/>, e.g. Today).
		/// </summary>
		[Parameter] public bool? ShowPredefinedDateRanges { get; set; }
		protected bool ShowPredefinedDateRangesEffective => this.ShowPredefinedDateRanges ?? this.Settings?.ShowPredefinedDateRanges ?? GetDefaults().ShowPredefinedDateRanges ?? throw new InvalidOperationException(nameof(ShowPredefinedDateRanges) + " default for " + nameof(HxInputDateRange) + " has to be set.");

		/// <summary>
		/// Predefined dates to be displayed.
		/// </summary>
		[Parameter] public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRanges { get; set; }
		private IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRangesEffective => this.PredefinedDateRanges ?? this.GetSettings()?.PredefinedDateRanges ?? GetDefaults().PredefinedDateRanges;

		/// <summary>
		/// Size of the input.
		/// </summary>
		[Parameter] public InputSize? InputSize { get; set; }
		protected InputSize InputSizeEffective => this.InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxInputDateRange) + " has to be set.");
		InputSize IInputWithSize.InputSizeEffective => this.InputSizeEffective;

		/// <summary>
		/// Gets or sets the error message used when displaying an a &quot;from&quot; parsing error.
		/// Used with <c>String.Format(...)</c>, <c>{0}</c> is replaced by Label property, <c>{1}</c> name of bounded property.
		/// </summary>
		[Parameter] public string FromParsingErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the error message used when displaying an a &quot;to&quot; parsing error.
		/// Used with <c>String.Format(...)</c>, <c>{0}</c> is replaced by Label property, <c>{1}</c> name of bounded property.
		/// </summary>
		[Parameter] public string ToParsingErrorMessage { get; set; }

		/// <summary>
		/// Indicates whether the <i>Clear</i> and <i>OK</i> buttons in calendars should be visible.<br/>
		/// Default is <c>true</c> (configurable in <see cref="HxInputDateRange.Defaults"/>).
		/// </summary>
		[Parameter] public bool? ShowCalendarButtons { get; set; }
		protected bool ShowCalendarButtonsEffective => this.ShowCalendarButtons ?? this.GetSettings()?.ShowCalendarButtons ?? this.GetDefaults().ShowCalendarButtons ?? throw new InvalidOperationException(nameof(ShowCalendarButtons) + " default for " + nameof(HxInputDateRange) + " has to be set.");

		/// <summary>
		/// First date selectable from the dropdown calendar.<br />
		/// Default is <c>1.1.1900</c> (configurable from <see cref="HxInputDateRange.Defaults"/>).
		/// </summary>
		[Parameter] public DateTime? MinDate { get; set; }
		protected DateTime MinDateEffective => this.MinDate ?? this.GetSettings()?.MinDate ?? GetDefaults().MinDate ?? throw new InvalidOperationException(nameof(MinDate) + " default for " + nameof(HxInputDateRange) + " has to be set.");

		/// <summary>
		/// Last date selectable from the dropdown calendar.<br />
		/// Default is <c>31.12.2099</c> (configurable from <see cref="HxInputDateRange.Defaults"/>).
		/// </summary>
		[Parameter] public DateTime? MaxDate { get; set; }
		protected DateTime MaxDateEffective => this.MaxDate ?? this.GetSettings()?.MaxDate ?? this.GetDefaults().MaxDate ?? throw new InvalidOperationException(nameof(MaxDate) + " default for " + nameof(HxInputDateRange) + " has to be set.");

		/// <summary>
		/// Allows customization of the dates in dropdown calendars.<br />
		/// Default customization is configurable with <see cref="HxInputDateRange.Defaults"/>.
		/// </summary>
		[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProvider { get; set; }
		protected CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective => this.CalendarDateCustomizationProvider ?? this.GetSettings()?.CalendarDateCustomizationProvider ?? GetDefaults().CalendarDateCustomizationProvider;

		[Inject] private IStringLocalizer<HxInputDateRange> StringLocalizer { get; set; }

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			RenderWithAutoCreatedEditContextAsCascandingValue(builder, 0, BuildRenderInputCore);
		}

		protected virtual void BuildRenderInputCore(RenderTreeBuilder builder)
		{
			builder.OpenComponent(1, typeof(HxInputDateRangeInternal));

			builder.AddAttribute(100, nameof(HxInputDateRangeInternal.Value), Value);
			builder.AddAttribute(101, nameof(HxInputDateRangeInternal.ValueChanged), EventCallback.Factory.Create<DateTimeRange>(this, value => CurrentValue = value));
			builder.AddAttribute(102, nameof(HxInputDateRangeInternal.ValueExpression), ValueExpression);

			builder.AddAttribute(200, nameof(HxInputDateRangeInternal.FromInputId), InputId);
			builder.AddAttribute(201, nameof(HxInputDateRangeInternal.InputCssClass), GetInputCssClassToRender());
			builder.AddAttribute(202, nameof(HxInputDateRangeInternal.InvalidCssClass), InvalidCssClass);
			builder.AddAttribute(203, nameof(HxInputDateRangeInternal.InputSizeEffective), this.InputSizeEffective);
			builder.AddAttribute(204, nameof(HxInputDateRangeInternal.EnabledEffective), EnabledEffective);
			builder.AddAttribute(205, nameof(HxInputDateRangeInternal.FromParsingErrorMessageEffective), GetFromParsingErrorMessage());
			builder.AddAttribute(206, nameof(HxInputDateRangeInternal.ToParsingErrorMessageEffective), GetToParsingErrorMessage());
			builder.AddAttribute(207, nameof(HxInputDateRangeInternal.ShowValidationMessage), ShowValidationMessage);
			builder.AddAttribute(208, nameof(HxInputDateRangeInternal.PredefinedDateRangesEffective), this.PredefinedDateRangesEffective);
			builder.AddAttribute(209, nameof(HxInputDateRangeInternal.ShowPredefinedDateRangesEffective), this.ShowPredefinedDateRangesEffective);
			builder.AddAttribute(210, nameof(HxInputDateRangeInternal.ShowCalendarButtonsEffective), ShowCalendarButtonsEffective);
			builder.AddAttribute(211, nameof(HxInputDateRangeInternal.MinDateEffective), MinDateEffective);
			builder.AddAttribute(212, nameof(HxInputDateRangeInternal.MaxDateEffective), MaxDateEffective);
			builder.AddAttribute(220, nameof(HxInputDateRangeInternal.CalendarDateCustomizationProviderEffective), CalendarDateCustomizationProviderEffective);

			builder.AddMultipleAttributes(300, this.AdditionalAttributes);

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