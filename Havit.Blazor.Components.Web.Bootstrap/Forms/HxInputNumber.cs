using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public class HxInputNumber<TValue> : HxInputBase<TValue>
	{
		// TODO
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "input");
			BuildRenderInput_AddCommonAttributes(builder, "number");

			builder.AddAttribute(1000, "value", FormatValueAsString(Value));
			builder.AddAttribute(1001, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
			builder.CloseElement();
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
			{
				validationErrorMessage = null;
				return true;
			}
			else
			{
				validationErrorMessage = GetParsingErrorMessage(FieldIdentifier.FieldName);
				return false;
			}
		}

		protected string GetParsingErrorMessage(string fieldName)
		{
			if (!String.IsNullOrEmpty(ParsingErrorMessage))
			{
				return String.Format(ParsingErrorMessage, fieldName);
			}

			// TODO: Theme
			throw new InvalidOperationException("TODO");
		}

		/// <summary>
		/// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
		/// </summary>
		/// <param name="value">The value to format.</param>
		/// <returns>A string representation of the value.</returns>
		protected override string FormatValueAsString(TValue value)
		{
			// Avoiding a cast to IFormattable to avoid boxing.
			switch (value)
			{
				case null:
					return null;

				case int @int:
					return BindConverter.FormatValue(@int, CultureInfo.InvariantCulture);

				case long @long:
					return BindConverter.FormatValue(@long, CultureInfo.InvariantCulture);

				//case short @short:
				//	return BindConverter.FormatValue(@short, CultureInfo.CurrentCulture);

				case float @float:
					return BindConverter.FormatValue(@float, CultureInfo.InvariantCulture);

				case double @double:
					return BindConverter.FormatValue(@double, CultureInfo.InvariantCulture);

				case decimal @decimal:
					return BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture);

				default:
					throw new InvalidOperationException($"Unsupported type {value.GetType()}");

			}
		}
	}
}