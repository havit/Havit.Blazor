using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxPlaceholderContainer"/> component.
	/// </summary>
	public class PlaceholderContainerSettings
	{
		/// <summary>
		/// Animation of the placeholders in container. Default is <see cref="PlaceholderAnimation.None"/> (no animation).
		/// </summary>
		public PlaceholderAnimation Animation { get; set; } = PlaceholderAnimation.None;

		/// <summary>
		/// Additional CSS class for <see cref="HxPlaceholderContainer"/>.
		/// </summary>
		public string CssClass { get; set; }
	}
}
