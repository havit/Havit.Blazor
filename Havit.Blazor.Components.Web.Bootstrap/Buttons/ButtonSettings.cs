using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxButton"/> and derived components.
	/// </summary>
	public record ButtonSettings
	{
		/// <summary>
		/// Bootstrap button size. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#sizes" />.
		/// </summary>
		public ButtonSize? Size { get; set; }

		/// <summary>
		/// CSS class to be rendered with the button.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Icon to be rendered with the button.
		/// </summary>
		public IconBase Icon { get; set; }

		/// <summary>
		/// Position of the icon within the button.
		/// </summary>
		public ButtonIconPlacement? IconPlacement { get; set; }

		/// <summary>
		/// Bootstrap button color (style).
		/// </summary>
		public ThemeColor? Color { get; set; }

		/// <summary>
		/// Bootstrap outline button style. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons" />.
		/// </summary>
		public bool? Outline { get; set; } = false;
	}
}
