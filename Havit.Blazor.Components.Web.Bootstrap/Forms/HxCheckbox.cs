﻿using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

// TODO: Labels for ToggleButtons are not positioned well.
// TODO: Not rendered correctly for button group support.

/// <summary>
/// Checkbox input.<br />
/// (Replaces the former <c>HxInputCheckbox</c> component which was dropped in v 4.0.0.)
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckbox">https://havit.blazor.eu/components/HxCheckbox</see>
/// </summary>
public class HxCheckbox : HxInputBase<bool>
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxCheckbox"/> and derived components.
	/// </summary>
	public static CheckboxSettings Defaults { get; set; }

	static HxCheckbox()
	{
		Defaults = new CheckboxSettings()
		{
			ValidationMessageMode = Havit.Blazor.Components.Web.Bootstrap.ValidationMessageMode.Floating,
			Color = ThemeColor.None,
			Outline = false
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override CheckboxSettings GetDefaults() => Defaults;
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

	/// <summary>
	/// Checkbox render mode.
	/// </summary>
	// TODO: No virtual, refactor!
	[Parameter] public virtual CheckboxRenderMode RenderMode { get; set; }

	/// <summary>
	/// Bootstrap button style - theme color.<br />
	/// The default is taken from <see cref="HxButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxButton) + " has to be set.");

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	[Parameter] public bool? Outline { get; set; }
	protected bool OutlineEffective => Outline ?? GetSettings()?.Outline ?? GetDefaults().Outline ?? throw new InvalidOperationException(nameof(Outline) + " default for " + nameof(HxCheckbox) + " has to be set.");

	/// <summary>
	/// Indicates HxCheckbox is used as a part of a button group. Value provided as a cascading value from <see cref="HxButtonGroup" />.
	/// </summary>
	/// <remarks>
	/// In a button group the html structure must be as simple as documented in .
	/// Thererefore we do not render neither the outer div(s) nor the validation message.
	/// </remarks>
	[CascadingParameter(Name = HxButtonGroup.InButtonGroupCascadingValueName)] public bool InButtonGroup { get; set; } = false;

	[Inject] protected IStringLocalizer<HxCheckbox> Localizer { get; set; }

	/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
	private protected override string CoreInputCssClass => RenderMode switch
	{
		CheckboxRenderMode.Checkbox => "form-check-input",
		CheckboxRenderMode.Switch => "form-check-input",
		CheckboxRenderMode.NativeSwitch => "form-check-input",
		CheckboxRenderMode.ToggleButton => "btn-check",
		_ => throw new InvalidOperationException($"Unknown {nameof(CheckboxRenderMode)}: {RenderMode}.")
	};

	/// <inheritdoc cref="HxInputBase{TValue}.CoreCssClass" />
	private protected override string CoreCssClass
	{
		get
		{
			if (InButtonGroup && (RenderMode == CheckboxRenderMode.ToggleButton))
			{
				// We need to disable rendering the outer div (and we expect Label and LabelTemplate properties are not set).
				return null;
			}

			return CssClassHelper.Combine(
				base.CoreCssClass,
				NeedsInnerDiv ? null : AdditionalFormElementCssClass,
				NeedsFormCheckOuter ? CssClassHelper.Combine("form-check", Inline ? "form-check-inline" : null) : null,
				Reverse ? "form-check-reverse" : null);
		}
	}

	// TODO: Refactor (remove?)
	private string AdditionalFormElementCssClass => RenderMode switch
	{
		CheckboxRenderMode.Checkbox => null,
		CheckboxRenderMode.Switch => "form-switch",
		CheckboxRenderMode.NativeSwitch => "form-switch",
		CheckboxRenderMode.ToggleButton => null,
		_ => throw new InvalidOperationException($"Unknown {nameof(CheckboxRenderMode)}: {RenderMode}.")
	};

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
	private protected bool NeedsFormCheck => (!string.IsNullOrWhiteSpace(Text) || (TextTemplate is not null)) && (RenderMode != CheckboxRenderMode.ToggleButton);

	/// <summary>
	/// For checkbox without any Label/LabelTemplate and without Text/TextTemplate we do not add form-check to the parent div.
	/// </summary>
	private protected bool NeedsFormCheckOuter => !NeedsInnerDiv && NeedsFormCheck;

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

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
		if ((RenderMode == CheckboxRenderMode.Switch) || (RenderMode == CheckboxRenderMode.NativeSwitch))
		{
			builder.AddAttribute(1003, "role", "switch");
		}
		if (RenderMode == CheckboxRenderMode.NativeSwitch)
		{
			builder.AddAttribute(1004, "switch", true); // does not render ="true", we need only "switch" attribute to be present
		}
		if (RenderMode == CheckboxRenderMode.ToggleButton)
		{
			builder.AddAttribute(10045, "autocomplete", "off");
		}

		builder.SetUpdatesAttributeName("checked");
		builder.AddEventStopPropagationAttribute(1006, "onclick", true);
		builder.AddElementReferenceCapture(1007, elementReference => InputElement = elementReference);
		builder.CloseElement(); // input

		builder.OpenElement(2000, "label");
		builder.AddAttribute(2001, "class", CssClassHelper.Combine(
			(RenderMode == CheckboxRenderMode.ToggleButton) ? "btn" : "form-check-label",
			(RenderMode == CheckboxRenderMode.ToggleButton) ? ColorEffective.ToButtonColorCss(OutlineEffective) : null,
			TextCssClass));
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

	protected override void BuildRenderValidationMessage(RenderTreeBuilder builder)
	{
		if (InButtonGroup && (RenderMode == CheckboxRenderMode.ToggleButton))
		{
			return;
		}

		base.BuildRenderValidationMessage(builder);
	}

	/// <summary>
	/// Focuses the checkbox.
	/// </summary>
	public async ValueTask FocusAsync() => await InputElement.FocusOrThrowAsync(this);
}
