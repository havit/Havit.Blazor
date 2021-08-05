using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Text input (also password, search, etc.)
	/// </summary>
	public class HxInputText : HxInputTextBase
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputText"/>.
		/// </summary>
		public static InputTextDefaults Defaults { get; } = new InputTextDefaults();

		/// <summary>
		/// Input type.
		/// </summary>
		[Parameter] public InputType Type { get; set; } = InputType.Text;

		/// <inheritdoc />
		private protected override string GetElementName() => "input";

		/// <inheritdoc />
		private protected override string GetTypeAttributeValue() => Type.ToString().ToLower();
	}
}
