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

		public static ButtonSkin Apply { get; } = new ButtonSkin(text: "Apply", icon: BootstrapIcon.Check2, resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin Cancel { get; } = new ButtonSkin(text: "Cancel", icon: BootstrapIcon.X, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Close { get; } = new ButtonSkin(text: "Close", icon: BootstrapIcon.X, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Delete { get; } = new ButtonSkin(text: "Delete", icon: BootstrapIcon.Trash, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Edit { get; } = new ButtonSkin(text: "Edit", icon: BootstrapIcon.Pencil, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Export { get; } = new ButtonSkin(text: "Export", icon: BootstrapIcon.BoxArrowUp, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Insert { get; } = new ButtonSkin(text: "Insert", icon: BootstrapIcon.BoxArrowInRight, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin New { get; } = new ButtonSkin(text: "New", icon: BootstrapIcon.Plus, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin OK { get; } = new ButtonSkin(text: "OK", icon: BootstrapIcon.Check2, resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);
		public static ButtonSkin Open { get; } = new ButtonSkin(text: "Open", icon: BootstrapIcon.BoxArrowUpRight, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Remove { get; } = new ButtonSkin(text: "Remove", icon: BootstrapIcon.Trash, resourceType: typeof(ButtonSkins), color: ThemeColor.Secondary);
		public static ButtonSkin Save { get; } = new ButtonSkin(text: "Save", icon: BootstrapIcon.Check2, resourceType: typeof(ButtonSkins), color: ThemeColor.Primary);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
