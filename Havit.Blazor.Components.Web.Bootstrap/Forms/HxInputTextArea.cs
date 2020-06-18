using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public class HxInputTextArea : HxInputText
	{
		private protected override string GetElementName() => "textarea";
		private protected override string GetTypeAttributeValue() => null;
	}
}
