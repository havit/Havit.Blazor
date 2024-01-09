namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context provided to the <see cref="HxSidebarItem.ContentTemplate" />.
/// </summary>
public class SidebarItemContentTemplateContext
{
	/// <summary>
	/// Indicates whether a <see cref="HxSidebarItem"/> is collapsed or expanded. Is <c>null</c> when the item has no expandable content.
	/// </summary>
	public bool? ItemExpanded { get; set; }
}
