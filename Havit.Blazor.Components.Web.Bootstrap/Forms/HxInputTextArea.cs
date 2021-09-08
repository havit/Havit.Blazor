using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.0/forms/floating-labels/#textareas" target="_blank">Textarea</a>.
	/// To set a custom height, do not use the rows attribute. Instead, set an explicit height (either inline or via custom CSS).
	/// </summary>
	public class HxInputTextArea : HxInputText
	{
		/// <inheritdoc />
		private protected override string GetElementName() => "textarea";

		/// <inheritdoc />
		private protected override string GetTypeAttributeValue() => null;
	}
}
