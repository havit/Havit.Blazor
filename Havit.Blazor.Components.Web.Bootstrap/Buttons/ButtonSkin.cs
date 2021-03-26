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
	public record ButtonSkin
	{
		/// <summary>
		/// CSS class to be rendered with button.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public IconBase Icon { get; set; }

		/// <summary>
		/// Button text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Bootstrap button color (style).
		/// </summary>
		public ThemeColor? Color { get; set; }

		/// <summary>
		/// Bootstrap outline button style. See https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons.
		/// </summary>
		public bool? Outlined { get; set; }

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; set; }
	}
}
