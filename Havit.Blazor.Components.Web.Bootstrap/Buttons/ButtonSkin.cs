using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button skin.
	/// </summary>
	public class ButtonSkin
	{
		/// <summary>
		/// CSS class to be rendered with button.
		/// </summary>
		public string CssClass { get; init; }

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public IconBase Icon { get; init; }

		/// <summary>
		/// Button text.
		/// </summary>
		public string Text { get; init; }

		/// <summary>
		/// Bootstrap button color (style).
		/// </summary>
		public ThemeColor? Color { get; init; }

		/// <summary>
		/// Bootstrap outline button style. See https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons.
		/// </summary>
		public bool? Outlined { get; init; }

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; init; }
	}
}
