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
	/// <summary>
	/// Base class for date inputs.
	/// Uses a <see href="https://github.com/jdtcn/BlazorDateRangePicker">DateRangePicker</see>, follow the Get Started guide!
	/// </summary>
	public abstract class HxInputDateBase<TValue> : HxInputBaseWithInputGroups<TValue>
	{
		/// <summary>
		/// Gets or sets the error message used when displaying an a parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		/// <summary>
		/// The short hint displayed in the input field before the user enters a value.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		private bool forceRenderValue = false;
		private int valueSequenceOffset = 0;

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if ((LabelType == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating) && !String.IsNullOrEmpty(Placeholder))
			{
				throw new InvalidOperationException($"Cannot use {nameof(Placeholder)} with floating labels.");
			}
		}

		/// <inheritdoc />
		protected override sealed void BuildRenderInput(RenderTreeBuilder builder)
		{
			EnsureInputId();

			RenderFragment<BlazorDateRangePicker.DateRangePicker> pickerTemplate = (BlazorDateRangePicker.DateRangePicker dateRangePicker) => (RenderTreeBuilder builder) =>
			{
				// default input in DateRangePicker:
				// <input id="@Id" type="text" @attributes="CombinedAttributes" value="@FormattedRange" @oninput="OnTextInput" @onfocusin="Open" @onfocusout="LostFocus" />

				builder.OpenElement(0, "input");
				BuildRenderInput_AddCommonAttributes(builder, "text"); // id, type, attributes (ale jiné)

				builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

				builder.AddAttribute(1001, "onfocus", "this.select();"); // source: https://stackoverflow.com/questions/4067469/selecting-all-text-in-html-text-input-when-clicked

				builder.AddAttribute(1002, "onfocusin", EventCallback.Factory.Create(this, dateRangePicker.Open));
				builder.AddAttribute(1003, "onfocusout", EventCallback.Factory.Create(this, dateRangePicker.LostFocus));
				builder.AddAttribute(1004, "placeholder", Placeholder);
				builder.AddEventStopPropagationAttribute(1004, "onclick", true);

				// Počítané hodnoty sekvence jsou proti smyslu sekvencí a proti veškerým obecným doporučením.
				// Zde chceme dosáhnout toho, aby při změně uživatelského vstupu, došlo k přerenderování hodnoty, přestože se nezměnila hodnota FormatValueAsString(Value).
				// Důvodem je scénář, kdy se zobrazí hodnota například "1.1.2020", ale uživatel ji změní na "01.1.2020". V takové situaci se nezmění CurrentValueAsString,
				// takže atribut není vyrenderován a zůstává uživatelský vstup, tedy "01.1.2020".
				// Více viz obdobný komentář v HxInputNumber.

				checked
				{
					if (forceRenderValue)
					{
						valueSequenceOffset++;
						forceRenderValue = false;
					}
					builder.AddAttribute(1005 + valueSequenceOffset, "value", CurrentValueAsString);
				}
				builder.AddElementReferenceCapture(Int32.MaxValue, elementReferece => InputElement = elementReferece);

				builder.CloseElement();
			};

			builder.OpenComponent<BlazorDateRangePicker.DateRangePicker>(0);
			builder.AddAttribute(1, nameof(BlazorDateRangePicker.DateRangePicker.Id), InputId);
			builder.AddAttribute(2, nameof(BlazorDateRangePicker.DateRangePicker.PickerTemplate), pickerTemplate);
			builder.AddAttribute(3, nameof(BlazorDateRangePicker.DateRangePicker.MinDate), new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero));
			builder.AddAttribute(4, nameof(BlazorDateRangePicker.DateRangePicker.MaxDate), new DateTimeOffset(2099, 12, 31, 0, 0, 0, TimeSpan.Zero));
			builder.AddAttribute(5, nameof(BlazorDateRangePicker.DateRangePicker.AlwaysShowCalendars), true);
			builder.AddAttribute(6, nameof(BlazorDateRangePicker.DateRangePicker.ShowCustomRangeLabel), false);
			builder.AddAttribute(7, nameof(BlazorDateRangePicker.DateRangePicker.AutoApply), true);

			BuildRenderInput_DateRangePickerAttributes(builder);

			builder.CloseComponent();
		}

		private protected abstract void BuildRenderInput_DateRangePickerAttributes(RenderTreeBuilder builder);

		/// <inheritdoc />
		protected override sealed bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			bool success = TryParseValueFromStringCore(value, out result, out validationErrorMessage);
			if (success && (FormatValueAsString(result) != value))
			{
				forceRenderValue = true;
			}
			return success;
		}

		/// <summary>
		/// Parses a string to create an instance of TValue.
		/// </summary>
		protected abstract bool TryParseValueFromStringCore(string value, out TValue result, out string validationErrorMessage);

		/// <summary>
		/// Returns message for parsing error.
		/// </summary>
		protected virtual string GetParsingErrorMessage(IStringLocalizer stringLocalizer)
		{
			var message = this.ParsingErrorMessage ?? stringLocalizer["ParsingErrorMessage"];
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}
	}
}