using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public class HxInputText : HxInputBase<string>
	{
		[Parameter]
		public ModelUpdateMode ModelUpdateMode { get; set; } = ModelUpdateMode.OnChange;

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "input");
			BuildRenderInput_AddCommonAttributes(builder, GetTypeAttributeValue());

			builder.AddAttribute(1000, "value", FormatValueAsString(Value));
			builder.AddAttribute(1001, ModelUpdateMode.ToEventName(), EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));

			builder.CloseElement();
		}

		protected virtual string GetTypeAttributeValue()
		{
			return "text";
		}

		protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
		{
			result = value;
			validationErrorMessage = null;
			return true;
		}
	}
}
