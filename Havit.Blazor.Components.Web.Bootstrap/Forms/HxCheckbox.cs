using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Checkbox input.<br />
/// (Replaces the former <c>HxInputCheckbox</c> component which was dropped in v 4.0.0.)
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckbox">https://havit.blazor.eu/components/HxCheckbox</see>
/// </summary>
public class HxCheckbox : HxInputBase<bool>
{
	/// <summary>
	/// Set of settings to be applied to the component instance.
	/// </summary>
	[Parameter] public CheckboxSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	protected override CheckboxSettings GetSettings() => Settings;

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
	/// Allows grouping checkboxes on the same horizontal row by rendering them inline. The default value is <c>false</c>.
	/// This only works when there is no label, no hint, and no validation message.
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
		NeedsInnerDiv ? null : AdditionalFormElementCssClass,
		NeedsFormCheckOuter ? CssClassHelper.Combine("form-check", Inline ? "form-check-inline" : null) : null,
		Reverse ? "form-check-reverse" : null);

	/// <summary>
	/// CSS class that allows adding <c>form-switch</c> in derived <see cref="HxSwitch"/>.
	/// </summary>
	private protected virtual string AdditionalFormElementCssClass => null;

	/// <summary>
	/// For a naked checkbox without Label/LabelTemplate, we add form-check, form-check-inline to the parent div (allows combining with CssClass etc.).
	/// It is expected that there is just the parent div, input, and label for Text/TextTemplate.
	/// No label for Label/LabelTemplate, no HintTemplate, no validation message is expected.
	/// For a checkbox with Label, there is a parent div followed by a label for Label/LabelTemplate.
	/// Siblings label, input, label do not work well and there is nowhere to add form-check.
	/// Therefore, we wrap the input and label in the additional div.
	/// </summary>
	private protected bool NeedsInnerDiv => !string.IsNullOrWhiteSpace(Label) || (LabelTemplate is not null);

	/// <summary>
	/// For checkbox without any Text/TextTemplate we do not add form-check class.
	/// </summary>
	private protected bool NeedsFormCheck => !string.IsNullOrWhiteSpace(Text) || (TextTemplate is not null);

	/// <summary>
	/// For checkbox without any Label/LabelTemplate and without Text/TextTemplate we do not add form-check to the parent div.
	/// </summary>
	private protected bool NeedsFormCheckOuter => !NeedsInnerDiv && NeedsFormCheck;

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		if (NeedsInnerDiv)
		{
			builder.OpenElement(-2, "div");
			builder.AddAttribute(-1, "class", CssClassHelper.Combine(NeedsFormCheck ? "form-check" : null, AdditionalFormElementCssClass));
		}

		EnsureInputId(); // must be called before the input is rendered

		builder.OpenElement(0, "input");
		BuildRenderInput_AddCommonAttributes(builder, "checkbox");
		builder.AddAttribute(1000, "checked", BindConverter.FormatValue(CurrentValue));

		// HTML input sends the "on" value when the checkbox is checked, and nothing otherwise.
		// We include the "value" attribute so that when this is posted by a form, "true"
		// is included in the form fields.
		builder.AddAttribute(1001, "value", bool.TrueString);

		builder.AddAttribute(1002, "onchange", value: EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue));
		builder.SetUpdatesAttributeName("checked");
		builder.AddEventStopPropagationAttribute(1003, "onclick", true);
		builder.AddElementReferenceCapture(1004, elementReference => InputElement = elementReference);
		builder.CloseElement(); // input

		builder.OpenElement(2000, "label");
		builder.AddAttribute(2001, "class", CssClassHelper.Combine("form-check-label", TextCssClass));
		builder.AddAttribute(2002, "for", InputId);
		if (TextTemplate == null)
		{
			builder.AddContent(2003, Text);
		}
		builder.AddContent(2004, TextTemplate);
		builder.CloseElement(); // label

		if (NeedsInnerDiv)
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
		builder.AddContent(0, String.IsNullOrWhiteSpace(Label) ? Text : Label);
	}

	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		// #886 [HxCheckbox] [HxSwitch] Use Text instead of Yes/No for chips
		// If both Text and Label are set, use Text for the positive chip value.
		// BTW: Negative value is currently never used as chip is rendered only if the value is not equal to default(TValue).
		// This might need additional attention if we implement support for three-state checkboxes
		// or allow setting neutral value other than default(TValue).
		string positiveValue;
		if (!String.IsNullOrWhiteSpace(Text) && !String.IsNullOrWhiteSpace(Label))
		{
			positiveValue = Text;
		}
		else
		{
			positiveValue = Localizer["ChipValueTrue"];
		}
		builder.AddContent(0, CurrentValue ? positiveValue : Localizer["ChipValueFalse"]);
	}
}
