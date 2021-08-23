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

		public static ButtonSkin Apply { get; set; } = new ButtonSkin() { Text = "Apply", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Cancel { get; set; } = new ButtonSkin() { Text = "Cancel", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Close { get; set; } = new ButtonSkin() { Text = "Close", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Delete { get; set; } = new ButtonSkin() { Text = "Delete", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Edit { get; set; } = new ButtonSkin() { Text = "Edit", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Export { get; set; } = new ButtonSkin() { Text = "Export", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Filter { get; set; } = new ButtonSkin() { Icon = BootstrapIcon.Filter, ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Insert { get; set; } = new ButtonSkin() { Text = "Insert", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin New { get; set; } = new ButtonSkin() { Text = "New", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin OK { get; set; } = new ButtonSkin() { Text = "OK", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Open { get; set; } = new ButtonSkin() { Text = "Open", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Remove { get; set; } = new ButtonSkin() { Text = "Remove", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light };
		public static ButtonSkin Save { get; set; } = new ButtonSkin() { Text = "Save", ResourceType = typeof(ButtonSkins), Color = ThemeColor.Primary };
		public static ButtonSkin Up { get; set; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.None, CssClass = "hx-grid-btn", Icon = BootstrapIcon.ChevronUp };
		public static ButtonSkin Down { get; set; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.None, CssClass = "hx-grid-btn", Icon = BootstrapIcon.ChevronDown };
		public static ButtonSkin CalendarPrevious { get; set; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light, Outlined = true, CssClass="text-dark", Icon = BootstrapIcon.ChevronLeft };
		public static ButtonSkin CalendarNext { get; set; } = new ButtonSkin() { ResourceType = typeof(ButtonSkins), Color = ThemeColor.Light, Outlined = true, CssClass="text-dark", Icon = BootstrapIcon.ChevronRight };

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
