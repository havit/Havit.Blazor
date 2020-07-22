using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Icons
{
	/// <summary>
	/// Base class for icons.
	/// </summary>
	public abstract class IconBase
	{
		/// <summary>
		/// Renderer of the icon. Must have a Icon property which receives the instance of the icon (IconBase descendant instance).
		/// See <see cref="BootstrapIcon"/>, <see cref="HxBootstrapIcon"/> and <see cref="HxIcon"/> implementations for more details.
		/// </summary>
		public abstract Type RendererComponentType { get; }
	}
}
