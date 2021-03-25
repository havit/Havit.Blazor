using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button skins. Localized (Czech, English).
	/// </summary>
	public static class ButtonSkins
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		// DO NOT FORGET TO MAINTAIN RESOURCES!

		public static ButtonSkin Apply { get; } = new ButtonSkin() { Text = "Apply", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Cancel { get; } = new ButtonSkin() { Text = "Cancel", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Close { get; } = new ButtonSkin() { Text = "Close", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Delete { get; } = new ButtonSkin() { Text = "Delete", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Edit { get; } = new ButtonSkin() { Text = "Edit", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Export { get; } = new ButtonSkin() { Text = "Export", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Insert { get; } = new ButtonSkin() { Text = "Insert", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin New { get; } = new ButtonSkin() { Text = "New", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin OK { get; } = new ButtonSkin() { Text = "OK", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Open { get; } = new ButtonSkin() { Text = "Open", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Remove { get; } = new ButtonSkin() { Text = "Remove", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Secondary };
		public static ButtonSkin Save { get; } = new ButtonSkin() { Text = "Save", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Up { get; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.None, CssClass = "hx-grid-btn", Icon = BootstrapIcon.ChevronUp };
		public static ButtonSkin Down { get; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.None, CssClass = "hx-grid-btn", Icon = BootstrapIcon.ChevronDown };

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
