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
	public class HxInputText : HxInputTextBase
	{
		[Parameter]
		public InputType Type { get; set; } = InputType.Text;

		private protected override string GetElementName() => "input";

		private protected override string GetTypeAttributeValue() => Type.ToString().ToLower();

	}
}
