using System.Reflection.Emit;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Documentation

	/// <summary>
	/// Checkbox input.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckbox">https://havit.blazor.eu/components/HxCheckbox</see>
	/// </summary>
	public class HxCheckbox : HxInputBase<bool>
	{
		/// <summary>
		/// Text to display next to the checkbox.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Content to display next to the checkbox.
		/// </summary>
		[Parameter] public RenderFragment TextTemplate { get; set; }


		[Inject] protected IStringLocalizer<HxCheckbox> Localizer { get; set; }

		/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
		private protected override string CoreInputCssClass => "form-check-input";

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(-2, "div");
			builder.AddAttribute(-1, "class", "form-check");

			builder.OpenElement(0, "input");
			BuildRenderInput_AddCommonAttributes(builder, "checkbox");
			builder.AddAttribute(1000, "checked", BindConverter.FormatValue(CurrentValue));
			builder.AddAttribute(1001, "onchange", value: EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue));
			builder.AddEventStopPropagationAttribute(1002, "onclick", true);
			builder.AddElementReferenceCapture(1003, elementReferece => InputElement = elementReferece);
			builder.CloseElement(); // input

			builder.OpenElement(2000, "label");
			builder.AddAttribute(2001, "class", "form-check-label");
			builder.AddAttribute(2002, "for", InputId);
			if (TextTemplate == null)
			{
				builder.AddContent(2003, Text);
			}
			builder.AddContent(2004, TextTemplate);
			builder.CloseElement(); // label

			builder.CloseElement(); // div
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out bool result, out string validationErrorMessage)
		{
			throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
		}

		protected override void RenderChipValue(RenderTreeBuilder builder)
		{
			builder.AddContent(0, CurrentValue ? Localizer["ChipValueTrue"] : Localizer["ChipValueFalse"]);
		}
	}
}
