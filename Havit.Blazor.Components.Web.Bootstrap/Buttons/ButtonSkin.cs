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
		public string CssClass { get; }

		/// <summary>
		/// Icon to be rendered with button.
		/// </summary>
		public BootstrapIcon? Icon { get; }

		/// <summary>
		/// Button text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; }

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
