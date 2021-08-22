using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Numeric input in percentages with value normalization (0% = 0, 100% = 1.0).
	/// </summary>
	public class HxInputPercent<TValue> : HxInputNumber<TValue>
	{
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) };

		public HxInputPercent()
		{
			Type undelyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
			if (!supportedTypes.Contains(undelyingType))
			{
				throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
			}

			InputGroupEnd = "%";
		}

		/// <summary>
		/// Converts value into percentages and formats it as a string.
		/// </summary>
		protected override string FormatValueAsString(TValue value)
		{
			string format = (DecimalsEffective > 0)
				? "0." + String.Join("", Enumerable.Repeat('0', DecimalsEffective))
				: "0";

			switch (value)
			{
				case null:
					return null;
				case float @float:
					@float *= 100;
					return @float.ToString(format, CultureInfo.CurrentCulture);
				case double @double:
					@double *= 100;
					return @double.ToString(format, CultureInfo.CurrentCulture);
				case decimal @decimal:
					@decimal *= 100;
					return @decimal.ToString(format, CultureInfo.CurrentCulture);
				default:
					throw new InvalidOperationException($"Unsupported type {value.GetType()}.");
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
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
			if (decimal.TryParse(value, NumberStyles.Float, culture, out decimal parsedValue))
			{
				decimal number = Math.Round(parsedValue, DecimalsEffective, MidpointRounding.AwayFromZero);
				number /= 100; // divide the number by 100

				workingValue = number.ToString(culture);
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
	}
}
