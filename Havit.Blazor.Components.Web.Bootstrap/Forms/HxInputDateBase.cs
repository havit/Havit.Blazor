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

		private protected abstract bool UseSingleDatePicker { get; }

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

				builder.AddAttribute(1000, "value", CurrentValueAsString);
				builder.AddAttribute(1001, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

				builder.AddAttribute(1002, "onfocusin", EventCallback.Factory.Create(this, dateRangePicker.Open));
				builder.AddAttribute(1003, "onfocusout", EventCallback.Factory.Create(this, dateRangePicker.LostFocus));

				builder.AddEventStopPropagationAttribute(1004, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxInputDate nastavitelné?
				
				builder.CloseElement();
			};

			builder.OpenComponent<BlazorDateRangePicker.DateRangePicker>(0);
			builder.AddAttribute(1, nameof(BlazorDateRangePicker.DateRangePicker.Id), InputId);
			builder.AddAttribute(2, nameof(BlazorDateRangePicker.DateRangePicker.PickerTemplate), pickerTemplate);
			builder.AddAttribute(3, nameof(BlazorDateRangePicker.DateRangePicker.SingleDatePicker), UseSingleDatePicker);
			builder.AddAttribute(4, nameof(BlazorDateRangePicker.DateRangePicker.StartDateChanged), EventCallback.Factory.Create<DateTimeOffset?>(this, HandleStartDateChanged));
			if (!UseSingleDatePicker)
			{
				builder.AddAttribute(5, nameof(BlazorDateRangePicker.DateRangePicker.EndDateChanged), EventCallback.Factory.Create<DateTimeOffset?>(this, HandleEndDateChanged));
			}
			builder.CloseComponent();
		}

		private protected abstract Task HandleStartDateChanged(DateTimeOffset? startDate);

		private protected abstract Task HandleEndDateChanged(DateTimeOffset? startDate);

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