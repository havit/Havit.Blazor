using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Variations for <see cref="HxNav"/>.
	/// </summary>
	/// <remarks>Can be switched to [Flags] when needed.</remarks>
	public enum NavVariant
	{
		Standard = 0,

		/// <summary>
		/// Tabs. <see href="https://getbootstrap.com/docs/5.1/components/navs-tabs/#tabs"/>.
		/// Remember to set <c>active</c> tab (<see cref="HxNavLink.CssClass"/>).
		/// </summary>
		Tabs = 1,

		/// <summary>
		/// <see href="https://getbootstrap.com/docs/5.1/components/navs-tabs/#pills"/>
		/// </summary>
		Pills = 3
	}
}
