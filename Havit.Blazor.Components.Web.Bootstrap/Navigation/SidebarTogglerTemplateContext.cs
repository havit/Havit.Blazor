using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context provided to the <see cref="HxSidebar.TogglerTemplate" />.
/// </summary>
public class SidebarTogglerTemplateContext
{
	/// <summary>
	/// Indicates whether a <see cref="HxSidebar"/> is collapsed or expanded.
	/// </summary>
	public bool Collapsed { get; set; }
}
