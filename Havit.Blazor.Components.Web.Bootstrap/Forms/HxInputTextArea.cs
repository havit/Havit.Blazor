using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	/// <summary>
	/// Textarea.
	/// </summary>
	public class HxInputTextArea : HxInputText
	{
		/// <inheritdoc />
		private protected override string GetElementName() => "textarea";
		
		/// <inheritdoc />
		private protected override string GetTypeAttributeValue() => null;
	}
}
