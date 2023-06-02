using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Checkbox input.<br />
/// Obsolete, use <see cref="HxCheckbox"/> instead.
/// </summary>
[Obsolete("Use HxCheckbox instead. The former Label parameter is now Text.")]
public class HxInputCheckbox : HxInputBase<bool>
{
	/// <summary>
	/// Allows grouping checkboxes on the same horizontal row by rendering them inline. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool Inline { get; set; }

	/// <summary>
	/// Set of settings to be applied to the component instance.
	/// </summary>
	[Parameter] public InputCheckboxSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	protected override InputCheckboxSettings GetSettings() => this.Settings;

	[Inject] protected IStringLocalizer<HxCheckbox> Localizer { get; set; }

	/// <inheritdoc cref="LabelValueRenderOrder" />
	protected override LabelValueRenderOrder RenderOrder => LabelValueRenderOrder.ValueLabel;

	/// <inheritdoc cref="HxInputBase{TValue}.CoreCssClass" />
	private protected override string CoreCssClass
	{
		get
		{
			if (String.IsNullOrWhiteSpace(this.Label))
			{
				return "position-relative";
			}
			return CssClassHelper.Combine("form-check", this.Inline ? "form-check-inline" : null, "position-relative");
		}
	}

	/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
	private protected override string CoreInputCssClass => "form-check-input";

	/// <inheritdoc cref="HxInputBase{TValue}.CoreLabelCssClass" />
	private protected override string CoreLabelCssClass => "form-check-label";

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "input");
		BuildRenderInput_AddCommonAttributes(builder, "checkbox");

		builder.AddAttribute(1000, "checked", BindConverter.FormatValue(CurrentValue));
		builder.AddAttribute(1001, "onchange", value: EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue));
		builder.AddEventStopPropagationAttribute(1002, "onclick", true);
		builder.AddElementReferenceCapture(1003, elementReference => InputElement = elementReference);

		builder.CloseElement();
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
