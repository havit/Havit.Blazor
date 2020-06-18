using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
			builder.OpenElement(0, GetElementName());
			BuildRenderInput_AddCommonAttributes(builder, GetTypeAttributeValue());

			var maxLengthAttribute = FieldIdentifier.Model.GetType().GetMember(FieldIdentifier.FieldName).Single().GetCustomAttribute<MaxLengthAttribute>();
			if ((maxLengthAttribute != null) && (maxLengthAttribute.Length > 0))
			{
				builder.AddAttribute(1000, "maxlength", maxLengthAttribute.Length);
			}

			builder.AddAttribute(1001, "value", FormatValueAsString(Value));
			builder.AddAttribute(1002, ModelUpdateMode.ToEventName(), EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));

			builder.CloseElement();
		}

		private protected virtual string GetElementName() => "input";

		private protected virtual string GetTypeAttributeValue() => "text";

		protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
		{
			result = value;
			validationErrorMessage = null;
			return true;
		}
	}
}
