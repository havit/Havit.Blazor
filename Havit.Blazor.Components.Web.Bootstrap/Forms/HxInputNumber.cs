using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
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
	/// Supported values: int (Int32), long (Int64), float (Single), double, decimal.
	/// </typeparam>
	public class HxInputNumber<TValue> : HxInputBaseWithInputGroups<TValue>
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
		/// Returns true for integer types (false for floating point types).
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

		private bool forceRenderValue = false;
		private int valueSequenceOffset = 0;

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (FloatingLabelEffective && !String.IsNullOrEmpty(Placeholder))
			{
				throw new InvalidOperationException($"Cannot use {nameof(Placeholder)} with floating labels.");
			}
		}

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "input");
			BuildRenderInput_AddCommonAttributes(builder, "text");

			if (!String.IsNullOrEmpty(Placeholder))
			{
				builder.AddAttribute(1000, "placeholder", Placeholder);
			}

			if (DecimalsEffective <= 0)
			{
				// We can use inputmode numeric when we do not want to enter decimals. It displays a keyboard without a key for a decimal point.
				// We do not use inputmode decimal because browser (ie. Safari on iPhone) displays a keyboard with a decimal point key depending on device settings (language & keyboard).
				builder.AddAttribute(1001, "inputmode", "numeric");
			}

			builder.AddAttribute(1002, "onfocus", "this.select();"); // source: https://stackoverflow.com/questions/4067469/selecting-all-text-in-html-text-input-when-clicked
			builder.AddAttribute(1003, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1004, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxInputNumber nastavitelné?

			// Počítané hodnoty sekvence jsou proti smyslu sekvencí a proti veškerým obecným doporučením.
			// Zde chceme dosáhnout toho, aby při změně uživatelského vstupu, došlo k přerenderování hodnoty, přestože se nezměnila hodnota FormatValueAsString(Value).
			// Důvodem je scénář, kdy se zobrazí hodnota například "1.00", ale uživatel ji změní na "1.0". V takové situaci se nezmění FormatValueAsString(Value),
			// takže atribut není vyrenderován a zůstává uživatelský vstup, tedy "1.0".
			// Jako řešení tedy použijeme hodnotu sequence 1005, 1006, 1007, čímž přimějeme Blazor, aby hodnotu přeci jen vyrenderovat (nezmění se hodnota, ale sequence).
			// Zároveň ale nechceme, aby každý input se pořád znovu a znovu renderoval, takže sequence změníme jen když chceme vynutit vyrenderování hodnoty.
			// (Původně jsem chtěl, aby se použili sequence 1000 a 1001, které se budou přepínat, avšak toto nefunguje - při přechodu z 1001 na 1000 nejspíš Blazor 
			// nejprve (sequence 1000) přijde na to, že má value přidat a poté (sequence 1001), že má value odebrat a tak výsledkem je mizející hodnota z inputu).
			checked
			{
				if (forceRenderValue)
				{
					valueSequenceOffset++;
					forceRenderValue = false;
				}
				builder.AddAttribute(1006 + valueSequenceOffset, "value", FormatValueAsString(Value));
			}
			builder.AddElementReferenceCapture(1007 + valueSequenceOffset, elementReferece => InputElement = elementReferece);

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