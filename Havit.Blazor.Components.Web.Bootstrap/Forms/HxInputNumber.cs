using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Numeric input.
	/// </summary>
	/// <typeparam name="TValue">
	/// Supported values: <c>int (Int32), long (Int64), float (Single), double, decimal</c>.
	/// </typeparam>
	public class HxInputNumber<TValue> : HxInputBaseWithInputGroups<TValue>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
	{
		// DO NOT FORGET TO MAINTAIN DOCUMENTATION!
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };

		/// <summary>
		/// Gets or sets the error message used when displaying an a parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		/// <summary>
		/// Placeholder for the input.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		/// <inheritdoc cref="Bootstrap.InputSize" />
		[Parameter] public InputSize? InputSize { get; set; }

		/// <inheritdoc cref="Bootstrap.LabelType" />
		[Parameter] public LabelType? LabelType { get; set; }

		/// <summary>
		/// Gets or sets the number of decimal digits.
		/// Can be used only for floating point types, for integer types throws exception.
		/// When not set, 2 decimal digits are used.
		/// </summary>
		[Parameter]
		public int? Decimals
		{
			get
			{
				return decimals;
			}
			set
			{
				if (IsTValueIntegerType)
				{
					throw new InvalidOperationException($"{nameof(Decimals)} can be set only on floating point types (not on integer types).");
				}
				decimals = value;
			}
		}
		private int? decimals;

		/// <summary>
		/// Gets effective value for Decimals (when not set gets 0 for integer types and 2 for floating point types.
		/// </summary>
		protected virtual int DecimalsEffective => Decimals ?? (IsTValueIntegerType ? 0 : 2);

		[Inject] private protected IStringLocalizer<HxInputNumber> StringLocalizer { get; set; }

		/// <summary>
		/// Returns <c>true</c> for integer types (<c>false</c> for floating point types).
		/// </summary>
		private bool IsTValueIntegerType
		{
			get
			{
				Type undelyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
				return (undelyingType == typeof(int))
					|| (undelyingType == typeof(long));
			}
		}

		/// <summary>
		/// Return <see cref="HxInputNumber{TValue}"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual InputNumberDefaults GetDefaults() => HxInputNumber.Defaults;
		IInputDefaultsWithSize IInputWithSize.GetDefaults() => GetDefaults(); // might be replaced with C# vNext convariant return types on interfaces

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxInputNumber()
		{
			Type undelyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
			if (!supportedTypes.Contains(undelyingType))
			{
				throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
			}
		}

		protected bool forceRenderValue = false;
		private int valueSequenceOffset = 0;

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "input");
			BuildRenderInput_AddCommonAttributes(builder, "text");

			// We can use inputmode numeric when we do not want to enter decimals. It displays a keyboard without a key for a decimal point.
			builder.AddAttribute(1001, "inputmode", DecimalsEffective <= 0 ? "numeric" : "decimal")

			builder.AddAttribute(1002, "onfocus", "this.select();"); // source: https://stackoverflow.com/questions/4067469/selecting-all-text-in-html-text-input-when-clicked
			builder.AddAttribute(1003, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1004, "onclick", true);

			// The counting sequence values violate all general recommendations.
			// We want the value of the HxInputNumber to be updated (rerendered) when the user input changes, even if FormatValueAsString(Value) hasn't changed.
			// The reason for this is that if a value such as "1.00" is displayed and a user modifies it to "1.0", FormatValueAsString(Value) isn't going to change,
			// the attribute is not rerendered, so the user input stays at "1.0".
			// To solve this issue, we will use the sequence values 1005, 1006, 1007, ... That way, we force Blazor to update the value anyway (because of the sequence change).
			// However, we adjust the sequence only if we want to enforce the rerendering. Otherwise, the component would update all the time.
			// (Originally, we wanted to use the sequences 1000 and 1001, which we would toggle.
			// However, this doesn't work - Blazor probably realizes that it should add the value (sequence 1000) and then remove the value (sequence 1001), resulting in a missing value of the input.)
			checked
			{
				if (forceRenderValue)
				{
					valueSequenceOffset++;
					forceRenderValue = false;
				}
				builder.AddAttribute(1006 + valueSequenceOffset, "value", FormatValueAsString(Value));
			}
			builder.AddElementReferenceCapture(Int32.MaxValue, elementReferece => InputElement = elementReferece);

			builder.CloseElement();
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			CultureInfo culture = CultureInfo.CurrentCulture;

			string workingValue = value;

			// replace . with ,
			if ((culture.NumberFormat.NumberDecimalSeparator == ",") // when decimal separator is ,
				&& (culture.NumberFormat.NumberGroupSeparator != ".")) // and . is NOT used as group separator)
			{
				workingValue = workingValue.Replace(".", ",");
			}

			// omezení počtu desetinných míst
			// pro komplikace s tím, že máme TValue a s ním se dost těžko pracuje se omezíme na řešení a úpravu vstupních dat před konverzí do cílového typu

			if (Decimal.TryParse(value, IsTValueIntegerType ? NumberStyles.Integer : NumberStyles.Float, culture, out decimal parsedValue))
			{
				workingValue = Math.Round(parsedValue, DecimalsEffective, MidpointRounding.AwayFromZero).ToString(culture);
			}

			// konverze do cílového typu

			if (BindConverter.TryConvertTo<TValue>(workingValue, culture, out result))
			{
				// pokud došlo jen ke změně bez změny hodnoty (třeba z 5.50 na 5.5), chceme hodnotu převést na korektní formát (5.5 na 5.50).
				// Nestačí však StateHasChange, viz komentář v BuildRenderInput.
				if (FormatValueAsString(result) != value)
				{
					forceRenderValue = true;
				}

				validationErrorMessage = null;
				return true;
			}
			else
			{
				validationErrorMessage = GetParsingErrorMessage();
				return false;
			}
		}

		/// <summary>
		/// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
		/// </summary>
		/// <param name="value">The value to format.</param>
		/// <returns>A string representation of the value.</returns>
		protected override string FormatValueAsString(TValue value)
		{
			switch (value)
			{
				case null:
					return null;

				case int @int:
					return BindConverter.FormatValue(@int, CultureInfo.CurrentCulture);

				case long @long:
					return BindConverter.FormatValue(@long, CultureInfo.CurrentCulture);
			}

			string format = (DecimalsEffective > 0)
				? "0." + String.Join("", Enumerable.Repeat('0', DecimalsEffective))
				: "0";

			switch (value)
			{
				//case short @short:
				//	return BindConverter.FormatValue(@short, CultureInfo.CurrentCulture);

				case float @float:
					return @float.ToString(format, CultureInfo.CurrentCulture);

				case double @double:
					return @double.ToString(format, CultureInfo.CurrentCulture);

				case decimal @decimal:
					return @decimal.ToString(format, CultureInfo.CurrentCulture);
			}

			throw new InvalidOperationException($"Unsupported type {value.GetType()}.");

		}

		/// <summary>
		/// Returns message for parsing error.
		/// </summary>
		protected virtual string GetParsingErrorMessage()
		{
			var message = this.ParsingErrorMessage;
			if (ParsingErrorMessage is null)
			{
				if (IsTValueIntegerType)
				{
					message = StringLocalizer["ParsingErrorMessage_Integer"];
				}
				else
				{
					message = StringLocalizer["ParsingErrorMessage"];
				}
			}
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}
	}
}
