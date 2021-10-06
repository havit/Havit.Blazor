using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxButton"/>.
	/// </summary>
	public record ButtonDefaults
	{
		/// <summary>
		/// Bootstrap button size. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#sizes" />
		/// </summary>
		public ButtonSize Size { get; set; } = ButtonSize.Regular;

		/// <summary>
		/// CSS class to be rendered with button.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public IconBase Icon { get; set; }

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
