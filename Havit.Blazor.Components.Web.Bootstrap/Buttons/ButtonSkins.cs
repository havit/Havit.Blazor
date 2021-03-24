using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button skins. Localized (Czech, English).
	/// </summary>
	public class ButtonSkins
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		// DO NOT FORGET TO MAINTAIN RESOURCES!

		public static ButtonSkin Apply { get; } = new ButtonSkin(text: "Apply", resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin Cancel { get; } = new ButtonSkin(text: "Cancel", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Close { get; } = new ButtonSkin(text: "Close", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Delete { get; } = new ButtonSkin(text: "Delete", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Edit { get; } = new ButtonSkin(text: "Edit", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Export { get; } = new ButtonSkin(text: "Export", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Insert { get; } = new ButtonSkin(text: "Insert", resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin New { get; } = new ButtonSkin(text: "New", resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin OK { get; } = new ButtonSkin(text: "OK", resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin Open { get; } = new ButtonSkin(text: "Open", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Remove { get; } = new ButtonSkin(text: "Remove", resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Save { get; } = new ButtonSkin(text: "Save", resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin Up { get; } = new ButtonSkin(resourceType: typeof(ButtonSkins), color: ThemeColor.None, cssClass: "hx-grid-btn", icon: BootstrapIcon.ChevronUp);
		public static ButtonSkin Down { get; } = new ButtonSkin(resourceType: typeof(ButtonSkins), color: ThemeColor.None, cssClass: "hx-grid-btn", icon: BootstrapIcon.ChevronDown);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
