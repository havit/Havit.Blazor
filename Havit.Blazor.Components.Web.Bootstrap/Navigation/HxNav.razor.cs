using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.0/components/navs-tabs/">Bootstrap Nav</see> component.
	/// </summary>
	public partial class HxNav
	{
		/// <summary>
		/// Orientation of the nav.
		/// Default is <see cref="NavOrientation.Horizontal"/>.
		/// </summary>
		[Parameter] public NavOrientation Orientation { get; set; } = NavOrientation.Horizontal;

		/// <summary>
		/// ID of the nav which can be used for <see cref="HxScrollspy.TargetId"/>.
		/// </summary>
		[Parameter] public string Id { get; set; } = "hx-" + Guid.NewGuid().ToString("N");

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Content of the nav.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
