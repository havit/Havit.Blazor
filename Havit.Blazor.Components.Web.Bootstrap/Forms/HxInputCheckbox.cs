using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Checkbox input.
	/// </summary>
	public class HxInputCheckbox : HxInputBase<bool>
	{
		/// <summary>
		/// Allows grouping checkboxes on the same horizontal row by rendering them inline. Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool Inline { get; set; }

		/// <summary>
		/// Color of the checkbox when used within a <see cref="HxButtonGroup"/>.
		/// </summary>
		[Parameter] public ThemeColor? ButtonColor { get; set; }

		[Inject] protected IStringLocalizer<HxInputCheckbox> Localizer { get; set; }

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
		private protected override string CoreInputCssClass => ButtonStyle ? "btn-check" : "form-check-input";

		/// <inheritdoc cref="HxInputBase{TValue}.CoreLabelCssClass" />
		private protected override string CoreLabelCssClass => ButtonStyle ? $"btn {ButtonColor.Value.ToOutlineButtonColorCss()}" : "form-check-label";

		private bool ButtonStyle => ButtonColor is not null;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (ButtonStyle)
			{
				RenderCheckbox(builder);
			}
			else
			{
				base.BuildRenderTree(builder);
			}
		}

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			if (!ButtonStyle)
			{
				RenderCheckbox(builder);
			}
		}

		private void RenderCheckbox(RenderTreeBuilder builder)
		{
			builder.OpenElement(1, "input");
			BuildRenderInput_AddCommonAttributes(builder, "checkbox");

			builder.AddAttribute(1000, "checked", BindConverter.FormatValue(CurrentValue));
			builder.AddAttribute(1001, "onchange", value: EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue));
			builder.AddEventStopPropagationAttribute(1002, "onclick", true);
			builder.AddElementReferenceCapture(1003, elementReferece => InputElement = elementReferece);

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
}
