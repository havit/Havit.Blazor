using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap.Buttons
{
	/// <summary>
	/// Button skin.
	/// </summary>
	public class ButtonSkin
	{
		/// <summary>
		/// Css class to be rendered with button.
		/// </summary>
		public string CssClass { get; set; } // TODO C# 9.0: set -> init

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public BootstrapIcon? Icon { get; set; } // TODO C# 9.0: set -> init

		/// <summary>
		/// Button text.
		/// </summary>
		public string Text { get; set; } // TODO C# 9.0: set -> init

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; set; } // TODO C# 9.0: set -> init

		/// <summary>
		/// Constructor.
		/// </summary>
		public ButtonSkin()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ButtonSkin(string text = null, string cssClass = null, BootstrapIcon? icon = null, Type resourceType = null)
		{
			CssClass = cssClass;
			Icon = icon;
			Text = text;
			ResourceType = resourceType;
		}
	}
}
