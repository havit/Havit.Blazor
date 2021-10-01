using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxPlaceholder.Defaults"/>.
	/// </summary>
	public class PlaceholderDefaults
	{
		/// <summary>
		/// Size of the placeholder. Default is <see cref="PlaceholderSize.Regular"/>.
		/// </summary>
		public PlaceholderSize Size { get; set; } = PlaceholderSize.Regular;

		/// <summary>
		/// Color of the placeholder. Default is <see cref="ThemeColor.None"/> (= use Bootstrap default placeholder color).
		/// </summary>
		public ThemeColor Color { get; set; } = ThemeColor.None;

		/// <summary>
		/// Animation of the placeholders in container. Default is <see cref="PlaceholderAnimation.None"/> (no animation).
		/// </summary>
		public PlaceholderAnimation Animation { get; set; } = PlaceholderAnimation.None;

		/// <summary>
		/// Additional CSS class for the <see cref="HxPlaceholderContainer"/>.
		/// </summary>
		public string ContainerCssClass { get; set; }

		/// <summary>
		/// Additional CSS class for <see cref="HxPlaceholder"/>s (for the children of <see cref="HxPlaceholderContainer"/>s).
		/// </summary>
		public string CssClass { get; set; }
	}
}
