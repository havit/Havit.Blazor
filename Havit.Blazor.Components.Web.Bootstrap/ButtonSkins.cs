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

		public static ButtonSkin Apply { get; } = new ButtonSkin(text: "Apply", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Cancel { get; } = new ButtonSkin(text: "Cancel", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Close { get; } = new ButtonSkin(text: "Close", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Delete { get; } = new ButtonSkin(text: "Delete", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Edit { get; } = new ButtonSkin(text: "Upravit", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Export { get; } = new ButtonSkin(text: "Export", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Insert { get; } = new ButtonSkin(text: "Insert", resourceType: typeof(ButtonSkins));
		public static ButtonSkin New { get; } = new ButtonSkin(text: "New", resourceType: typeof(ButtonSkins));
		public static ButtonSkin OK { get; } = new ButtonSkin(text: "OK", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Open { get; } = new ButtonSkin(text: "Otevřít", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Remove { get; } = new ButtonSkin(text: "Remove", resourceType: typeof(ButtonSkins));
		public static ButtonSkin Save { get; } = new ButtonSkin(text: "Save", resourceType: typeof(ButtonSkins));

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
