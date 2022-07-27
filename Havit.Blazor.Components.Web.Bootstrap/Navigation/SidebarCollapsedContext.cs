using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context indicating whether a <see cref="HxSidebar"/> is collapsed or expanded.
/// </summary>
public class SidebarCollapsedContext
{
	/// <summary>
	/// Indicates whether a <see cref="HxSidebar"/> is collapsed or expanded.
	/// </summary>
	public bool Collapsed { get; set; }
}
