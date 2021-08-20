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
	/// Numeric input in percentages
	/// </summary>
	public class HxInputPercent : HxInputNumber<decimal>
	{
		public HxInputPercent()
		{
			InputGroupEnd = "%";
		}

		/// <summary>
		/// Converts value into percentages and formats it as a string.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected override string FormatValueAsString(decimal value)
		{
			value *= 100; // convert value to percentages

			string format = (DecimalsEffective > 0)
				? "0." + String.Join("", Enumerable.Repeat('0', DecimalsEffective))
				: "0";

			return value.ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <param name="validationErrorMessage"></param>
		/// <returns></returns>
		protected override bool TryParseValueFromString(string value, out decimal result, out string validationErrorMessage)
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

			if (Decimal.TryParse(value, NumberStyles.Float, culture, out decimal parsedValue))
			{
				decimal number = Math.Round(parsedValue, DecimalsEffective, MidpointRounding.AwayFromZero);
				number /= 100; // divide the number by 100

				workingValue = number.ToString(culture);
			}

			// konverze do cílového typu

			if (BindConverter.TryConvertTo<decimal>(workingValue, culture, out result))
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
