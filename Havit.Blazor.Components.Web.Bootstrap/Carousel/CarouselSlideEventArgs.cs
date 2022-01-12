using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class CarouselSlideEventArgs
	{
		/// <summary>
		/// Slide from which the sliding transition began.
		/// </summary>
		public int From { get; set; }
		/// <summary>
		/// Slide to which the carousel started sliding or has slid.
		/// </summary>
		public int To { get; set; }
		/// <summary>
		/// Direction of sliding (<c>left</c> or <c>right</c>).
		/// </summary>
		public string Direction { get; set; }
	}
}
