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
	public class HxInputDate<TValue> : HxInputBaseWithInputGroups<TValue>
	{
		// DO NOT FORGET TO MAINTAIN DOCUMENTATION!
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(DateTime), typeof(DateTimeOffset) };

		private const string DateFormat = "yyyy-MM-dd"; // Compatible with HTML date inputs

		/// <summary>
		/// Gets or sets the error message used when displaying an a parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		[Inject] private protected IStringLocalizer<HxInputDate> StringLocalizer { get; set; }

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
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			EnsureInputId();

			RenderFragment<BlazorDateRangePicker.DateRangePicker> pickerTemplate = (BlazorDateRangePicker.DateRangePicker dateRangePicker) => (RenderTreeBuilder builder) =>
			{
				// default input in DateRangePicker:
				// <input id="@Id" type="text" @attributes="CombinedAttributes" value="@FormattedRange" @oninput="OnTextInput" @onfocusin="Open" @onfocusout="LostFocus" />

				builder.OpenElement(0, "input");
				BuildRenderInput_AddCommonAttributes(builder, "text"); // id, type, attributes (ale jiné)

				builder.AddAttribute(1000, "value", FormatValueAsString(Value));
				builder.AddAttribute(1001, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

				builder.AddAttribute(1002, "onfocusin", EventCallback.Factory.Create(this, dateRangePicker.Open));
				builder.AddAttribute(1003, "onfocusout", EventCallback.Factory.Create(this, dateRangePicker.LostFocus));

				builder.AddEventStopPropagationAttribute(1004, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxInputDate nastavitelné?
				
				builder.CloseElement();
			};

			builder.OpenComponent<BlazorDateRangePicker.DateRangePicker>(0);
			builder.AddAttribute(1, nameof(BlazorDateRangePicker.DateRangePicker.Id), InputId);
			builder.AddAttribute(2, nameof(BlazorDateRangePicker.DateRangePicker.PickerTemplate), pickerTemplate);
			builder.AddAttribute(3, nameof(BlazorDateRangePicker.DateRangePicker.SingleDatePicker), true);
			builder.AddAttribute(4, nameof(BlazorDateRangePicker.DateRangePicker.StartDateChanged), EventCallback.Factory.Create<DateTimeOffset?>(this, HandleStartDateChanged));

			builder.CloseComponent();
		}

		private async Task HandleStartDateChanged(DateTimeOffset? startDate)
		{
			Value = GetValueFromDateTimeOffset(startDate);
			await ValueChanged.InvokeAsync(Value);
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
				validationErrorMessage = GetParsingErrorMessage();
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

		/// <summary>
		/// Returns message for parsing error.
		/// </summary>
		protected virtual string GetParsingErrorMessage()
		{
			var message = this.ParsingErrorMessage ?? StringLocalizer["ParsingErrorMessage"];
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}
	}
}