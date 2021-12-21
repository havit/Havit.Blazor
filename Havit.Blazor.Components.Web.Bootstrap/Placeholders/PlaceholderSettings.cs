using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxPlaceholder"/> component.
	/// </summary>
	public class PlaceholderSettings
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
		/// Additional CSS class for <see cref="HxPlaceholder"/>.
		/// </summary>
		public string CssClass { get; set; }
	}
}
