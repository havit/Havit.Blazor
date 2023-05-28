using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Checkbox input.<br />
/// (Replaces the former <see cref="HxInputCheckbox"/> component which is now obsolete.)
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckbox">https://havit.blazor.eu/components/HxCheckbox</see>
/// </summary>
public class HxCheckbox : HxInputBase<bool>
{
	/// <summary>
	/// Set of settings to be applied to the component instance.
	/// </summary>
	[Parameter] public CheckboxSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	protected override CheckboxSettings GetSettings() => this.Settings;

	/// <summary>
	/// Text to display next to the checkbox.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Content to display next to the checkbox.
	/// </summary>
	[Parameter] public RenderFragment TextTemplate { get; set; }

	/// <summary>
	/// CSS class to apply to the text.
	/// </summary>
	[Parameter] public string TextCssClass { get; set; }

	/// <summary>
	/// Allows grouping checkboxes on the same horizontal row by rendering them inline. Default is <c>false</c>.		
	/// Works only when there is no label, no hint and no validation message.
	/// </summary>
	[Parameter] public bool Inline { get; set; }

	/// <summary>
	/// Put the checkbox on the opposite side - first text, then checkbox.
	/// </summary>
	[Parameter] public bool Reverse { get; set; }

	[Inject] protected IStringLocalizer<HxCheckbox> Localizer { get; set; }

	/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
	private protected override string CoreInputCssClass => "form-check-input";

	/// <inheritdoc cref="HxInputBase{TValue}.CoreCssClass" />
	private protected override string CoreCssClass => CssClassHelper.Combine(
		base.CoreCssClass,
		!NeedsFormCheckInnerDiv ? CssClassHelper.Combine(this.CoreFormElementCssClass, Inline ? "form-check-inline" : null) : null,
		Reverse ? "form-check-reverse" : null);

	/// <summary>
	/// CSS class for <c>form-check</c> element (e.g. allows adding <c>form-switch</c> in derived <see cref="HxSwitch"/>).
	/// </summary>
	private protected virtual string CoreFormElementCssClass => "form-check";

	/// <summary>
	/// For naked checkbox without Label/LabelTemplate, we add form-check, form-check-inline to the parent div (allows combining with CssClass etc.).
	/// It is expected there is just the parent div, input and label for Text/TextTemplate.
	/// No label for Label/LabelTemplate, no HintTemplate, no validation message is expected.
	/// For checkbox with Label, there is a parent div followed by label for Label/LabelTemplate.
	/// Siblings label, input, label does not work well and there is no where to add form-check.
	/// There for we wrap the input and label to the additional div.
	/// </summary>
	private protected bool NeedsFormCheckInnerDiv => !String.IsNullOrWhiteSpace(this.Label) || (this.LabelTemplate is not null);

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		if (NeedsFormCheckInnerDiv)
		{
			builder.OpenElement(-2, "div");
			builder.AddAttribute(-1, "class", this.CoreFormElementCssClass);
		}

		EnsureInputId(); // must be called before the input is rendered

		builder.OpenElement(0, "input");
		BuildRenderInput_AddCommonAttributes(builder, "checkbox");
		builder.AddAttribute(1000, "checked", BindConverter.FormatValue(CurrentValue));
		builder.AddAttribute(1001, "onchange", value: EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue));
		builder.AddEventStopPropagationAttribute(1002, "onclick", true);
		builder.AddElementReferenceCapture(1003, elementReference => InputElement = elementReference);
		builder.CloseElement(); // input

		builder.OpenElement(2000, "label");
		builder.AddAttribute(2001, "class", CssClassHelper.Combine("form-check-label", this.TextCssClass));
		builder.AddAttribute(2002, "for", InputId);
		if (TextTemplate == null)
		{
			builder.AddContent(2003, Text);
		}
		builder.AddContent(2004, TextTemplate);
		builder.CloseElement(); // label

		if (NeedsFormCheckInnerDiv)
		{
			builder.CloseElement(); // div
		}
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, out bool result, out string validationErrorMessage)
	{
		throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
	}

	protected override void RenderChipLabel(RenderTreeBuilder builder)
	{
		builder.AddContent(0, String.IsNullOrWhiteSpace(this.Label) ? this.Text : this.Label);
	}

	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, CurrentValue ? Localizer["ChipValueTrue"] : Localizer["ChipValueFalse"]);
	}
}
