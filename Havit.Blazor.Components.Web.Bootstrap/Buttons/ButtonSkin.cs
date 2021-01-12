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
		/// Css class to be rendered with button.
		/// </summary>
		public string CssClass { get; }

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public IconBase Icon { get; }

		/// <summary>
		/// Button text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Bootstrap button color (style).
		/// </summary>
		public ThemeColor? Color { get; set; }

		/// <summary>
		/// Bootstrap outline button style. See https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons.
		/// </summary>
		public bool? Outline { get; set; }

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public ButtonSkin(string text = null, string cssClass = null, IconBase icon = null, Type resourceType = null, ThemeColor? color = null, bool? outline = false)
		{
			this.CssClass = cssClass;
			this.Icon = icon;
			this.Text = text;
			this.ResourceType = resourceType;
			this.Color = color;
			this.Outline = outline;
		}
	}
}
