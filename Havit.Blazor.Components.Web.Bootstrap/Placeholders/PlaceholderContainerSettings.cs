using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxPlaceholderContainer"/> and derived components.
	/// </summary>
	public record PlaceholderContainerSettings
	{
		/// <summary>
		/// Animation of the placeholders in container.
		/// </summary>
		public PlaceholderAnimation? Animation { get; set; }

		/// <summary>
		/// Additional CSS class for <see cref="HxPlaceholderContainer"/>.
		/// </summary>
		public string CssClass { get; set; }
	}
}
