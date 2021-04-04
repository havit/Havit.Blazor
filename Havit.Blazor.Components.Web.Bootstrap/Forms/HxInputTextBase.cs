using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Text-based (string) input base class.
	/// </summary>
	public abstract class HxInputTextBase : HxInputBaseWithInputGroups<string>
	{
		/// <summary>
		/// Gets or sets the behavior when the model is updated from then input.
		/// </summary>
		[Parameter] public BindEvent BindEvent { get; set; } = BindEvent.OnChange;

		/// <summary>
		/// Placeholder for the input.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (FloatingLabelEffective && !String.IsNullOrEmpty(Placeholder))
			{
				throw new InvalidOperationException($"Cannot use {nameof(Placeholder)} with floating labels.");
			}
		}

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, GetElementName());
			BuildRenderInput_AddCommonAttributes(builder, GetTypeAttributeValue());

			MaxLengthAttribute maxLengthAttribute = GetValueAttribute<MaxLengthAttribute>();
			if ((maxLengthAttribute != null) && (maxLengthAttribute.Length > 0))
			{
				builder.AddAttribute(1000, "maxlength", maxLengthAttribute.Length);
			}

			if (!String.IsNullOrEmpty(Placeholder))
			{
				builder.AddAttribute(1001, "placeholder", Placeholder);
			}

			builder.AddAttribute(1002, "value", FormatValueAsString(Value));
			builder.AddAttribute(1003, BindEvent.ToEventName(), EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1004, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxInputTextBase nastavitelné?
			builder.AddElementReferenceCapture(1005, elementReferece => InputElement = elementReferece);

			builder.CloseElement();
		}

		/// <summary>
		/// Returns element name to render.
		/// </summary>
		private protected abstract string GetElementName();

		/// <summary>
		/// Returns type attribute value.
		/// </summary>
		private protected abstract string GetTypeAttributeValue();

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
		{
			result = value;
			validationErrorMessage = null;
			return true;
		}
	}
}
