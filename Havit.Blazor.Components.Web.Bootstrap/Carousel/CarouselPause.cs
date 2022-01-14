using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Behavior of the <see cref="HxCarousel"/>.
	/// </summary>
	public enum CarouselPause
	{
		/// <summary>
		/// Carousel will stop sliding on hover.
		/// </summary>
		Hover = 0,

		/// <summary>
		/// Carousel won't stop sliding on hover.
		/// </summary>
		False = 1
	}
}
