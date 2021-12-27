using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
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
		/// Application-wide defaults for the <see cref="HxInputFile"/> and derived components.
		/// </summary>
		public static InputTextSettings Defaults { get; set; }

		static HxInputText()
		{
			Defaults = new InputTextSettings()
			{
				InputSize = Bootstrap.InputSize.Regular,
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected override InputTextSettings GetDefaults() => Defaults;

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
