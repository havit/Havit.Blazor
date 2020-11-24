using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date input.
	/// </summary>
	/// <typeparam name="TValue">Supports DateTime and DateTimeOffset.</typeparam>
	public class HxInputDate<TValue> : HxInputBaseWithInputGroups<TValue>
	{
		private const string DateFormat = "yyyy-MM-dd"; // Compatible with HTML date inputs

		/// <summary>
		/// Gets or sets the error message used when displaying an a parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "input");
			
			BuildRenderInput_AddCommonAttributes(builder, "date");

			builder.AddAttribute(1000, "value", FormatValueAsString(Value));
			builder.AddAttribute(1001, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			
			builder.CloseElement();
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
					return BindConverter.FormatValue(dateTimeValue, DateFormat, CultureInfo.InvariantCulture);
				case DateTimeOffset dateTimeOffsetValue:
					return BindConverter.FormatValue(dateTimeOffsetValue, DateFormat, CultureInfo.InvariantCulture);
				default:
					return string.Empty; // Handles null for Nullable<DateTime>, etc.
			}
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			// Unwrap nullable types. We don't have to deal with receiving empty values for nullable
			// types here, because the underlying InputBase already covers that.
			var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

			bool success;
			if (targetType == typeof(DateTime))
			{
				success = TryParseDateTime(value, out result);
			}
			else if (targetType == typeof(DateTimeOffset))
			{
				success = TryParseDateTimeOffset(value, out result);
			}
			else
			{
				throw new InvalidOperationException($"The type '{targetType}' is not a supported date type.");
			}

			if (success)
			{
				validationErrorMessage = null;
				return true;
			}
			else
			{
				validationErrorMessage = GetParsingErrorMessage();
				return false;
			}
		}

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			CheckParsingErrorMessage();
		}

		private static bool TryParseDateTime(string value, out TValue result)
		{
			var success = BindConverter.TryConvertToDateTime(value, CultureInfo.InvariantCulture, DateFormat, out var parsedValue);
			if (success)
			{
				result = (TValue)(object)parsedValue;
				return parsedValue != default(DateTime);
			}
			else
			{
				result = default;
				return false;
			}
		}

		private static bool TryParseDateTimeOffset(string value, out TValue result)
		{
			var success = BindConverter.TryConvertToDateTimeOffset(value, CultureInfo.InvariantCulture, DateFormat, out var parsedValue);
			if (success)
			{
				result = (TValue)(object)parsedValue;
				return parsedValue != default(DateTimeOffset);
			}
			else
			{
				result = default;
				return false;
			}
		}

		/// <summary>
		/// Returns message for parsing error.
		/// </summary>
		protected virtual string GetParsingErrorMessage()
		{
			return String.Format(ParsingErrorMessage, Label, FieldIdentifier.FieldName);
		}

		/// <summary>
		/// Checks message for parsing error is set.
		/// If not set, throws <see cref="InvalidOperationException"/>.
		/// </summary>
		protected virtual void CheckParsingErrorMessage()
		{
			if (String.IsNullOrEmpty(ParsingErrorMessage))
			{
				throw new InvalidOperationException($"Missing {nameof(ParsingErrorMessage)} property value on {GetType()}.");
			}
		}

	}
}