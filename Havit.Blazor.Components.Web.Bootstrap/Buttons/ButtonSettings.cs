using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for <see cref="HxButton"/> and derived components.
	/// </summary>
	public record ButtonSettings
	{
		/// <summary>
		/// Bootstrap button size. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#sizes" />
		/// </summary>
		public ButtonSize Size { get; set; } = ButtonSize.Regular;

		/// <summary>
		/// CSS class to be rendered with the button.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Icon to be rendered with the button.
		/// </summary>
		public IconBase Icon { get; set; }

		/// <summary>
		/// Position of the icon within the button. Default is <see cref="ButtonIconPlacement.Start" />
		/// </summary>
		public ButtonIconPlacement IconPlacement { get; set; } = ButtonIconPlacement.Start;


		/// <summary>
		/// Bootstrap button color (style). Default is <see cref="ThemeColor.None"/>.
		/// </summary>
		public ThemeColor? Color { get; set; } = ThemeColor.None;

		/// <summary>
		/// Bootstrap outline button style. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons" />.
		/// </summary>
		public bool Outline { get; set; } = false;
	}
}
