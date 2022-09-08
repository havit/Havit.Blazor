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
