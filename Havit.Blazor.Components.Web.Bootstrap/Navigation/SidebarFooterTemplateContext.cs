using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context provided to the <see cref="HxSidebar.FooterTemplate" />.
/// </summary>
public class SidebarFooterTemplateContext
{
	/// <summary>
	/// Indicates whether the containing <see cref="HxSidebar"/> is collapsed or expanded.
	/// </summary>
	public bool SidebarCollapsed { get; set; }
}
