using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class PlaceholderContainerDefaults
	{
		/// <summary>
		/// Animation of the placeholders in container. Default is <see cref="PlaceholderAnimation.None"/> (no animation).
		/// </summary>
		public PlaceholderAnimation Animation { get; set; } = PlaceholderAnimation.None;

		/// <summary>
		/// Additional CSS class for <see cref="HxPlaceholder"/>s (for the children of <see cref="HxPlaceholderContainer"/>s).
		/// </summary>
		public string CssClass { get; set; }
	}
}
