namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context provided to the <see cref="HxSidebarBrand.LogoTemplate" />.
/// </summary>
public class SidebarBrandLogoTemplateContext
{
	/// <summary>
	/// Indicates whether a <see cref="HxSidebar"/> is collapsed or expanded.
	/// </summary>
	public bool SidebarCollapsed { get; set; }
}
